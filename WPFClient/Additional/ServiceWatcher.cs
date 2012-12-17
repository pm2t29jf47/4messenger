using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using Entities.Additional;
using System.Threading;
using System.Collections;
using DBService;
using System.ComponentModel;
using System.ServiceModel;
using ServiceInterface;
using System.Data.SqlTypes;

namespace WPFClient.Additional
{
    /// <summary>
    /// im watching u!
    /// </summary>
    public class ServiceWatcher
    {  
        public ServiceWatcher( TimeSpan timeSpan)
        {
            this.timer.Interval = timeSpan;
            this.timer.Tick += new EventHandler(OntimerTick);
            this.AllEmployees = new List<Employee>();
            this.InboxMessages = new List<Message>();
            this.SentboxMessages = new List<Message>();
            this.DeletedInboxMessages = new List<Message>();
            this.DeletedSentboxMessages = new List<Message>();
            this.ViewedDeletedInboxMessages = new List<Message>();
            this.ViewedInboxMessages = new List<Message>();
           
        }
     
        public void StartWatching()
        {            
            DownloadData();           
            timer.Start();
        }

        public void StopWatching()
        {
            timer.Stop();
        }


        /// <summary>
        /// Хранит прокси класс для обращения к методам сервиса
        /// </summary>
        IService1 Proxy { get; set; }

        ChannelFactory<IService1> factory = new ChannelFactory<IService1>("*");

        public string FactoryUsername
        {
            get
            {
                return factory.Credentials.UserName.UserName;
            }
            set
            {
                if (string.Compare(factory.Credentials.UserName.UserName, value) != 0)
                {
                    factory.Credentials.UserName.UserName = value;
                }
            }
        }

        public string FactoryPassword
        {
            get
            {
                return factory.Credentials.UserName.Password;
            }
            set
            {
                if (string.Compare(factory.Credentials.UserName.Password, value) != 0)
                {
                    factory.Credentials.UserName.Password = value;
                }
            }
        }

        public void CreateChannel()
        {
            DestroyCurrentChannel();
            Proxy = factory.CreateChannel();
        }

        public void DestroyCurrentChannel()
        {
            if (!IService1.Equals(Proxy, null))
            {
                ((System.ServiceModel.Channels.IChannel)Proxy).Close();
                Proxy = null;
            }
        }
  

        public Exception DataDownloadException { get; private set; }

        public List<Employee> AllEmployees { get; private set; }

        public List<Message> InboxMessages { get; private set; }

        public List<Message> ViewedInboxMessages { get; private set; }

        public List<Message> DeletedInboxMessages { get; private set; }

        public List<Message> ViewedDeletedInboxMessages { get; private set; }

        public List<Message> SentboxMessages { get; private set; }

        public List<Message> DeletedSentboxMessages { get; private set; }


        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        void OntimerTick(object sender, EventArgs e)
        {
            timer.Stop();
            DownloadData();
            timer.Start();
            CreateDataUpdatedEvent(new PropertyChangedEventArgs("AllData"));
        }

        void DownloadData()
        {
            try
            {
                DataDownloadException = null;
                UpdateEmployees(AllEmployees);   

                MessageTypes messageTypes = new MessageTypes();
                UpdateMessages(FolderType.inbox,messageTypes, InboxMessages);

                messageTypes = MessageTypes.viewed;
                UpdateMessages(FolderType.inbox, messageTypes, ViewedInboxMessages);

                messageTypes = MessageTypes.deleted;
                UpdateMessages(FolderType.inbox, messageTypes, DeletedInboxMessages);

                messageTypes = MessageTypes.deleted | MessageTypes.viewed;
                UpdateMessages(FolderType.inbox, messageTypes, ViewedDeletedInboxMessages);

                messageTypes = new MessageTypes();
                UpdateMessages(FolderType.sentbox, messageTypes, SentboxMessages);

                messageTypes = MessageTypes.deleted;
                UpdateMessages(FolderType.sentbox, messageTypes, DeletedSentboxMessages);               
            }

            /// Сервис не отвечает
            catch (EndpointNotFoundException ex)
            {
                HandleDownloadDataException(ex);
            }

            ///Креденшелы не подходят
            catch (System.ServiceModel.Security.MessageSecurityException ex) 
            {
                HandleDownloadDataException(ex);
            }

            /// Ошибка в сервисе
            /// (маловероятна, при таком варианте скорее сработает ошибка креденшелов,
            /// т.к. проверка паролей происходит на каждом запросе к сервису и ей необходима БД)
            catch (FaultException<System.ServiceModel.ExceptionDetail> ex) 
            {
                HandleDownloadDataException(ex);
            }
                
            /// Остальные исключения
            catch (Exception ex) 
            {
                HandleDownloadDataException(ex);
                throw; ///Неизвестное исключение пробасывается дальше
            }            
        }

        void UpdateEmployees(List<Employee> employees)
        {
            List<Entity> entities = new List<Entity>();
            entities.AddRange(employees);
            byte[] recentVersion = GetMaxTimestamp(entities);
            EmployeePack pack = Proxy.GetAllEmployees(recentVersion);
            if (!UpdateEmployeesByPack(pack, employees))
            {
                List<string> usernamesCollection = Proxy.GetAllEmployeesIds();
                TrimEmployees(usernamesCollection, employees);
            }
        }

        void UpdateMessages(FolderType folderType,MessageTypes messageTypes, List<Message> messages)
        {
            List<Entity> entities = new List<Entity>();
            entities.AddRange(messages);
            byte[] recentVersion = GetMaxTimestamp(entities); 
            MessagesPack pack = Proxy.GetMessages(folderType, messageTypes, recentVersion);
            if (!UpdateMessagesByPack(pack, messages))
            {
                List<int> idCollection = Proxy.GetMessagesIds(folderType, messageTypes);
                TrimMessages(idCollection, messages);
            }
        }

        byte[] GetMaxTimestamp(List<Entity> Entities)
        {
            byte[] currentMax = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            SqlBinary sqlCurrentMax = new SqlBinary(currentMax);
            SqlBinary sqlCurrent;
            foreach (Entity item in Entities)
            {
                sqlCurrent = new SqlBinary(item.Version);
                if (sqlCurrentMax.CompareTo(sqlCurrent) < 0)
                    sqlCurrentMax = sqlCurrent;                
            }
            return sqlCurrentMax.Value;
        }

        bool UpdateMessagesByPack(MessagesPack pack, List<Message> messages)
        {
            if (pack.Messages.Count > 0)
            {
                Message message;
                foreach (Message item in pack.Messages)
                {
                    message = messages.FirstOrDefault(row => row.Id == item.Id);
                    if (message == null)
                    {
                        messages.Add(item);
                    }
                    else
                    {
                        messages.Remove(message);
                        messages.Add(item);
                    }
                }
                /// Восстанавливаем нормальный порядок
                messages = messages.OrderBy(row => row.Date).ToList();
            }
            return (messages.Count == pack.CountInDB);
        }

        bool UpdateEmployeesByPack(EmployeePack pack, List<Employee> employees)
        {
            if (pack.Employees.Count > 0)
            {
                Employee employee;
                foreach (Employee item in pack.Employees)
                {
                    employee = employees.FirstOrDefault(row => string.Compare(row.Username, item.Username) == 0);
                    if (employee == null)
                    {
                        employees.Add(item);
                    }
                    else
                    {
                        employees.Remove(employee);
                        employees.Add(item);
                    }
                }
                /// Восстанавливаем нормальный порядок
                employees = employees.OrderBy(row => row.SecondName).ToList();
            }
            return (employees.Count == pack.CountInDB);
        }

        void TrimMessages(List<int> ids, List<Message> messages)
        {
            List<Message> removed = new List<Message>();
            foreach (Message item in messages)
            {
                if (!ids.Contains((int)item.Id))
                {
                    removed.Add(item);
                }
            }
            foreach (Message item in removed)
            {
                messages.Remove(item);
            }
        }

        void TrimEmployees(List<string> usernames, List<Employee> employees)
        {
            List<Employee> removed = new List<Employee>();
            foreach (Employee item in employees)
            {
                if (!usernames.Contains(item.Username))
                {
                    removed.Add(item);
                }
            }
            foreach (Employee item in removed)
            {
                employees.Remove(item);
            }
        }

        void HandleDownloadDataException(Exception ex)
        {
            ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.Additional.ServiceWatcher.DownloadData()");
            DataDownloadException = ex;
        }
        

        void CreateDataUpdatedEvent(PropertyChangedEventArgs e)
        {
            if (DataUpdated != null)
                DataUpdated(this, e);
        }

        public event eventHandler DataUpdated;

        public delegate void eventHandler(object sender, PropertyChangedEventArgs e);
        

        public void CheckUser()
        {
            Proxy.CheckUser(); 
        }

        public Employee GetEmployee(string username)
        {
            return Proxy.GetEmployee(username);  
        }

        public void SendMessage(Message message)
        {
            Proxy.SendMessage(message);
        }  

        public void SetRecipientViewed(int messageId, bool viewed)
        {
            Proxy.SetRecipientViewed(messageId, viewed);           
        }

        public void SetRecipientDeleted(int messageId, bool deleted)
        {
            Proxy.SetRecipientDeleted(messageId, deleted);
        }

        public void SetMessageDeleted(int messageId, bool deleted)
        {
            Proxy.SetMessageDeleted(messageId, deleted);
        }

        public void PermanentlyDeleteRecipient(int messageId)
        {
            Proxy.PermanentlyDeleteRecipient(messageId);
        }

        public void PermanentlyDeleteMessage(int messageId)
        {
            Proxy.PermanentlyDeleteMessage(messageId);
        }      
    }
}