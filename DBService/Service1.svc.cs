using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DataSourceLayer;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Threading;
using System.Security;
using System.Web.Security;
using Entities;
using ServiceInterface;

namespace DBService
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Service1 : IService1
    {
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Employee> GetAllEmployees()
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            return EmployeeGateway.SelectAll(currentUsername);
        }        
        
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SendMessage(Message message)
        {            
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            CheckMessage(message, currentUsername);    
            message.Date = DateTime.Now;
            int insertedMessageId = MessageGateway.Insert(message, currentUsername);
            foreach (Recipient item in message.Recipients)
            {
                Recipient recipient = new Recipient(item.RecipientUsername, insertedMessageId);
                RecipientGateway.Insert(recipient, currentUsername);
            }   
        }

        void CheckMessage(Message message, string currentUsername)
        {
            if (message == null)
            {
                ArgumentNullException ex = new ArgumentNullException("message");
                throw new FaultException<ArgumentNullException>(ex, ex.Message);
            }

            if (message.Recipients == null
                || message.Content == null
                || message.Sender == null
                || message.SenderUsername == null
                || message.Title == null)
            {
                ArgumentException ex = new ArgumentException("Only message.Id can be NULL");
                throw new FaultException<ArgumentException>(ex, ex.Message);
            }
            ///нельзя отсылать письма под чужим именем   
            if (string.Compare(currentUsername, message.SenderUsername) != 0)
            {
                ArgumentException ex = new ArgumentException("message.SenderUsername must be equal current username");
                throw new FaultException<ArgumentException>(ex, ex.Message);
            }
        }

        void FillMessage(Message message, string currentUsername)
        {
            if (message == null
                || message.Id == null
                || message.SenderUsername == null)
                return;
            message.Sender = EmployeeGateway.SelectByUsername(message.SenderUsername, currentUsername);
            message.Recipients = RecipientGateway.SelectByMessageId((int)message.Id, currentUsername);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void CheckUser() { }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public Employee GetEmployee(string selectableUsername)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            if (string.Equals(selectableUsername, null))
            {
                ArgumentNullException ex = new ArgumentNullException("selectableUsername");
                throw new FaultException<ArgumentNullException>(ex, ex.Message);
            }
            return EmployeeGateway.SelectByUsername(selectableUsername, currentUsername);
        }        

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SetRecipientViewed(int messageId, bool viewed)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            RecipientGateway.UpdateViewed(currentUsername, messageId, viewed);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SetRecipientDeleted(int messageId, bool deleted)
        {
           ///
        }
        
        public List<Message> GetMessages(Folder folder, bool deleted, bool viewed)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            switch (folder)
            {
                case Folder.inbox:
                    {
                        return GetInboxMessages(currentUsername, deleted, viewed);
                    }
                default:
                    {
                        return GetSentboxMessages(currentUsername, deleted);
                    }
            }            
        }

        List<Message> GetInboxMessages(string currentUsername, bool deleted, bool viewed)
        {
            List<Message> recievedMessages = new List<Message>();
            Message result;
            List<Recipient> recipients = RecipientGateway.Select(currentUsername, deleted, viewed);
            foreach (Recipient item in recipients)
            {
                result = MessageGateway.SelectById((int)item.MessageId, currentUsername);
                FillMessage(result, currentUsername);
                recievedMessages.Add(result);
            }
            return recievedMessages;
        }

        List<Message> GetSentboxMessages(string currentUsername, bool deleted)
        {
            List<Message> sentMessages = MessageGateway.SelectBy_SenderUsername_Deleted(currentUsername, deleted);
            foreach (Message item in sentMessages)
            {
                FillMessage(item, currentUsername);
            }
            return sentMessages;
        }
    }
}