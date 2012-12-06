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
        public static List<Employee> SelectAll(string username)
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
                ExceptionHandler.HandleExcepion(ex, "public static List<Employee> SelectAll(string username)");
                return rows;
            }
        }

        /// <summary>
        /// Возвращает объект Employee с заполненым полем пароля
        /// </summary>
        /// <param name="authenticationUsername"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Employee SelectSecurity(string authenticationUsername, string username)
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
                ExceptionHandler.HandleExcepion(ex, "public static Employee SelectSecurity(string authenticationUsername, string username)"); 
                return null;
            }
        }

        /// <summary>
        /// Возвращает сотрудника по его идентификатору 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Employee SelectByUsername(string selectableUsername, string username)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("select_employee;2", GetConnection(username)))
                {
                    PrepareSE2(cmd, selectableUsername);
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        return CreateEmployee(reader, false);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "public static Employee SelectByUsername(string selectableUsername, string username)");
                return null;
            }
        }

        /// <summary> 
        /// Создает объек типа Employee по данным из таблицы 
        /// </summary>
        private static Employee CreateEmployee(SqlDataReader reader, bool security)
        {
            return !reader.HasRows ? null : new Employee((string)reader["Username"])
            {
                FirstName = (string)reader["FirstName"],
                SecondName = (string)reader["SecondName"],
                Role = (string)reader["Role"],
                Password = security ? (string)reader["Password"] : null
            };
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