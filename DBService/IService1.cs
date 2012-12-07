using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Entities;

namespace DBService
{
    [ServiceContract]
    public interface IService1
    {
        /// <summary>
        /// Проверяет аутентификационные данные пользователя
        /// </summary>
        [OperationContract]
        void CheckUser();

        /// <summary> 
        /// Возвращает коллекцию содержащую всех сотрудников 
        /// </summary>
        [OperationContract]
        List<Employee> GetAllEmployees(ref bool error);

        /// <summary>
        /// Возвращает сотрудника по его иднтификатору
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Employee GetEmployee(string username, ref bool error);
               
        /// <summary>
        /// Отослать сообщение
        /// </summary>
        /// <param name="message">
        /// Сущность "Message"
        /// </param>
        /// <param name="recipient">
        /// Коллекция получателей сообщения
        /// </param>
        [OperationContract]
        void SendMessage(Message message, ref bool error);

        /// <summary>
        /// Взвращает коллекцию писем из папки входящих
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Message> GetInboxMessages(ref bool error);

        /// <summary>
        /// Возвращает письма помеченные удаленными в папке входящих
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Message> GetDeletedInboxMessages(ref bool error);

        /// <summary>
        /// Возвращает письма помеченные удаленными в папке отправленных
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Message> GetDeletedSentboxMessages(ref bool error);

        /// <summary>
        /// Возвращает коллекцию отправленных писем
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Message> GetSentboxMessages(ref bool error);

        /// <summary>
        /// Задает флаг прочитанности
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="viewed"></param>
        [OperationContract]
        void SetRecipientViewed(int messageId, bool viewed, ref bool error);

        /// <summary>
        /// Задает флаг удаления
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="deleted"></param>
        [OperationContract]
        void SetRecipientDeleted(int messageId, bool deleted, ref bool error); 
    }   
}
