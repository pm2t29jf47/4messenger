using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Entities;
using ServerSideExceptionHandler;

namespace DataSourceLayer
{

    /// <summary> 
    /// Класс для доступа к данным таблицы Employee 
    /// </summary>
    public class EmployeeGateway : Gateway
    {

        /// <summary> 
        /// Возвращает коллекцию всех сотрудников
        /// </summary>
        public static List<Employee> SelectAllEmployees(string username)
        {
            List<Employee> rows = new List<Employee>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("select_employee;1", GetConnection(username)))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            rows.Add(CreateEmployee(reader,false));
                    return rows;
                }                
            }
            catch (Exception ex)
            {
                new ExceptionHandler().HandleExcepion(ex,"SelectAllEmployees");
                return rows;
            }
        }

        /// <summary>
        /// Возвращает объект Employee с заполненым полем пароля
        /// </summary>
        /// <param name="authenticationUsername"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Employee SelectSecurityEmployee(string authenticationUsername, string username)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("select_employee;2", GetConnection(username)))
                {
                    PrepareSE2(cmd, authenticationUsername);
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        return CreateEmployee(reader,true);
                    }                    
                }                
            }
            catch (Exception ex)
            {
                new ExceptionHandler().HandleExcepion(ex,"SelectSecurityEmployee"); 
                return null;
            }
        }

        /// <summary> 
        /// Создает объек типа Employee по данным из таблицы 
        /// </summary>
        private static Employee CreateEmployee(SqlDataReader reader, bool security)
        {
            return !reader.HasRows ? null : new Employee(
                (string)reader["Username"],
                (string)reader["FirstName"],
                (string)reader["SecondName"],
                (string)reader["Role"],
                security ? (string)reader["Password"] : null);
        }

        /// <summary>
        /// Подготавливает команду для выполнения ХП select_emloyee;2 (SE2)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="username"></param>
        private static void PrepareSE2(SqlCommand cmd, string username)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 50));
            cmd.Parameters["@username"].Value = username;
        }

    }
}