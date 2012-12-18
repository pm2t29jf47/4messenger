using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Entities;
using ServerSideExceptionHandler;

namespace EFDataSourceLayer
{

    /// <summary> 
    /// Класс для доступа к данным таблицы Message 
    /// </summary>
    public class MessageGateway : Gateway
    {
        /*
        /// <summary> 
        /// Производит вставку письма в таблицу 
        /// </summary>
        public static int Insert(Message message, string connectionUsername)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("insert_message", GetConnection(connectionUsername)))
                {
                    PrepareIM(cmd, message);                    
                    cmd.ExecuteNonQuery();
                    return int.Parse(cmd.Parameters["@id"].Value.ToString());
                }                
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(int)DataSourceLayer.MessageGateway.Insert(Message message, string username)");
                throw;
            }
        }

        /// <summary>
        /// Подготавливает команду для выполнения ХП insert_message (IM)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="message"></param>
        static void PrepareIM(SqlCommand cmd, Message message)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            CreateIMParameters(cmd);
            SetIMParameters(cmd, message);
        }

        /// <summary> 
        /// Задает параметры хранимой процедуры insert_mesage 
        /// </summary>
        static void CreateIMParameters(SqlCommand cmd)
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
                new SqlParameter("@deleted", SqlDbType.Bit));
            cmd.Parameters.Add(
                new SqlParameter("@id", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                });
            cmd.Parameters.Add(
                new SqlParameter("@lastUpdate", SqlDbType.DateTime));
        }

        /// <summary> 
        /// Заполняет параметры хранимой процедуры insert_mesage 
        /// </summary>
        static void SetIMParameters(SqlCommand cmd, Message message)
        {
            cmd.Parameters["@title"].Value = message.Title;
            cmd.Parameters["@date"].Value = message.Date;
            cmd.Parameters["@senderUsername"].Value = message.SenderUsername;
            cmd.Parameters["@content"].Value = message.Content;
            cmd.Parameters["@deleted"].Value = false;
            cmd.Parameters["@lastUpdate"].Value = DateTime.Now;
        }

        /// <summary>
        /// Возвращает сообщение по его идентификатору 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connectionUsername"></param>
        /// <returns></returns>
        public static Message Select(int id, string connectionUsername)
        {          
            try
            {
                using (SqlCommand cmd = new SqlCommand("select_message;1", GetConnection(connectionUsername)))
                {
                    PrepareSM1(cmd, id);   
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        return CreateMessage(reader);
                    }                                                                                                 
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(Message)DataSourceLayer.MessageGateway.Select(int id, string connectionUsername)");
                throw;
            }
        }

        /// <summary>
        /// Возвращает отправленные сообщения
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<Message> Select(string senderUsername ,string connectionUsername, bool deleted)
        {
            List<Message> rows = new List<Message>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("select_message;2", GetConnection(connectionUsername)))
                {
                    PrepareSM2(cmd, senderUsername, deleted);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rows.Add(CreateMessage(reader));
                        }
                    }                    
                    return rows;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(List<Message>)DataSourceLayer.MessageGateway.Select(string senderUsername ,string connectionUsername, bool deleted)");
                throw;
            }
        }

        /// <summary>
        /// Возвращает коллекцию Id отправленных сообщений
        /// </summary>
        /// <param name="senderUsername"></param>
        /// <param name="connectionUsername"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public static List<int> SelectIds(string senderUsername, string connectionUsername, bool deleted)
        {
            List<int> rows = new List<int>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("select_message;3", GetConnection(connectionUsername)))
                {
                    PrepareSM2(cmd, senderUsername, deleted);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rows.Add(int.Parse(reader["Id"].ToString()));
                        }
                    }
                    return rows;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(List<int>)DataSourceLayer.MessageGateway.SelectIds(string senderUsername, string connectionUsername, bool deleted)");
                throw;
            }
        }

        public static void Update(int id, string connectionUsername)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("update_message;1", GetConnection(connectionUsername)))
                {
                    PrepareUM1(cmd, id, DateTime.Now);
                    cmd.ExecuteNonQuery();                   
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "()DataSourceLayer.MessageGateway.SelectIds(string senderUsername, string connectionUsername, bool deleted)");
                throw;
            }
        }

        /// <summary>
        /// Подготавливает команду для выполнения ХП select_message;2 (SM2)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="username"></param>
        static void PrepareSM2(SqlCommand cmd, string username, bool deleted)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@senderUsername", SqlDbType.NVarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@deleted", SqlDbType.Bit));
            cmd.Parameters["@senderUsername"].Value = username;
            cmd.Parameters["@deleted"].Value = deleted;
        }

        static void PrepareUM1(SqlCommand cmd, int id, DateTime lastUpdate)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 50));
            cmd.Parameters.Add(new SqlParameter("@lastUpdate", SqlDbType.DateTime));
            cmd.Parameters["@id"].Value = id;
            cmd.Parameters["@lastUpdate"].Value = lastUpdate;
        }

        /// <summary>
        /// Создает объек типа Message по данным из таблицы 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="recipients"></param>
        /// <returns></returns>
        static Message CreateMessage(SqlDataReader reader)
        {
            if(!reader.HasRows)
                return null;
            
            return new Message(int.Parse(reader["Id"].ToString()))
            {
                Title = (string) reader["Title"],
                Date = (DateTime.Parse(reader["Date"].ToString())),
                SenderUsername = (string) reader["SenderUsername"],
                Content = (string) reader["Content"],
                Deleted = bool.Parse(reader["Deleted"].ToString()),
                Version = (byte[])reader["Version"]
            };
        }

        /// <summary>
        /// Подготавливает команду для выполнения ХП select_message;1 (SM1)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="messageId"></param>
        static void PrepareSM1(SqlCommand cmd, int messageId)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            cmd.Parameters["@id"].Value = messageId;
        }

        public static void Update(int id, bool deleted, string connectionUsername)
        {            
            try
            {
                using (SqlCommand cmd = new SqlCommand("update_message;2", GetConnection(connectionUsername)))
                {
                    PrepareUM2(cmd, id, deleted);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "()DataSourceLayer.MessageGateway.Update(int id, bool deleted, string connectionUsername)");
                throw;
            }            
        }

        static void PrepareUM2(SqlCommand cmd, int messageId, bool deleted)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@deleted", SqlDbType.Bit));
            cmd.Parameters["@id"].Value = messageId;
            cmd.Parameters["@deleted"].Value = deleted;
        }

        public static void Delete(int id, string connectionUsername)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("delete_message;1", GetConnection(connectionUsername)))
                {
                    PrepareDM1(cmd, id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "()DataSourceLayer.MessageGateway.Delete(int id, string connectionUsername)");
                throw;
            }
        }

        static void PrepareDM1(SqlCommand cmd, int messageId)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            cmd.Parameters["@id"].Value = messageId;
        }

        */
    }
}