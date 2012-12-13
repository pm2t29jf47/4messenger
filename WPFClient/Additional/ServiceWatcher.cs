using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Threading;
using System.Collections;
using DBService;
using System.ComponentModel;
using System.ServiceModel;

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

        List<Message> inboxMessages = new List<Message>();

        List<Message> sentboxMessages = new List<Message>();

        List<Message> deletedInboxMessages = new List<Message>();

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
                allEmployees = Proxy.GetAllEmployees();
                inboxMessages = Proxy.GetInboxMessages();
                sentboxMessages = Proxy.GetSentboxMessages();
                deletedInboxMessages = Proxy.GetDeletedInboxMessages();
                deletedSentboxMessages = Proxy.GetDeletedSentboxMessages();
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
            return inboxMessages;         
        }

        public List<Message> GetDeletedInboxMessages()
        {
            return deletedInboxMessages;
        }

        public List<Message> GetSentboxMessages()
        {
            return sentboxMessages;
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
    }
}

