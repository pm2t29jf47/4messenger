using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using DBWebService;
using Entities;
using ServerSideExceptionHandler;



namespace DBWebService
{

    /// <summary> 
    /// Класс для доступа к данным таблицы Employee 
    /// </summary>
    public class EmployeeGateway : Gateway
    {

        /// <summary> 
        /// Возвращает коллекцию всех сотрудников
        /// </summary>
        public List<Employee> SelectEmployees()
        {
            List<Employee> rows = new List<Employee>();
            try
            {            
                using (SqlCommand cmd = new SqlCommand("select_employees", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            rows.Add(CreateEmployee(reader));
                    return rows;
                }                
            }
            catch (Exception ex)
            {
                new ExceptionHandler().HandleExcepion(ex);
                return rows;
            }
        }

        /// <summary> 
        /// Создает объек типа Employee по данным из таблицы 
        /// </summary>
        private Employee CreateEmployee(SqlDataReader reader)
        {
            return new Employee(
                int.Parse(reader[0].ToString()),
                reader[1].ToString()
            );
        }
    }
}