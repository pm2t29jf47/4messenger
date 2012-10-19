using System;
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
        public static void InsertRecipient(Recipient recipient, string username)
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
                new ExceptionHandler().HandleExcepion(ex);
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
            cmd.Parameters.Add(new SqlParameter("@delete", SqlDbType.Bit));
        }

        /// <summary> 
        /// Заполняет параметры хранимой процедуры insert_recipient 
        /// </summary>
        private static void SetIRParameters(SqlCommand cmd, Recipient recipient)
        {
            cmd.Parameters["@recipientUsername"].Value = recipient.RecipientUsername;
            cmd.Parameters["@messageId"].Value = recipient.MessageId;
            cmd.Parameters["@delete"].Value = recipient.Delete;
        }

        public static List<Recipient> SelectRecipient(string username)
        {
            List<Recipient> rows = new List<Recipient>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("select_recipient", GetConnection(username)))
                {
                    PrepareSR(cmd, username);
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            rows.Add(CreateRecipient(reader));
                    return rows;
                }
            }
            catch (Exception ex)
            {
                new ExceptionHandler().HandleExcepion(ex);
                return rows;
            }
        }

        private static Recipient CreateRecipient(SqlDataReader reader)
        {
            string a1, a2, a3;
            return new Recipient(
                (string) reader["RecipientUsername"],
                int.Parse(reader["MessageId"].ToString()),
                bool.Parse(reader["Delete"].ToString()));
        }

   

        private static void PrepareSR(SqlCommand cmd, string username)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@recipientUsername", SqlDbType.NVarChar, 50));
            cmd.Parameters["@recipientUsername"].Value = username;
        } 
    }
}
