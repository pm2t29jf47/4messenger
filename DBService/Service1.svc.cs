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

namespace DBService
{
    public class Service1 : IService1
    {

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Employee> GetAllEmployees()
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            return EmployeeGateway.SelectAll(currentUsername);
        }
        
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SendMessage(Message message, List<Recipient> recipients)
        {
            
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            ///нельзя отсылать письма под чужим именем   
            if (string.Compare(currentUsername, message.SenderUsername) != 0)
                return;

            message.Date = DateTime.Now;
            int? insertedMessageId = MessageGateway.Insert(message, currentUsername);
            if (insertedMessageId == null) return;
            foreach (var item in recipients)
            {
                item.MessageId = (int)insertedMessageId;
                RecipientGateway.Insert(item, currentUsername);
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Message> GetInboxMessages()
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            List<Message> messages = new List<Message>();
            List<Recipient> recipients = RecipientGateway.SelectBy_RecipientUsername_Deleted(currentUsername,false);
            foreach (var recipient in recipients)
                ///recipient.MessageId не может быть null, тк берется из базы
                messages.Add(MessageGateway.SelectById((int)recipient.MessageId, currentUsername));
            return messages;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void CheckUser(){ }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Message> GetSentboxMessages()
        {
            ///добавить проверку на удаленность
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            return MessageGateway.SelectBy_SenderUsername_Deleted(currentUsername,false);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SetInboxMessageViewed(int messageId)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            RecipientGateway.UpdateViewed(currentUsername, messageId, true); 
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Message> GetDeletedMessages()
        {
            string curentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            var deletedMessages = new List<Message>();
            var userInRecipients = RecipientGateway.SelectBy_RecipientUsername_Deleted(curentUsername, true);
            foreach (var recipient in userInRecipients)
            {
                var receivedMessage = MessageGateway.SelectById((int)recipient.MessageId, curentUsername);
                ///recipient.MessageId не может быть null, тк берется из базы
                deletedMessages.Add(receivedMessage);
            }           
            var a = MessageGateway.SelectBy_SenderUsername_Deleted(curentUsername,true);
            var sentMessages = MessageGateway.SelectBy_SenderUsername_Deleted(curentUsername, true);
            deletedMessages.AddRange(sentMessages);
            return deletedMessages;      
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public Employee GetEmployee(string selectableUsername)
        {
            string curentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            return EmployeeGateway.SelectByUsername(selectableUsername, curentUsername);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SetInboxMessageDeleted(int MessageId)
        {
            throw new NotImplementedException();
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Recipient> GetRecipients(int messageId)
        {
            string curentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            return RecipientGateway.SelectByMessageId(messageId, curentUsername);
        }
    }
}