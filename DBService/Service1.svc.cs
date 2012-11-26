﻿using System;
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
        public void SendMessage(Message message)
        {
            
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            ///нельзя отсылать письма под чужим именем   
            if (string.Compare(currentUsername, message.SenderUsername) != 0)
                return;

            message.Date = DateTime.Now;
            int? insertedMessageId = MessageGateway.Insert(message, currentUsername);
            if (insertedMessageId == null) return;
            foreach (var item in message.EDRecipient_MessageId)
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
            foreach (Recipient item in recipients)
                messages.Add(MessageGateway.SelectById((int)item.MessageId, currentUsername));

            foreach (Message item in messages)
            {
                FillMessage(item, currentUsername);
            }
            return messages;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Message> GetSentboxMessages()
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            List<Message> sentMessages = MessageGateway.SelectBy_SenderUsername_Deleted(currentUsername,false);
            foreach (Message item in sentMessages)
            {
                FillMessage(item, currentUsername);
            }
            return sentMessages;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Message> GetDeletedMessages()
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            var deletedMessages = new List<Message>();
            List<Recipient> currentUserInRecipients = RecipientGateway.SelectBy_RecipientUsername_Deleted(currentUsername, true);
            foreach (Recipient item in currentUserInRecipients)
            {
                var receivedMessage = MessageGateway.SelectById((int)item.MessageId, currentUsername);            
                deletedMessages.Add(receivedMessage);
            }           
            List<Message> currentUserInSenders = MessageGateway.SelectBy_SenderUsername_Deleted(currentUsername,true);            
            deletedMessages.AddRange(currentUserInSenders);
            foreach (Message item in deletedMessages)
            {
                FillMessage(item, currentUsername);
            }
            return deletedMessages;      
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void CheckUser() { }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SetInboxMessageViewed(int messageId)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            RecipientGateway.UpdateViewed(currentUsername, messageId, true);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public Employee GetEmployee(string selectableUsername)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            return EmployeeGateway.SelectByUsername(selectableUsername, currentUsername);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SetInboxMessageDeleted(int MessageId)
        {
            throw new NotImplementedException();
        }

        void FillMessage(Message message, string currentUsername)
        {
            if (message == null
                || message.Id == null
                || message.SenderUsername == null)
                return;
            message.FKEmployee_SenderUsername = EmployeeGateway.SelectByUsername(message.SenderUsername, currentUsername);
            message.EDRecipient_MessageId = RecipientGateway.SelectByMessageId((int)message.Id, currentUsername);
            

        }

        /* Сообщения уже содержат в себе получателей
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Recipient> GetRecipients(int messageId)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            return RecipientGateway.SelectByMessageId(messageId, currentUsername);
        }
        */
    }
}