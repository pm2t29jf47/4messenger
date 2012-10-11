using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using Entities;
using DBWebService;



namespace DBWebService
{
    /// <summary>
    /// Summary description for DBWebService
    /// </summary>
    [WebService(Namespace = "localhost")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DBWebService : System.Web.Services.WebService
    {
        /// <summary> 
        /// Возвращает коллекцию содержащую всех сотрудников 
        /// </summary>
        [WebMethod]
        public List<Employee> GetEmployeeList()
        {
            return new EmployeeGateway().SelectEmployees();
        }

        /// <summary> 
        /// Производит вставку письма в таблицу Message 
        /// </summary>
        [WebMethod]
        public void InsertMessage(Message message)
        {
            new MessageGateway().InsertMessage(message);
        }
    }
}
