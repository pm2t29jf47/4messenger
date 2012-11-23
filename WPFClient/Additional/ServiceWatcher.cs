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

        void OntimerTick(object sender, EventArgs e)
        {
            var a = new EventArgs();
            CreateDataUpdatedEvent(new PropertyChangedEventArgs(""));
        }

        TimeSpan TimeSpan { get; set; }

        void CreateDataUpdatedEvent(PropertyChangedEventArgs e)
        {
            if (DataUpdated != null)
                DataUpdated(this, e);
        }

        public event eventHandler DataUpdated;

        public delegate void eventHandler(object sender, PropertyChangedEventArgs e);

        List<Employee> allEmployees;

        List<Message> inboxMessages;

        List<Message> sentboxMessages;

        public void CheckUser()
        {
            Proxy.CheckUser();
        }
     
        public List<Employee> GetAllEmployees()
        {
            if (allEmployees == null)
            {
                allEmployees = Proxy.GetAllEmployees();
                CreateDataUpdatedEvent(new PropertyChangedEventArgs("allEmployees"));
            }
            return allEmployees;             
        }

        public Employee GetEmployee(string username)
        {
            
            ///если еще не загружали - подгружаем и ищем
            if (allEmployees == null)
            {
                allEmployees = Proxy.GetAllEmployees();
                CreateDataUpdatedEvent(new PropertyChangedEventArgs("allEmployees")); //потестить, когда сработает обработчик
                return allEmployees.FirstOrDefault(row => string.Compare(row.Username, username) == 0);
            }

            /// ищем локально, если не нашли, то перезагружаем список сотрудников и ищем опять
            Employee result = allEmployees.FirstOrDefault(row => string.Compare(row.Username, username) == 0);
            if (result == null)
            {
                allEmployees = Proxy.GetAllEmployees();
                CreateDataUpdatedEvent(new PropertyChangedEventArgs("allEmployees"));
                return allEmployees.FirstOrDefault(row => string.Compare(row.Username, username) == 0);
            }
            return result;             
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
            if (inboxMessages == null)
            { 
                inboxMessages = Proxy.GetInboxMessages();
                CreateDataUpdatedEvent(new PropertyChangedEventArgs("inboxMessages"));
            }
            return null;         
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
