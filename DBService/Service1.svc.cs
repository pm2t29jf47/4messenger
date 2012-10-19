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
        public Service1()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            sqlConnection.Open();
        }

        SqlConnection sqlConnection;

        /// <summary> 
        /// Возвращает коллекцию содержащую всех сотрудников 
        /// </summary>
        public List<Entities.Employee> GetEmployeeList()
        {
            //return EmployeeGateway.SelectEmployees(sqlConnection);
            return null;
        }

        /// <summary> 
        /// Производит вставку письма в таблицу Message 
        /// </summary>
        public void SendMessage(Entities.Message message)
        {   
            //int? insertedMessageId = MessageGateway.InsertMessage(message, sqlConnection);
            //if (insertedMessageId == null) return;
            //foreach (var recipient in message.Recipients)
            //{
            //    recipient.MessageId = (int)insertedMessageId;
            //    RecipientGateway.InsertRecipient(recipient, sqlConnection);
            //}
        }

        /// <summary>
        /// Получить письма
        /// </summary>
        /// <returns></returns>
        public List<Entities.Message> ReceiveMessages()
        {
            return null;
        }

        /// <summary>
        /// Получить true
        /// </summary>
        /// <returns></returns>
        public bool GetTrue()
        {      
            return true;
        }

        /// <summary>
        /// Доступ имеют все зарегистрированные пользователи
        /// </summary>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        /// <summary>
        /// получить объект сотруднка
        /// </summary>
        /// <returns></returns>
        public Entities.Employee GetNewEmployee()
        {
            var a = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            return null; // new Entities.Employee(1, "new new new employee");
        }
    }
}