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
            foreach (var a in message.Recipients)
                a.MessageId = (int) insertedMessageId;            
            foreach (var recipient in message.Recipients)
                RecipientGateway.InsertRecipient(recipient, sqlConnection);

        }
    }
}
