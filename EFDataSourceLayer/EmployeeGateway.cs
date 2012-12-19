using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Entities;
using ServerSideExceptionHandler;

namespace EFDataSourceLayer
{

    /// <summary> 
    /// Класс для доступа к данным таблицы Employee 
    /// </summary>
    public class EmployeeGateway : Gateway
    {
        
        /// <summary> 
        /// Возвращает коллекцию всех сотрудников
        /// </summary>
        public static List<Employee> SelectAll(string connectionUsername)
        {
            List<Employee> rows = new List<Employee>();
            try
            {
                var dc = Gateway.GetContext(connectionUsername);
                List<Employee> entities = dc.Employees.ToList();
                List<Employee> result = new List<Employee>();
                foreach (Employee item in entities)
                {
                    result.Add(TranslateToDepletedEmployeeObject(item));
                }
                return result;                              
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(List<Employee>)EFDataSourceLayer.EmployeeGateway.SelectAll(string username)");
                throw;           
            }
        }

        static Employee TranslateToDepletedEmployeeObject(Employee efEmployee)
        {
            return new Employee(efEmployee.Username)
            {
                FirstName = efEmployee.FirstName,
                Role = efEmployee.Role,
                SecondName = efEmployee.SecondName,
                Version = efEmployee.Version
            };
        }

        /// <summary>
        /// Возвращает объект Employee с заполненым полем пароля
        /// </summary>
        /// <param name="authenticationUsername"></param>
        /// <param name="connectionUsername"></param>
        /// <returns></returns>
        public static Employee SelectSecurity(string authenticationUsername, string connectionUsername)
        {
            try
            {
                var dc = Gateway.GetContext(connectionUsername);
                return dc.Employees.FirstOrDefault(row => string.Compare(row.Username, authenticationUsername) == 0);                  
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(Employee)EFDataSourceLayer.EmployeeGateway.SelectSecurity(string authenticationUsername, string username)");
                throw;
            }
        }

        /// <summary>
        /// Возвращает сотрудника по его идентификатору 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Employee Select(string selectableUsername, string connectionUsername)
        {
            try
            {
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                Employee result = dc.Employees.FirstOrDefault(row => string.Compare(row.Username, selectableUsername) == 0);
                return TranslateToDepletedEmployeeObject(result);
              
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(Employee)EFDataSourceLayer.EmployeeGateway.Select(string selectableUsername, string connectionUsername)");
                throw;
            }
        }

        public static List<string> SelectIds(string connectionUsername)
        {
            List<string> result = new List<string>();
            try
            {
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                return dc.Employees.Select(row => row.Username).ToList();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(Employee)EFDataSourceLayer.EmployeeGateway.SelectIds(string connectionUsername)");
                throw;
            }
        }




        
    }
}