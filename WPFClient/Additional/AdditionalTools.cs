using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using Entities.Additional;
using System.Threading;
using System.Collections;
using DBService;
using System.ComponentModel;
using System.ServiceModel;
using ServiceInterface;
using System.Data.SqlTypes;

namespace WPFClient.Additional
{
    /// <summary>
    /// im watching u!
    /// </summary>
    class AdditionalTools
    {
        public static byte[] GetMaxTimestamp(List<Entity> Entities)
        {
            byte[] currentMax = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            SqlBinary sqlCurrentMax = new SqlBinary(currentMax);
            SqlBinary sqlCurrent;
            foreach (Entity item in Entities)
            {
                sqlCurrent = new SqlBinary(item.Version);
                if (sqlCurrentMax.CompareTo(sqlCurrent) < 0)
                    sqlCurrentMax = sqlCurrent;
            }
            return sqlCurrentMax.Value;
        }

     


        //public void CreateChannel()
        //{
        //    DestroyCurrentChannel();
        //    Proxy = factory.CreateChannel();
        //}

        //public void DestroyCurrentChannel()
        //{
        //    if (!IService1.Equals(Proxy, null))
        //    {
        //        ((System.ServiceModel.Channels.IChannel)Proxy).Close();
        //        Proxy = null;
        //    }
        //}
  

        //public Exception DataDownloadException { get; private set; }

        //public List<Employee> AllEmployees { get; private set; }

        //public List<Message> InboxMessages { get; private set; }

        //public List<Message> ViewedInboxMessages { get; private set; }

        //public List<Message> DeletedInboxMessages { get; private set; }

        //public List<Message> ViewedDeletedInboxMessages { get; private set; }

        //public List<Message> SentboxMessages { get; private set; }

        //public List<Message> DeletedSentboxMessages { get; private set; }


        //System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        //void OntimerTick(object sender, EventArgs e)
        //{
        //    timer.Stop();
        //    DownloadData();
        //    timer.Start();
        //    CreateDataUpdatedEvent(new PropertyChangedEventArgs("AllData"));
        //}

        //public void ForceDataDownload()
        //{
        //    timer.Stop();
        //    DownloadData();
        //    timer.Start();
        //    CreateDataUpdatedEvent(new PropertyChangedEventArgs("AllData"));
        //}

        //void DownloadData()
        //{
        //    try
        //    {
        //       

        //        MessageTypes messageTypes = MessageTypes.Unknown;
        //        UpdateMessages(FolderType.Inbox,messageTypes, InboxMessages);

        //        messageTypes = MessageTypes.Viewed;
        //        UpdateMessages(FolderType.Inbox, messageTypes, ViewedInboxMessages);

        //        messageTypes = MessageTypes.Deleted;
        //        UpdateMessages(FolderType.Inbox, messageTypes, DeletedInboxMessages);

        //        messageTypes = MessageTypes.Deleted | MessageTypes.Viewed;
        //        UpdateMessages(FolderType.Inbox, messageTypes, ViewedDeletedInboxMessages);

        //        messageTypes = new MessageTypes();
        //        UpdateMessages(FolderType.Sentbox, messageTypes, SentboxMessages);

        //        messageTypes = MessageTypes.Deleted;
        //        UpdateMessages(FolderType.Sentbox, messageTypes, DeletedSentboxMessages);               
        //    }

        //    /// Сервис не отвечает
        //    catch (EndpointNotFoundException ex)
        //    {
        //        HandleDownloadDataException(ex);
        //    }

        //    ///Креденшелы не подходят
        //    catch (System.ServiceModel.Security.MessageSecurityException ex) 
        //    {
        //        HandleDownloadDataException(ex);
        //    }

        //    /// Ошибка в сервисе
        //    /// (маловероятна, при таком варианте скорее сработает ошибка креденшелов,
        //    /// т.к. проверка паролей происходит на каждом запросе к сервису и ей необходима БД)
        //    catch (FaultException<System.ServiceModel.ExceptionDetail> ex) 
        //    {
        //        HandleDownloadDataException(ex);
        //    }
                
        //    /// Остальные исключения
        //    catch (Exception ex) 
        //    {
        //        HandleDownloadDataException(exs);
        //        throw; ///Неизвестное исключение пробасывается дальше
        //    }            
        //}

        







        



      

     
    }
}