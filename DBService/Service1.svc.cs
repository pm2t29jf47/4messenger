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




namespace DBService
{
    public class Service1 : IService1
    {
        /// <summary> 
        /// Возвращает коллекцию содержащую всех сотрудников 
        /// </summary>
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Entities.Employee> GetEmployeeList()
        {
            //return EmployeeGateway.SelectEmployees(sqlConnection);
            return null;
        }

        /// <summary> 
        /// Производит вставку письма в таблицу Message 
        /// </summary>
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public void SendMessage(Entities.Message message)
        {
            
            string username = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            ///нельзя отсылать письма под чужим именем
            ///вернуть ошибку!!!!
            if (string.Compare(username, message.SenderUsername) == 0)
                return;
            int? insertedMessageId = MessageGateway.InsertMessage(message, username);
            if (insertedMessageId == null) return;
            foreach (var recipient in message.Recipients)
            {
                recipient.MessageId = (int)insertedMessageId;
                RecipientGateway.InsertRecipient(recipient, username);
            }
        }

        /// <summary>
        /// Получить письма
        /// </summary>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public List<Entities.Message> ReceiveMessages()
        {
            string username = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            List<Entities.Message> messages = new List<Entities.Message>();
            List<Entities.Recipient> recipients = RecipientGateway.SelectRecipient(username);
            foreach (var recipient in recipients)
                ///recipient.MessageId не может быть null, тк берется из базы
                messages.Add(MessageGateway.SelectMessage((int)recipient.MessageId, username));
            return messages;
        }
    }
}