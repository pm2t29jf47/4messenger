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

        /// <summary> 
        /// Производит вставку письма в таблицу 
        /// </summary>
        public static int Insert(Message message, string connectionUsername)
        {
            try
            {
                Message depletedMessage = TranslateToDepletedMessageObject(message);
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                dc.Messages.Add(depletedMessage);
                dc.SaveChanges();
                return (int)depletedMessage.Id;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(int)EFDataSourceLayer.MessageGateway.Insert(Message message, string username)");
                throw;
            }
        }
        
        ///<summary>
        ///Возвращает сообщение по его идентификатору 
        ///</summary>
        ///<param name="id"></param>
        ///<param name="connectionUsername"></param>
        ///<returns></returns>
        public static Message Select(int id, string connectionUsername)
        {
            try
            {
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                Message result = dc.Messages.FirstOrDefault(row => row.Id == id);
                return TranslateToDepletedMessageObject(result);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(Message)EFDataSourceLayer.MessageGateway.Select(int id, string connectionUsername)");
                throw;
            }
        }

        /// <summary>
        /// Возвращает отправленные сообщения
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<Message> Select(string senderUsername, string connectionUsername, bool deleted)
        {
            List<Message> rows = new List<Message>();
            try
            {
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                List<Message> result = dc.Messages.Where(row => string.Compare(row.SenderUsername, senderUsername) == 0
                    && row.Deleted == deleted).ToList();
                foreach (Message item in result)
                {
                    rows.Add(TranslateToDepletedMessageObject(item));
                }
                return rows;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(List<Message>)EFDataSourceLayer.MessageGateway.Select(string senderUsername ,string connectionUsername, bool deleted)");
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
            try
            {
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                return dc.Messages.Where(row => string.Compare(senderUsername, row.SenderUsername) == 0
                    && row.Deleted == deleted).Select(row => (int)row.Id).ToList();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "(List<int>)EFDataSourceLayer.MessageGateway.SelectIds(string senderUsername, string connectionUsername, bool deleted)");
                throw;
            }
        }

        public static void Update(int id, string connectionUsername)
        {
            try
            {
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                Message result = dc.Messages.FirstOrDefault(row => row.Id == id);
                result.LastUpdate = DateTime.Now;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "()EFDataSourceLayer.MessageGateway.SelectIds(string senderUsername, string connectionUsername, bool deleted)");
                throw;
            }
        }

    

        public static void Update(int id, bool deleted, string connectionUsername)
        {
            try
            {
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                Message result = dc.Messages.FirstOrDefault(row => row.Id == id);
                result.Deleted = deleted;
                dc.SaveChanges();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "()EFDataSourceLayer.MessageGateway.Update(int id, bool deleted, string connectionUsername)");
                throw;
            }
        }

        public static void Delete(int id, string connectionUsername)
        {
            try
            {
                CustomDbContext dc = Gateway.GetContext(connectionUsername);
                Message result = dc.Messages.FirstOrDefault(row => row.Id == id);
                dc.Messages.Remove(result);
                dc.SaveChanges();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleExcepion(ex, "()EFDataSourceLayer.MessageGateway.Delete(int id, string connectionUsername)");
                throw;
            }
        }

        static Message TranslateToDepletedMessageObject(Message efMessage)
        {
            return new Message(efMessage.Id)
            {
                Content = efMessage.Content,
                Date = efMessage.Date,
                Deleted = efMessage.Deleted,
                LastUpdate = efMessage.LastUpdate,
                SenderUsername = efMessage.SenderUsername,
                Title = efMessage.Title,
                Version = efMessage.Version
            };
        }
        
    }
}