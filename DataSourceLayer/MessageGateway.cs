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
        public static int? InsertMessage(Message message, string username)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("insert_message", GetConnection(username)))
                {
                    PrepareIM(cmd, message);                    
                    cmd.ExecuteNonQuery();
                    return int.Parse(cmd.Parameters["@id"].Value.ToString());
                }                
            }
            catch (Exception ex)
            {
                new ExceptionHandler().HandleExcepion(ex,"InsertMessage");
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
                new SqlParameter("@title", SqlDbType.NVarChar, 100));
            cmd.Parameters.Add(
                new SqlParameter("@date", SqlDbType.DateTime));
            cmd.Parameters.Add(
                new SqlParameter("@content", SqlDbType.NVarChar, 1000));
            cmd.Parameters.Add(
                new SqlParameter("@senderUsername", SqlDbType.NVarChar, 50));            
            cmd.Parameters.Add(
                new SqlParameter("@delete", SqlDbType.Bit));
            cmd.Parameters.Add(
                new SqlParameter("@id", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                });
        }

        /// <summary> 
        /// Заполняет параметры хранимой процедуры insert_mesage 
        /// </summary>
        private static void SetIMParameters(SqlCommand cmd, Message message)
        {
            cmd.Parameters["@title"].Value = message.Title;
            cmd.Parameters["@date"].Value = message.Date.Date;
            cmd.Parameters["@senderUsername"].Value = message.SenderUsername;
            cmd.Parameters["@content"].Value = message.Content;
            cmd.Parameters["@delete"].Value = false;
        }

        /// <summary>
        /// Возвращает сообщение по его идентификатору 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Message SelectMessage(int id, string username)
        {          
            try
            {
                using (SqlCommand cmd = new SqlCommand("select_message;1", GetConnection(username)))
                {
                    PrepareSM1(cmd, id);
                    ///будет работать с тем же подключением в пуле, что итекущий метод!
                    List<Recipient> recipients = RecipientGateway.SelectRecipient(id, username);
                    ////////////////////////////////////////////////////////////////////////////
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        return CreateMessage(reader, recipients);
                    }
                                                           
                }
            }
            catch (Exception ex)
            {
                new ExceptionHandler().HandleExcepion(ex,"SelectMessage");
                return null;
            }
        }

        /// <summary>
        /// Создает объек типа Message по данным из таблицы 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="recipients"></param>
        /// <returns></returns>
        private static Message CreateMessage(SqlDataReader reader, List<Recipient> recipients)
        {
            return !reader.HasRows ? null : new Message(
                int.Parse(reader["Id"].ToString()),
                (string) reader["Title"],
                (DateTime.Parse(reader["Date"].ToString())),
                recipients,
                (string) reader["SenderUsername"],
                (string) reader["Content"]);            
        }

        /// <summary>
        /// Подготавливает команду для выполнения ХП select_message;1 (SM1)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="messageId"></param>
        private static void PrepareSM1(SqlCommand cmd, int messageId)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            cmd.Parameters["@id"].Value = messageId;
        }
    }
}