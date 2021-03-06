﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Data.SqlClient;
using System.Data;
using ServerSideExceptionHandler;

namespace DataSourceLayer
{
    /// <summary>
    /// Класс для доступа к данным таблицы Recipient
    /// </summary>
    public class RecipientGateway : Gateway
    {

        /// <summary>
        /// Добавляет нового адресата к письму
        /// </summary>
        /// <param name="recipient"></param>
        public static void Insert(Recipient recipient, string connectionUsername)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("insert_recipient", GetConnection(connectionUsername)))
                {
                    PrepareIR(cmd, recipient);
                    cmd.ExecuteNonQuery();
                    MessageGateway.Update((int)recipient.MessageId, connectionUsername);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "()DataSourceLayer.RecipientGateway.Insert(Recipient recipient, string username)");
                throw;
            }
        }

        /// <summary>
        /// Подготавливает команду для выполнения ХП insert_recipient (IR)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="recipient"></param>
        static void PrepareIR(SqlCommand cmd, Recipient recipient)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            CreateIRParameters(cmd);
            SetIRParameters(cmd, recipient);
        }

        /// <summary>
        /// Задает параметры хранимой процедуры insert_recipient 
        /// </summary>
        /// <param name="cmd"></param>
        static void CreateIRParameters(SqlCommand cmd)
        {
            cmd.Parameters.Add(new SqlParameter("@recipientUsername", SqlDbType.NVarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@messageId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@deleted", SqlDbType.Bit));
            cmd.Parameters.Add(new SqlParameter("@viewed", SqlDbType.Bit));
        }

        /// <summary> 
        /// Заполняет параметры хранимой процедуры insert_recipient 
        /// </summary>
        static void SetIRParameters(SqlCommand cmd, Recipient recipient)
        {
            cmd.Parameters["@recipientUsername"].Value = recipient.RecipientUsername;
            cmd.Parameters["@messageId"].Value = recipient.MessageId;
            cmd.Parameters["@deleted"].Value = recipient.Deleted;
            cmd.Parameters["@viewed"].Value = recipient.Viewed;
        }


        /// <summary>
        /// Возвращает все строки таблицы Recipient с данным адресатом и флагом удаления
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<Recipient> Select(string recipientUsername, string connectionUsername, bool deleted, bool viewed)
        {
            List<Recipient> rows = new List<Recipient>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("select_recipient;1", GetConnection(connectionUsername)))
                {
                    PrepareSR1(cmd, recipientUsername, deleted, viewed);
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            rows.Add(CreateRecipient(reader));
                    return rows;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(List<Recipient>)DataSourceLayer.RecipientGateway.Select(string recipientUsername, string connectionUsername, bool deleted, bool viewed)");
                throw;
            }
        }

        /// <summary>
        /// Подготавливает команду для выполнения ХП select_recipient;1 (SR1)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="username"></param>
        static void PrepareSR1(SqlCommand cmd, string username, bool deleted, bool viewed)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@recipientUsername", SqlDbType.NVarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@deleted", SqlDbType.Bit));
            cmd.Parameters.Add(new SqlParameter("@viewed", SqlDbType.Bit));
            cmd.Parameters["@recipientUsername"].Value = username;
            cmd.Parameters["@deleted"].Value = deleted;
            cmd.Parameters["@viewed"].Value = viewed;
        }


        /// <summary>
        /// Возвращает все строки таблицы Recipient с данным идентификатором сообщения
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<Recipient> Select(int messageId, string connectionUsername)
        {
            List<Recipient> rows = new List<Recipient>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("select_recipient;2", GetConnection(connectionUsername)))
                {
                    PrepareSR2(cmd, messageId);
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            rows.Add(CreateRecipient(reader));
                    return rows;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(List<Recipient>)DataSourceLayer.RecipientGateway.Select(int messageId, string connectionUsername)");        
                throw;
            }
        }

        /// <summary>
        /// Подготавливает команду для выполнения ХП select_recipient;2 (SR2)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="messageId"></param>
        static void PrepareSR2(SqlCommand cmd, int messageId)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@messageId", SqlDbType.Int));
            cmd.Parameters["@messageId"].Value = messageId;
        }


        /// <summary>
        /// Обновляет флаг прочитанности у строки таблицы Recipient
        /// </summary>
        /// <param name="username"></param>
        /// <param name="messageId"></param>
        /// <param name="viewed"></param>
        public static void Update(string recipientUsername, int messageId, bool viewed, string connectionUsername)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("update_recipient;1", GetConnection(connectionUsername)))
                {
                    PrepareUR1(cmd, recipientUsername, messageId, viewed);
                    cmd.ExecuteNonQuery();
                    MessageGateway.Update(messageId, connectionUsername);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "()DataSourceLayer.RecipientGateway.Update(string recipientUsername, int messageId, bool viewed, string connectionUsername)");
                throw;
            }
        }

        /// <summary>
        /// Подготавливает команду для выполнения ХП update_recipient;1 (UR1)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="username"></param>
        /// <param name="messageId"></param>
        /// <param name="viewed"></param>
        static void PrepareUR1(SqlCommand cmd, string username, int messageId, bool viewed)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            CreateUR1Parameters(cmd);
            SetUR1Parameters(cmd, username, messageId, viewed);
        }

        /// <summary>
        /// Задает параметры хранимой процедуры update_recipient
        /// </summary>
        /// <param name="cmd"></param>
        static void CreateUR1Parameters(SqlCommand cmd)
        {
            cmd.Parameters.Add(new SqlParameter("@recipientUsername", SqlDbType.NVarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@messageId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@viewed", SqlDbType.Bit));
        }

        /// <summary>
        /// Заполняет параметры хранимой процедуры update_recipient;1
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="username"></param>
        /// <param name="messageId"></param>
        /// <param name="viewed"></param>
        static void SetUR1Parameters(SqlCommand cmd, string username, int messageId, bool viewed)
        {
            cmd.Parameters["@recipientUsername"].Value = username;
            cmd.Parameters["@messageId"].Value = messageId;
            cmd.Parameters["@viewed"].Value = viewed;
        }

        /// <summary>
        /// Создает объект типа Recipient
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        static Recipient CreateRecipient(SqlDataReader reader)
        {
            return !reader.HasRows ? null : new Recipient(
                (string)reader["RecipientUsername"],
                int.Parse(reader["MessageId"].ToString()))
                {
                    Deleted = bool.Parse(reader["Deleted"].ToString()),
                    Viewed = bool.Parse(reader["Viewed"].ToString()),
                    Version = (byte[])reader["Version"]
                };
        }

        public static void UpdateDeleted(string recipientUsername, int messageId, bool deleted, string connectionUsername)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("update_recipient;2", GetConnection(connectionUsername)))
                {
                    PrepareUR2(cmd, recipientUsername, messageId, deleted);
                    cmd.ExecuteNonQuery();
                    MessageGateway.Update(messageId, connectionUsername);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "()DataSourceLayer.RecipientGateway.UpdateDeleted(string recipientUsername, int messageId, bool deleted, string connectionUsername)");
                throw;
            }
        }

        static void PrepareUR2(SqlCommand cmd, string username, int messageId, bool deleted)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            CreateUR2Parameters(cmd);
            SetUR2Parameters(cmd, username, messageId, deleted);
        }

        /// <summary>
        /// Задает параметры хранимой процедуры update_recipient
        /// </summary>
        /// <param name="cmd"></param>
        static void CreateUR2Parameters(SqlCommand cmd)
        {
            cmd.Parameters.Add(new SqlParameter("@recipientUsername", SqlDbType.NVarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@messageId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@deleted", SqlDbType.Bit));
        }

        /// <summary>
        /// Заполняет параметры хранимой процедуры update_recipient;1
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="username"></param>
        /// <param name="messageId"></param>
        /// <param name="viewed"></param>
        static void SetUR2Parameters(SqlCommand cmd, string username, int messageId, bool deleted)
        {
            cmd.Parameters["@recipientUsername"].Value = username;
            cmd.Parameters["@messageId"].Value = messageId;
            cmd.Parameters["@deleted"].Value = deleted;
        }

        public static void Delete(string recipientUsername, int messageId, string connectionUsername)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("delete_recipient;1", GetConnection(connectionUsername)))
                {
                    PrepareDR1(cmd, recipientUsername, messageId);
                    cmd.ExecuteNonQuery();
                    MessageGateway.Update(messageId, connectionUsername);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "()DataSourceLayer.RecipientGateway.Delete(string recipientUsername, int messageId, string connectionUsername)");
                throw;
            }
        }

        static void PrepareDR1(SqlCommand cmd, string recipientUsername, int messageId)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@messageId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@recipientUsername", SqlDbType.NVarChar, 50));
            cmd.Parameters["@messageId"].Value = messageId;
            cmd.Parameters["@recipientUsername"].Value = recipientUsername;
        }
    }
}
