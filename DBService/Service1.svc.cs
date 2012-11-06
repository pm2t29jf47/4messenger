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
        /// <summary> 
        /// Возвращает коллекцию содержащую всех сотрудников 
        /// </summary>
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Employee> GetEmployeeList()
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            return EmployeeGateway.SelectAll(currentUsername);
        }

        /// <summary> 
        /// Производит вставку письма в таблицу Message 
        /// </summary>
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
            foreach (var recipient in message.Recipients)
            {
                recipient.MessageId = (int)insertedMessageId;
                RecipientGateway.Insert(recipient, currentUsername);
            }
        }

        /// <summary>
        /// Получить письма
        /// </summary>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Message> GetInboxFolder()
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            List<Message> messages = new List<Message>();
            List<Recipient> recipients = RecipientGateway.SelectBy_RecipientUsername_Deleted(currentUsername,false);
            foreach (var recipient in recipients)
                ///recipient.MessageId не может быть null, тк берется из базы
                messages.Add(MessageGateway.SelectById((int)recipient.MessageId, currentUsername));
            return messages;
        }

        /// <summary>
        /// Проверяет аутентификационные данные пользователя
        /// </summary>
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void CheckUser(){ }

        /// <summary>
        /// Возвращает коллекцию отправленных писем
        /// </summary>
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Message> GetSentFolder()
        {
            ///добавить проверку на удаленность
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            return MessageGateway.SelectBy_SenderUsername_Deleted(currentUsername,false);
        }

        /// <summary>
        /// Задает сообщению флаг прочитанности
        /// </summary>
        /// <param name="MessageId"></param>
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SetMessageViewed(int messageId)
        {
            string currentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            RecipientGateway.UpdateViewed(currentUsername, messageId, true); 
        }
        
        /// <summary>
        /// Возвращает письма помеченные удаленными
        /// </summary>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Message> GetDeletedFolder()
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

        /// <summary>
        /// Возвращает сотрудника по его иднтификатору
        /// </summary>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public Employee GetEmployee(string selectableUsername)
        {
            string curentUsername = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            return EmployeeGateway.SelectByUsername(selectableUsername, curentUsername);
        }
        
        public void SetMessageDeleted(int MessageId)
        {
            throw new NotImplementedException();
        }
    }
}