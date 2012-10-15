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




namespace DBWcfService
{
    public class Service1 : IService1
    {
        public Service1()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            sqlConnection.Open();

        }

        SqlConnection sqlConnection;

        public List<Entities.Employee> GetEmployeeList()
        {
            return EmployeeGateway.SelectEmployees(sqlConnection);
        }

        public void SendMessage(Entities.Message message)
        {         
            int? insertedMessageId = MessageGateway.InsertMessage(message, sqlConnection);
            if (insertedMessageId == null) return;
            foreach (var recipient in message.Recipients)
            {
                recipient.MessageId = (int)insertedMessageId;
                RecipientGateway.InsertRecipient(recipient, sqlConnection);
            }
        }

        public List<Entities.Message> ReceiveMessages()//должен возвращать входящие письма для текущего пользователя
        {
            return null;
        }


        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public string[] GetRoles(string username)
        {
            if (ServiceSecurityContext.Current.PrimaryIdentity.Name == username)
            {
                return null; //CustomPrincpal.Current.Roles;
            }
            else
            {
                return Roles.GetRolesForUser(username);
            }

            //// inline authorization

            //// only administrators can retrieve the role information for other users
            //if (ServiceSecurityContext.Current.PrimaryIdentity.Name != username)
            //{
            //    if (Thread.CurrentPrincipal.IsInRole("administrators"))
            //    {
            //        // return roles for given user
            //        return Roles.GetRolesForUser(username);
            //    }
            //    else
            //    {
            //        // access denied
            //        throw new SecurityException();
            //    }
            //}

            //// return roles for current user
            //return CustomPrincipal.Current.Roles;
        }
    }
}
     
