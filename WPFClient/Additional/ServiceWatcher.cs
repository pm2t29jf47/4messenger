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

        TimeSpan TimeSpan { get; set; }

        void CreateDataUpdatedEvent(PropertyChangedEventArgs e)
        {
            if (DataUpdated != null)
                DataUpdated(this, e);
        }

        public event eventHandler DataUpdated;

        public delegate void eventHandler(object sender, PropertyChangedEventArgs e);

        List<Employee> allEmployees;

        public List<Employee> AllEmployees 
        {
            get
            {
                if (allEmployees == null) 
                    AllEmployees = Proxy.GetAllEmployees();                    
                
                    return allEmployees;                
            }
            set
            {
                if (allEmployees != value)
                {
                    allEmployees = value;
                    CreateDataUpdatedEvent(new PropertyChangedEventArgs("AllEmployees"));                    
                }
            }
        }

        void OntimerTick(object sender, EventArgs e)
        {
            var a = new EventArgs();
            CreateDataUpdatedEvent(new PropertyChangedEventArgs(""));
        }

        public void CheckUser()
        {
            Proxy.CheckUser();
        }
     
        public List<Employee> GetAllEmployees()
        {
            return AllEmployees;
        }

        public Employee GetEmployee(string username)
        {
            ///ищем локально, если не нашли, то перезагружаем список сотрудников и ищем опять
            Employee result = AllEmployees.FirstOrDefault(row => string.Compare(row.Username, username) == 0);
            if (result == null)
            {
                AllEmployees = Proxy.GetAllEmployees();
                result = AllEmployees.FirstOrDefault(row => string.Compare(row.Username, username) == 0);
            }
            return result;             
        }

        public List<Recipient> GetRecipients(int MessageId)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(Message message, List<Recipient> recipient)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetInboxMessages()
        {
            throw new NotImplementedException();
        }

        public List<Message> GetDeletedMessages()
        {
            throw new NotImplementedException();
        }

        public List<Message> GetSentboxMessages()
        {
            throw new NotImplementedException();
        }

        public void SetInboxMessageViewed(int messageId)
        {
            throw new NotImplementedException();
        }

        public void SetInboxMessageDeleted(int messageId)
        {
            throw new NotImplementedException();
        }
    }
}
