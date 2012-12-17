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
using Entities.Additional;
using ServiceInterface;
using System.Data.SqlTypes;

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
            message.Sender = EmployeeGateway.Select(message.SenderUsername, currentUsername);
            message.Recipients = RecipientGateway.Select((int)message.Id, currentUsername);
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
            return EmployeeGateway.Select(selectableUsername, currentUsername);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SetRecipientViewed(int messageId, bool viewed)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            RecipientGateway.Update(currentUsername, messageId, viewed, currentUsername);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SetRecipientDeleted(int messageId, bool deleted)
        {
            ///
        }

        public MessagesPack GetMessages(FolderType folderType, MessageTypes messageTypes, Byte[] recentVersion)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            switch (folderType)
            {
                case FolderType.inbox:
                    {
                        return GetInboxMessages(currentUsername, messageTypes, recentVersion);
                    }
                default:
                    {
                        return GetSentboxMessages(currentUsername, messageTypes, recentVersion);
                    }
            }            
        }

        MessagesPack GetInboxMessages(string currentUsername, MessageTypes messageTypes, Byte[] recentVersion)
        {
            List<Message> recievedMessages = new List<Message>();
            Message result;
            bool deleted = messageTypes.HasFlag(MessageTypes.deleted);
            bool viewed = messageTypes.HasFlag(MessageTypes.viewed);
            ///Ищем в получателях
            List<Recipient> recipients = RecipientGateway.Select(currentUsername, currentUsername, deleted, viewed);
            foreach (Recipient item in recipients)
            {
                result = MessageGateway.Select((int)item.MessageId, currentUsername);                
                recievedMessages.Add(result);
            }
            MessagesPack messagesPack = new MessagesPack();
            //Сумма Id всех полученных сообщений
            messagesPack.CountInDB = recievedMessages.Count();
            ///Сообщения с версией выше чем у клиента
            messagesPack.Messages = GetRecentVersionMessages(recievedMessages, recentVersion);
            ///Звполнить Sender, Recipients
            foreach (Message item in messagesPack.Messages)
            {
                FillMessage(item, currentUsername);
            }
            return messagesPack;
        }

        MessagesPack GetSentboxMessages(string currentUsername, MessageTypes messageTypes, Byte[] recentVersion)
        {
            bool deleted = messageTypes.HasFlag(MessageTypes.deleted);
            List<Message> sentMessages = MessageGateway.Select(currentUsername, currentUsername, deleted);
            MessagesPack messagesPack = new MessagesPack();
            messagesPack.CountInDB = sentMessages.Count();
            messagesPack.Messages = GetRecentVersionMessages(sentMessages, recentVersion);
            foreach (Message item in messagesPack.Messages)
            {
                FillMessage(item, currentUsername);
            }
            return messagesPack;
        } 

        List<Message> GetRecentVersionMessages(List<Message> messages, byte[] recentVersion)
        {
            List<Message> result = new List<Message>();
            SqlBinary sqlRecentVersion = new SqlBinary(recentVersion);
            foreach (Message item in messages)
            {
                SqlBinary sqlCurrentVersion = new SqlBinary(item.Version);
                if (sqlCurrentVersion.CompareTo(sqlRecentVersion) > 0)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<int> GetMessagesIds(FolderType folderType, MessageTypes messageTypes)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            switch (folderType)
            {
                case FolderType.inbox:
                    {
                        return GetInboxMessagesIds(currentUsername, messageTypes);
                    }
                default:
                    {
                        return GetSentboxMessagesIds(currentUsername, messageTypes);
                    }
            }
        }

        List<int> GetSentboxMessagesIds(string currentUsername, MessageTypes messageTypes)
        {
            bool deleted = messageTypes.HasFlag(MessageTypes.deleted);
            return MessageGateway.SelectIds(currentUsername, currentUsername, deleted);            
        }

        List<int> GetInboxMessagesIds(string currentUsername, MessageTypes messageTypes)
        {
            List<int> recievedMessagesIds = new List<int>();      
            bool deleted = messageTypes.HasFlag(MessageTypes.deleted);
            bool viewed = messageTypes.HasFlag(MessageTypes.viewed);
            ///Ищем в получателях
            List<Recipient> recipients = RecipientGateway.Select(currentUsername, currentUsername, deleted, viewed);
            foreach (Recipient item in recipients)
            {
                recievedMessagesIds.Add((int)item.MessageId);
            }
            return recievedMessagesIds;
        }
    }
}