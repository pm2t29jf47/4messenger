using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DataSourceLayer;


namespace DBWcfService
{
    public class Service1 : IService1
    {
        public List<Entities.Employee> GetEmployeeList()
        {
            return new EmployeeGateway().SelectEmployees();
        }

        public void SendMessage(Entities.Message message)
        {
            new MessageGateway().InsertMessage(message);
        }
    }
}
