using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DataSourceLayer;
//using EFDataSourceLayer;
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
        public EmployeePack GetAllEmployees(byte[] recentVersion)
        {
            var a = OperationContext.Current.Host.Extensions.Count;
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            List<Employee> allEmployees = EmployeeGateway.SelectAll(currentUsername);
            EmployeePack pack = new EmployeePack();
            pack.Employees = GetRecentVersionEmployees(allEmployees, recentVersion);
            pack.CountInDB = allEmployees.Count;
            return pack;
        }

        List<Employee> GetRecentVersionEmployees(List<Employee> employees, byte[] recentVersion)
        {
            List<Employee> result = new List<Employee>();
            SqlBinary sqlRecentVersion = new SqlBinary(recentVersion);
            foreach (Employee item in employees)
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
        public List<string> GetAllEmployeesIds()
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            return EmployeeGateway.SelectIds(currentUsername); 
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SendMessage(Message message)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            CheckMessage(message, currentUsername);
            PrepareMessage(message);
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
            if (message.Recipients == null)
            {
                ArgumentException ex = new ArgumentException("message.Recipients cannot be NULL");
                throw new FaultException<ArgumentException>(ex, ex.Message);
            }
            if(message.Content == null)
            {
                ArgumentException ex = new ArgumentException("message.Content cannot be NULL");
                throw new FaultException<ArgumentException>(ex, ex.Message);
            }
            ///Можно нечаянно добавить нового пользователя
            if(message.Sender != null)
            {
                ArgumentException ex = new ArgumentException("message.Sender must be NULL");
                throw new FaultException<ArgumentException>(ex, ex.Message);
            }
            if(message.SenderUsername == null)
            {
                ArgumentException ex = new ArgumentException("message.SenderUsername cannot be NULL");
                throw new FaultException<ArgumentException>(ex, ex.Message);
            }
            if(message.Title == null)
            {
                ArgumentException ex = new ArgumentException("message.Title cannot be NULL");
                throw new FaultException<ArgumentException>(ex, ex.Message);
            }

            foreach (Recipient item in message.Recipients)
            {
                ///Можно нечаянно добавить нового пользователя
                if (item.RecipientEmployee != null)
                {
                    ArgumentException ex = new ArgumentException("message.Recipients[i].RecipientEmployee must be NULL");
                    throw new FaultException<ArgumentException>(ex, ex.Message);
                }
                ///Чтобы не было конфликта
                if (item.Message != null)
                {
                    ArgumentException ex = new ArgumentException("message.Recipients[i].Message must be NULL");
                    throw new FaultException<ArgumentException>(ex, ex.Message);
                }
                /// еще не известно какой Id
                if (item.MessageId != null)
                {
                    ArgumentException ex = new ArgumentException("message.Recipients[i].MessageId must be NULL");
                    throw new FaultException<ArgumentException>(ex, ex.Message);
                }
            }
           


            ///нельзя отсылать письма под чужим именем   
            if (string.Compare(currentUsername, message.SenderUsername) != 0)
            {
                ArgumentException ex = new ArgumentException("message.SenderUsername must be equal current username");
                throw new FaultException<ArgumentException>(ex, ex.Message);
            }
        }

        void PrepareMessage(Message message)
        {
            message.Date = DateTime.Now;
            message.LastUpdate = DateTime.Now;
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
        public MessagesPack GetMessages(FolderType folderType, MessageTypes messageTypes, Byte[] recentVersion)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            switch (folderType)
            {
                case FolderType.Inbox:
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
            bool deleted = messageTypes.HasFlag(MessageTypes.Deleted);
            bool viewed = messageTypes.HasFlag(MessageTypes.Viewed);
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
            bool deleted = messageTypes.HasFlag(MessageTypes.Deleted);
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
                case FolderType.Inbox:
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
            bool deleted = messageTypes.HasFlag(MessageTypes.Deleted);
            return MessageGateway.SelectIds(currentUsername, currentUsername, deleted); 
        }

        List<int> GetInboxMessagesIds(string currentUsername, MessageTypes messageTypes)
        {
            List<int> recievedMessagesIds = new List<int>();
            bool deleted = messageTypes.HasFlag(MessageTypes.Deleted);
            bool viewed = messageTypes.HasFlag(MessageTypes.Viewed);
            ///Ищем в получателях
            List<Recipient> recipients = RecipientGateway.Select(currentUsername, currentUsername, deleted, viewed);
            foreach (Recipient item in recipients)
            {
                recievedMessagesIds.Add((int)item.MessageId);
            }
            return recievedMessagesIds;
        }


        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SetMessageDeleted(int messageId, bool deleted)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            Message message = MessageGateway.Select(messageId, currentUsername);
            if (string.Compare(message.SenderUsername, currentUsername) != 0)
            {
                ArgumentException ex = new ArgumentException("message.SenderUsername must be equal current username");
                throw new FaultException<ArgumentException>(ex, ex.Message);
            }
            MessageGateway.Update(messageId, deleted, currentUsername);           
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SetRecipientDeleted(int messageId, bool deleted)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            RecipientGateway.UpdateDeleted(currentUsername, messageId, deleted, currentUsername);            
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void PermanentlyDeleteRecipient(int messageId)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            RecipientGateway.Delete(currentUsername, messageId, currentUsername);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void PermanentlyDeleteMessage(int messageId)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            Message removedMessage = MessageGateway.Select(messageId, currentUsername);
            if (string.Compare(removedMessage.SenderUsername, currentUsername) != 0)
            {
                ArgumentException ex = new ArgumentException("message.SenderUsername must be equal current username");
                throw new FaultException<ArgumentException>(ex, ex.Message);
            }
            MessageGateway.Delete(messageId, currentUsername);
        }       
    }
}