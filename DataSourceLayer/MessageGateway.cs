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
    public class MessageGateway
    {

        /// <summary> 
        /// Производит вставку письма в таблицу 
        /// </summary>
        public static int? InsertMessage(Message message, SqlConnection sqlConnection)
        {
            try
            {   
                using (SqlCommand cmd = new SqlCommand("insert_message", sqlConnection))
                {
                    PrepareIM(cmd, message);                    
                    cmd.ExecuteNonQuery();
                    return int.Parse(cmd.Parameters["@id"].Value.ToString());
                }                
            }
            catch (Exception ex)
            {
                new ExceptionHandler().HandleExcepion(ex);
                return null;
            }
        }

        /// <summary>
        /// Подготавливает команду для выполнения ХП insert_message (IM)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="message"></param>
        private static void PrepareIM(SqlCommand cmd, Message message)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            CreateIMParameters(cmd);
            SetIMParameters(cmd, message);
        }

        /// <summary> 
        /// Задает параметры хранимой процедуры insert_mesage 
        /// </summary>
        private static void CreateIMParameters(SqlCommand cmd)
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
        /// Заполняет параметры хранимой процедуры insert_mesage 
        /// </summary>
        private static void SetIMParameters(SqlCommand cmd, Message message)
        {
            cmd.Parameters["@title"].Value = message.Title;
            cmd.Parameters["@date"].Value = message.Date.Date;
            cmd.Parameters["@senderId"].Value = message.Sender.EmployeeId;
            cmd.Parameters["@content"].Value = message.Content;
            cmd.Parameters["@deleteBySender"].Value = false;
        }
    }
}