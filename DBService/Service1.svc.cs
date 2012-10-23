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
            //return EmployeeGateway.SelectEmployees(sqlConnection);
            return null;
        }

        /// <summary> 
        /// Производит вставку письма в таблицу Message 
        /// </summary>
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SendMessage(Message message)
        {
            
            string username = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            ///нельзя отсылать письма под чужим именем
            ///вернуть ошибку!!!!
            if (string.Compare(username, message.SenderUsername) == 0)
                return;
            int? insertedMessageId = MessageGateway.Insert(message, username);
            if (insertedMessageId == null) return;
            foreach (var recipient in message.Recipients)
            {
                recipient.MessageId = (int)insertedMessageId;
                RecipientGateway.Insert(recipient, username);
            }
        }

        /// <summary>
        /// Получить письма
        /// </summary>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Message> ReceiveMessages()
        {
            string username = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            List<Entities.Message> messages = new List<Entities.Message>();
            List<Entities.Recipient> recipients = RecipientGateway.SelectByRecipientUsername(username);
            foreach (var recipient in recipients)
                ///recipient.MessageId не может быть null, тк берется из базы
                messages.Add(MessageGateway.SelectById((int)recipient.MessageId, username));
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
        public List<Message> SentMessages()
        {
            string username = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            return MessageGateway.SelectBySenderUsername(username);
        }

        /// <summary>
        /// Задает сообщению флаг прочитанности
        /// </summary>
        /// <param name="MessageId"></param>
        public void SetMessageViewed(int messageId)
        {
            string username = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            RecipientGateway.UpdateViewed(username, messageId, true); 
        }




        public List<Message> ReceiveDeletedMessages()
        {
            throw new NotImplementedException();
        }
    }
}