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
        public static void InsertRecipient(Recipient recipient, SqlConnection sqlConnection)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("insert_recipient", sqlConnection))
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
            cmd.Parameters.Add(new SqlParameter("@employeeId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@messageId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@deleteByRecipient", SqlDbType.Bit));
        }

        /// <summary> 
        /// Заполняет параметры хранимой процедуры insert_recipient 
        /// </summary>
        private static void SetIRParameters(SqlCommand cmd, Recipient recipient)
        {
            cmd.Parameters["@employeeId"].Value = recipient.EmployeeId;
            cmd.Parameters["@messageId"].Value = recipient.MessageId;
            cmd.Parameters["@deleteByRecipient"].Value = recipient.DeleteByRecipient;
        }        
    }
}