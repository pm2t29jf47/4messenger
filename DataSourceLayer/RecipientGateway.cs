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
        public static void Insert(Recipient recipient, string username)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("insert_recipient", GetConnection(username)))
                {
                    PrepareIR(cmd, recipient);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                new ExceptionHandler().HandleExcepion(ex, "public static void Insert(Recipient recipient, string username)");
            }
        }

        /// <summary>
        /// Подготавливает команду для выполнения ХП insert_recipient (IR)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="recipient"></param>
        private static void PrepareIR(SqlCommand cmd, Recipient recipient)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            CreateIRParameters(cmd);
            SetIRParameters(cmd, recipient);
        }

        /// <summary>
        /// Задает параметры хранимой процедуры insert_recipient 
        /// </summary>
        /// <param name="cmd"></param>
        private static void CreateIRParameters(SqlCommand cmd)
        {
            cmd.Parameters.Add(new SqlParameter("@recipientUsername", SqlDbType.NVarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@messageId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@deleted", SqlDbType.Bit));
            cmd.Parameters.Add(new SqlParameter("@viewed", SqlDbType.Bit));
        }

        /// <summary> 
        /// Заполняет параметры хранимой процедуры insert_recipient 
        /// </summary>
        private static void SetIRParameters(SqlCommand cmd, Recipient recipient)
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
        public static List<Recipient> SelectBy_RecipientUsername_Deleted(string username,bool deleted)
        {
            List<Recipient> rows = new List<Recipient>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("select_recipient;1", GetConnection(username)))
                {
                    PrepareSR1(cmd, username,deleted);
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            rows.Add(CreateRecipient(reader));
                    return rows;
                }
            }
            catch (Exception ex)
            {
                new ExceptionHandler().HandleExcepion(ex, "public static List<Recipient> SelectBy_RecipientUsername_Deleted(string username,bool deleted)");
                return rows;
            }
        }

        /// <summary>
        /// Подготавливает команду для выполнения ХП select_recipient;1 (SR1)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="username"></param>
        private static void PrepareSR1(SqlCommand cmd, string username, bool deleted)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@recipientUsername", SqlDbType.NVarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@deleted", SqlDbType.Bit));
            cmd.Parameters["@recipientUsername"].Value = username;
            cmd.Parameters["@deleted"].Value = deleted;
        }

        /// <summary>
        /// Возвращает все строки таблицы Recipient с данным идентификатором сообщения
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<Recipient> SelectByMessageId(int messageId, string username)
        {
            List<Recipient> rows = new List<Recipient>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("select_recipient;2", GetConnection(username)))
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
                new ExceptionHandler().HandleExcepion(ex, "public static List<Recipient> SelectByMessageId(int messageId, string username)");
                return rows;
            }
        }

        /// <summary>
        /// Подготавливает команду для выполнения ХП select_recipient;2 (SR2)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="messageId"></param>
        private static void PrepareSR2(SqlCommand cmd, int messageId)
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
        public static void UpdateViewed(string username, int messageId, bool viewed)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("update_recipient;1", GetConnection(username)))
                {
                    PrepareUR1(cmd, username, messageId, viewed);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                new ExceptionHandler().HandleExcepion(ex, "public static void UpdateViewed(string username, int messageId, bool viewed)");
            }
        }

        /// <summary>
        /// Подготавливает команду для выполнения ХП update_recipient;1 (UR1)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="username"></param>
        /// <param name="messageId"></param>
        /// <param name="viewed"></param>
        private static void PrepareUR1(SqlCommand cmd, string username, int messageId, bool viewed)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            CreateUR1Parameters(cmd);
            SetUR1Parameters(cmd, username, messageId, viewed);
        }

        /// <summary>
        /// Задает параметры хранимой процедуры update_recipient
        /// </summary>
        /// <param name="cmd"></param>
        private static void CreateUR1Parameters(SqlCommand cmd)
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
        private static void SetUR1Parameters(SqlCommand cmd, string username, int messageId, bool viewed)
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
        private static Recipient CreateRecipient(SqlDataReader reader)
        {
            return !reader.HasRows ? null : new Recipient(
                (string) reader["RecipientUsername"],
                int.Parse(reader["MessageId"].ToString()),
                bool.Parse(reader["Deleted"].ToString()),
                bool.Parse(reader["Viewed"].ToString()));
        }         
    }
}
