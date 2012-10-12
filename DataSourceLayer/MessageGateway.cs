using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Entities;
using ServerSideExceptionHandler;

namespace DataSourceLayer
{

    /// <summary> 
    /// Класс для доступа к данным таблицы Message 
    /// </summary>
    public class MessageGateway : Gateway
    {
        /// <summary> 
        /// Производит вставку письма в таблицу 
        /// </summary>
        public int InsertMessage(Message message)
        {
            try
            {   
                using (SqlCommand cmd = new SqlCommand("insert_message", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    CreateSPParameters(cmd);
                    SetSPParameters(cmd, message);
                    cmd.ExecuteNonQuery();
                    return int.Parse(cmd.Parameters["@id"].Value.ToString());//нужен для вставки в другие таблицы
                }                
            }
            catch (Exception ex)
            {
                new ExceptionHandler().HandleExcepion(ex);
                return -1;
            }
        }

        /// <summary> 
        /// Определяет параметры хранимой процедуры для объекта типа SqlCommand 
        /// </summary>
        private void CreateSPParameters(SqlCommand cmd)
        {
            cmd.Parameters.Add(
                new SqlParameter("@title", SqlDbType.VarChar, 100));
            cmd.Parameters.Add(
                new SqlParameter("@date", SqlDbType.Date));
            cmd.Parameters.Add(
                new SqlParameter("@senderId", SqlDbType.Int));
            cmd.Parameters.Add(
                new SqlParameter("@content", SqlDbType.VarChar, 1000));
            cmd.Parameters.Add(
                new SqlParameter("@id", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                });
            cmd.Parameters.Add(
                new SqlParameter("@deleteBySender", SqlDbType.Bit));
        }

        /// <summary> 
        /// Задает параметры хранимой процедуры для объекта типа SqlCommand 
        /// </summary>
        private void SetSPParameters(SqlCommand cmd, Message message)
        {
            cmd.Parameters["@title"].Value = message.Title;
            cmd.Parameters["@date"].Value = message.Date.Date;
            cmd.Parameters["@senderId"].Value = message.Sender.EmployeeId;
            cmd.Parameters["@content"].Value = message.Content;
            cmd.Parameters["@deleteBySender"].Value = false;
        }

        /// <summary> 
        /// Создает объек типа Message по данным из таблицы 
        /// </summary>
    }
}