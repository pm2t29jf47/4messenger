using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Threading;
using System.Collections;
using DBService;
using System.ComponentModel;

namespace WPFClient.Additional
{
    /// <summary>
    /// im watching u!
    /// </summary>
    public class ServiceWatcher : IService1
    {      
        public ServiceWatcher(IService1 proxy, TimeSpan timeSpan)
        {     
            this.Proxy = proxy;            
            this.TimeSpan = timeSpan;           
        }

        public ServiceWatcher()
        {

        }
     
        public void StartWatching()
        {
            timer.Interval = TimeSpan;
            timer.Tick += new EventHandler(OntimerTick);
            DownloadData();
            CreateDataUpdatedEvent(new PropertyChangedEventArgs(""));
            timer.Start();
        }


        /// <summary>
        /// Хранит прокси класс для обращения к методам сервиса
        /// </summary>
        IService1 Proxy { get; set; }

        TimeSpan TimeSpan { get; set; }

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
                allEmployees = Proxy.GetAllEmployees();
                CreateDataUpdatedEvent(new PropertyChangedEventArgs("allEmployees"));
                inboxMessages = Proxy.GetInboxMessages();
                CreateDataUpdatedEvent(new PropertyChangedEventArgs("inboxMessages"));
                sentboxMessages = Proxy.GetSentboxMessages();
                CreateDataUpdatedEvent(new PropertyChangedEventArgs("sentboxMessages"));
                deletedInboxMessages = Proxy.GetDeletedInboxMessages();
                CreateDataUpdatedEvent(new PropertyChangedEventArgs("deletedInboxMessages"));
                deletedSentboxMessages = Proxy.GetDeletedSentboxMessages();
                CreateDataUpdatedEvent(new PropertyChangedEventArgs("deletedSentboxMessages"));
            }
            catch (System.ServiceModel.FaultException ex1)
            {
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex1, "()WPFClient.Additional.ServiceWatcher.DownloadData()");
                throw ex1;
            }
            catch (Exception ex2)
            {
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex2, "()WPFClient.Additional.ServiceWatcher.DownloadData()");
                throw ex2;
            }
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
            try
            {
                Proxy.CheckUser();
            }
            catch (Exception ex)
            {
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.Additional.ServiceWatcher.CheckUser()");
                throw ex;
            }
        }

        public List<Employee> GetAllEmployees()
        {
            return allEmployees;             
        }

        public Employee GetEmployee(string username)
        {
            try
            {
                return Proxy.GetEmployee(username);
            }
            catch (Exception ex)
            {
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "(Employee)WPFClient.Additional.ServiceWatcher.GetEmployee(string username)");
                throw ex;
            }
        }

        public void SendMessage(Message message)
        {
            var a = ((System.ServiceModel.Channels.IChannel)Proxy).State;
            try
            {
                Proxy.SendMessage(message);
            }
            catch (Exception ex)
            {
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.Additional.ServiceWatcher.SendMessage(Message message)");
                throw ex;
            }
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
            try
            {
                Proxy.SetRecipientViewed(messageId, viewed);
            }
            catch (Exception ex)
            {
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.Additional.ServiceWatcher.SetRecipientViewed(int messageId, bool viewed)");
                throw ex;
            }
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

