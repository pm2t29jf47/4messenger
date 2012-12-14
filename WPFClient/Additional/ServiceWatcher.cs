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

namespace WPFClient.Additional
{
    /// <summary>
    /// im watching u!
    /// </summary>
    public class ServiceWatcher : IService1
    {  
        public ServiceWatcher( TimeSpan timeSpan)
        {
            timer.Interval = timeSpan;
            timer.Tick += new EventHandler(OntimerTick);
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
  

        public Exception DataDownloadException { get; set; } 

        List<Employee> allEmployees = new List<Employee>();

        List<Message> undeletedUnviewedInboxMessages = new List<Message>();

        List<Message> undeletedViewedInboxMessages = new List<Message>();

        List<Message> deletetUnviewedInboxMessages = new List<Message>();

        List<Message> deletedViewedInboxMessages = new List<Message>();

        List<Message> undeletedSentboxMessages = new List<Message>();

        List<Message> deletedSentboxMessages = new List<Message>();


        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        void OntimerTick(object sender, EventArgs e)
        {
            DownloadData();
        }

        void DownloadData()
        {
            try
            {
                DataDownloadException = null;
                allEmployees = Proxy.GetAllEmployees(); //?

               // undeletedUnviewedInboxMessages = Proxy.GetMessages(Folder.inbox, false, false, null);
               // undeletedViewedInboxMessages = Proxy.GetMessages(Folder.inbox, false, true, null);
             //   deletetUnviewedInboxMessages = Proxy.GetMessages(Folder.inbox, true, false, null);
             //   deletedViewedInboxMessages = Proxy.GetMessages(Folder.inbox, true, true, null);
             //   undeletedSentboxMessages = Proxy.GetMessages(Folder.sentbox, false, true, null);
            //    deletedSentboxMessages = Proxy.GetMessages(Folder.sentbox, true, true, null);


                var b = Proxy.GetMessages(Folder.sentbox, true, true, null);


                var a = CreateVersionedMessages(deletedSentboxMessages);
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
            CreateDataUpdatedEvent(new PropertyChangedEventArgs("AllData"));
        }

        List<VersionedMessage> CreateVersionedMessages(List<Message> messages)
        {
            List<VersionedMessage> versionedMessages = new List<VersionedMessage>();
            foreach (Message item in messages)
            {
                VersionedEmployee versionedEmployee = CreateVersionedEmployee(item.Sender);
                List<VersionedRecipient> versionedRecipients = CreateVersionedRecipients(item.Recipients);

                versionedMessages.Add(new VersionedMessage
                {
                    Id = (int)item.Id,
                    Recipients = versionedRecipients,
                    Sender = versionedEmployee,
                    Version = item.Version
                });
                
            }
            return versionedMessages;
        }


        List<VersionedRecipient> CreateVersionedRecipients(List<Recipient> recipients)
        {
            List<VersionedRecipient> versionedRecipients = new List<VersionedRecipient>();
            foreach(Recipient item in recipients)
            {
                versionedRecipients.Add(
                    new VersionedRecipient()
                    {
                        MessageId = (int)item.MessageId,
                        RecipientUsername = item.RecipientUsername,
                        Version = item.Version
                    });
            }
            return versionedRecipients;
        }

        VersionedEmployee CreateVersionedEmployee(Employee employee)
        {
            return new VersionedEmployee()
            {
                Username = employee.Username,
                Version = employee.Version,
            };
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

        public List<Employee> GetAllEmployees()
        {
            return allEmployees;             
        }

        public Employee GetEmployee(string username)
        {
            return Proxy.GetEmployee(username);  
        }

        public void SendMessage(Message message)
        {
            Proxy.SendMessage(message);
        }

        public List<Message> GetInboxMessages()
        {
            return null;//inboxMessages;         
        }

        public List<Message> GetDeletedInboxMessages()
        {
            return null;// deletedInboxMessages;
        }

        public List<Message> GetSentboxMessages()
        {
            return null;// sentboxMessages;
        }

        public void SetRecipientViewed(int messageId, bool viewed)
        {
            Proxy.SetRecipientViewed(messageId, viewed);           
        }

        public void SetRecipientDeleted(int messageId, bool deleted)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetDeletedSentboxMessages()
        {
            return deletedSentboxMessages;
        }

        public MessagesPack GetMessages(Folder folder, bool deleted, bool viewed, List<Entities.Additional.VersionedMessage> sourceCollection)
        {
            return null;
        }
    }
}