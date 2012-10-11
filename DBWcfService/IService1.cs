using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Entities;

namespace DBWcfService
{
    [ServiceContract]
    public interface IService1
    {
        /// <summary> 
        /// Возвращает коллекцию содержащую всех сотрудников 
        /// </summary>
        [OperationContract]
        List<Employee> GetEmployeeList();

        /// <summary> 
        /// Возвращает коллекцию содержащую все письма 
        /// </summary>
        [OperationContract]
        List<Message> GetMessageList();

        /// <summary> 
        /// Производит вставку письма в таблицу Message 
        /// </summary>
        [OperationContract]
        void InsertMessage(Message message);
    }   
}
