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
            sentboxMessages.Add(message);
            CreateDataUpdatedEvent(new PropertyChangedEventArgs("sentboxMessages"));
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
            UpdateLocalInboxMessages(messageId, viewed);
            UpdateLocalSentboxMessages(messageId, viewed);
            UpdateLocalDeletedInboxMessages(messageId, viewed);
            UpdateLocalDeletedSentboxMessages(messageId, viewed);  
        }

        void UpdateLocalInboxMessages(int messageId, bool viewed)
        {
            Message message = inboxMessages.FirstOrDefault(row => row.Id == messageId);
            if (message != null)
            {
                Recipient recipient = message.EDRecipient_MessageId.FirstOrDefault(row => string.Compare(row.RecipientUsername, App.Username) == 0);
                if (recipient != null)
                {
                    recipient.Viewed = viewed;
                    CreateDataUpdatedEvent(new PropertyChangedEventArgs("inboxMessages"));                    
                }
            }
        }

        void UpdateLocalSentboxMessages(int messageId, bool viewed)
        {

            Message message = sentboxMessages.FirstOrDefault(row => row.Id == messageId);
            if (message != null)
            {
                Recipient recipient = message.EDRecipient_MessageId.FirstOrDefault(row => string.Compare(row.RecipientUsername, App.Username) == 0);
                if (recipient != null)
                {
                    recipient.Viewed = viewed;                   
                    CreateDataUpdatedEvent(new PropertyChangedEventArgs("sentboxMessages"));
                }
            }
        }

        void UpdateLocalDeletedInboxMessages(int messageId, bool viewed)
        {

            Message message = deletedInboxMessages.FirstOrDefault(row => row.Id == messageId);
            if (message != null)
            {
                Recipient recipient = message.EDRecipient_MessageId.FirstOrDefault(row => string.Compare(row.RecipientUsername, App.Username) == 0);
                if (recipient != null)
                {
                    recipient.Viewed = viewed;        
                    CreateDataUpdatedEvent(new PropertyChangedEventArgs("deletedInboxMessages"));
                }
            }
        }

        void UpdateLocalDeletedSentboxMessages(int messageId, bool viewed)
        {

            Message message = deletedSentboxMessages.FirstOrDefault(row => row.Id == messageId);
            if (message != null)
            {
                Recipient recipient = message.EDRecipient_MessageId.FirstOrDefault(row => string.Compare(row.RecipientUsername, App.Username) == 0);
                if (recipient != null)
                {
                    recipient.Viewed = viewed;
                    CreateDataUpdatedEvent(new PropertyChangedEventArgs("deletedSentboxMessages"));
                }
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

