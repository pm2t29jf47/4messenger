using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using DBWebService;
using Entities;
using ServerSideExceptionHandler;

namespace DBWebService
{

    /// <summary> 
    /// Класс для доступа к данным таблицы Message 
    /// </summary>
    public class MessageGateway : Gateway
    {
        /// <summary> 
        /// Возвращает коллекцию всех писем
        /// </summary>
        public List<Message> SelectMessages()
        {
            List<Message> rows = new List<Message>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("select_messages", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            rows.Add(CreateMessage(reader));
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
        /// Производит вставку письма в таблицу 
        /// </summary>
        public void InsertMessage(Message message)
        {
            try
            {   
                using (SqlCommand cmd = new SqlCommand("insert_message", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    CreateSPParameters(cmd);
                    SetSPParameters(cmd, message);
                    cmd.ExecuteNonQuery();
                }                
            }
            catch (Exception ex)
            {
                new ExceptionHandler().HandleExcepion(ex);          
            }
        }

        /// <summary> 
        /// Определяет параметры хранимой процедуры для объекта типа SqlCommand 
        /// </summary>
        private void CreateSPParameters(SqlCommand cmd)
        {
            cmd.Parameters.Add(new SqlParameter("@title", SqlDbType.VarChar, 100));
            cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.Date));
            cmd.Parameters.Add(new SqlParameter("@recipientId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@senderId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@content", SqlDbType.VarChar, 1000));
        }

        /// <summary> 
        /// Задает параметры хранимой процедуры для объекта типа SqlCommand 
        /// </summary>
        private void SetSPParameters(SqlCommand cmd, Message message)
        {
            cmd.Parameters["@title"].Value = message.Title;
            cmd.Parameters["@date"].Value = message.Date.Date;
            cmd.Parameters["@recipientId"].Value = message.RecipientId;
            cmd.Parameters["@senderId"].Value = message.SenderId;
            cmd.Parameters["@content"].Value = message.Content;
        }

        /// <summary> 
        /// Создает объек типа Message по данным из таблицы 
        /// </summary>
        private Message CreateMessage(SqlDataReader reader)
        {
            return new Message(
                int.Parse(reader["MessageId"].ToString()),
                reader["Title"].ToString(),
                DateTime.Parse(reader["Date"].ToString()),
                reader["Recipient"].ToString(),
                reader["Sender"].ToString(),
                reader["Content"].ToString());                
        }
    }
}