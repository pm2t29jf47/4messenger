using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Data.SqlClient;
using System.Data;
using ServerSideExceptionHandler;

namespace EFDataSourceLayer
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
                Recipient depletedRecipient = TranslateToDepletedRecipientObject(recipient);                
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                dc.Recipients.Add(depletedRecipient);
                dc.SaveChanges();
                MessageGateway.Update((int)recipient.MessageId, connectionUsername);               
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "()DataSourceLayer.RecipientGateway.Insert(Recipient recipient, string username)");
                throw;
            }
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
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                List<Recipient> result = dc.Recipients.Where(row => string.Compare(row.RecipientUsername, recipientUsername) == 0
                    && row.Deleted == deleted
                    && row.Viewed == viewed).ToList();
                foreach (Recipient item in result)
                {
                    rows.Add(TranslateToDepletedRecipientObject(item));
                }
                return rows;                
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(List<Recipient>)EFDataSourceLayer.RecipientGateway.Select(string recipientUsername, string connectionUsername, bool deleted, bool viewed)");
                throw;
            }
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
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                List<Recipient> result = dc.Recipients.Where(row => row.MessageId == messageId).ToList();
                foreach (Recipient item in result)
                {
                    rows.Add(TranslateToDepletedRecipientObject(item));
                }
                return rows;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(List<Recipient>)DataSourceLayer.RecipientGateway.Select(int messageId, string connectionUsername)");
                throw;
            }
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
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                Recipient result = dc.Recipients.FirstOrDefault(row => string.Compare(row.RecipientUsername, recipientUsername) == 0
                    && row.MessageId == messageId);
                result.Viewed = viewed;
                dc.SaveChanges();
                MessageGateway.Update(messageId, connectionUsername);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "()EFDataSourceLayer.RecipientGateway.Update(string recipientUsername, int messageId, bool viewed, string connectionUsername)");
                throw;
            }
        }

        public static void UpdateDeleted(string recipientUsername, int messageId, bool deleted, string connectionUsername)
        {
            try
            {
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                Recipient result = dc.Recipients.FirstOrDefault(row => string.Compare(row.RecipientUsername, recipientUsername) == 0
                    && row.MessageId == messageId);
                result.Deleted = deleted;
                dc.SaveChanges();
                MessageGateway.Update(messageId, connectionUsername);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "()EFDataSourceLayer.RecipientGateway.UpdateDeleted(string recipientUsername, int messageId, bool deleted, string connectionUsername)");
                throw;
            }
        }

        public static void Delete(string recipientUsername, int messageId, string connectionUsername)
        {
            try
            {
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                Recipient result = dc.Recipients.FirstOrDefault(row => string.Compare(row.RecipientUsername, recipientUsername) == 0
                    && row.MessageId == messageId);
                dc.Recipients.Remove(result);
                dc.SaveChanges();
                MessageGateway.Update(messageId, connectionUsername);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "()EFDataSourceLayer.RecipientGateway.Delete(string recipientUsername, int messageId, string connectionUsername)");
                throw;
            }
        }

        static Recipient TranslateToDepletedRecipientObject(Recipient efRecipient)
        {
            return new Recipient(efRecipient.RecipientUsername, efRecipient.MessageId)
            {
                Deleted = efRecipient.Deleted,
                Viewed = efRecipient.Viewed
            };
        }
         
    }
        
}
