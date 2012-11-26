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
    public class ServiceWatcher : IService1
    {
        /// <summary>
        /// im watching u!
        /// </summary>
      
        public ServiceWatcher(IService1 proxy, TimeSpan timeSpan)
        {     
            this.Proxy = proxy;            
            this.TimeSpan = timeSpan;           
        }

        public ServiceWatcher()
        {

        }
     
        public void StartWatch()
        {
            timer.Interval = TimeSpan;
            timer.Tick += new EventHandler(OntimerTick);
            CreateDataUpdatedEvent(new PropertyChangedEventArgs(""));
            timer.Start();
        }

        /// <summary>
        /// Хранит прокси класс для обращения к методам сервиса
        /// </summary>
        IService1 Proxy { get; set; }

        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        void OntimerTick(object sender, EventArgs e)
        {
            UpdateData();
        }

        public void UpdateData()
        {
            allEmployees = Proxy.GetAllEmployees();
            CreateDataUpdatedEvent(new PropertyChangedEventArgs("allEmployees"));
            inboxMessages = Proxy.GetInboxMessages();
            CreateDataUpdatedEvent(new PropertyChangedEventArgs("inboxMessages"));
            sentboxMessages = Proxy.GetSentboxMessages();
            CreateDataUpdatedEvent(new PropertyChangedEventArgs("sentboxMessages"));
            deletedMessages = Proxy.GetDeletedMessages();
            CreateDataUpdatedEvent(new PropertyChangedEventArgs("deletedMessages"));
        }

        TimeSpan TimeSpan { get; set; }

        void CreateDataUpdatedEvent(PropertyChangedEventArgs e)
        {
            if (DataUpdated != null)
                DataUpdated(this, e);
        }

        public event eventHandler DataUpdated;

        public delegate void eventHandler(object sender, PropertyChangedEventArgs e);

        List<Employee> allEmployees = new List<Employee>();

        List<Message> inboxMessages = new List<Message>();

        List<Message> sentboxMessages = new List<Message>();

        List<Message> deletedMessages = new List<Message>();

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

        public List<Recipient> GetRecipients(int MessageId)
        {
            return Proxy.GetRecipients(MessageId);
        }

        public void SendMessage(Message message, List<Recipient> recipient)
        {
            Proxy.SendMessage(message, recipient);
            sentboxMessages = Proxy.GetSentboxMessages();
            CreateDataUpdatedEvent(new PropertyChangedEventArgs("sentboxMessages"));
        }

        public List<Message> GetInboxMessages()
        {
            return inboxMessages;         
        }

        public List<Message> GetDeletedMessages()
        {
            return deletedMessages;
        }

        public List<Message> GetSentboxMessages()
        {
            return sentboxMessages;
        }

        public void SetInboxMessageViewed(int messageId)
        {
            Proxy.SetInboxMessageViewed(messageId);
            inboxMessages = Proxy.GetInboxMessages();
            CreateDataUpdatedEvent(new PropertyChangedEventArgs("inboxMessages"));
        }

        public void SetInboxMessageDeleted(int messageId)
        {
            Proxy.SetInboxMessageDeleted(messageId);
            inboxMessages = Proxy.GetInboxMessages();
            CreateDataUpdatedEvent(new PropertyChangedEventArgs("inboxMessages"));
            deletedMessages = Proxy.GetDeletedMessages();
            CreateDataUpdatedEvent(new PropertyChangedEventArgs("deletedMessages"));    
        }
    }
}
