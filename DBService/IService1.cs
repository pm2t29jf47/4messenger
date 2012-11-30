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
        List<Employee> GetAllEmployees();

        /// <summary>
        /// Возвращает сотрудника по его иднтификатору
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Employee GetEmployee(string username);

        /* Сообщения уже содержат в себе получателей
        /// <summary>
        /// Возвращает всех получателей определенного сообщения
        /// </summary>
        /// <param name="MessageId">
        /// Id сообщения по которому осуществляется выборка в таблице
        /// </param>
        /// <returns>
        /// Список получателей
        /// </returns>
        [OperationContract]
        List<Recipient> GetRecipients(int MessageId);
        */

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
        void SendMessage(Message message);

        /// <summary>
        /// Взвращает коллекцию писем из папки входящих
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Message> GetInboxMessages();

        /// <summary>
        /// Возвращает письма помеченные удаленными
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Message> GetDeletedMessages();

        /// <summary>
        /// Возвращает коллекцию отправленных писем
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Message> GetSentboxMessages();

        /// <summary>
        /// Задает флаг прочитанности
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="viewed"></param>
        [OperationContract]
        void SetRecipientViewed(int messageId, bool viewed);

        /// <summary>
        /// Задает флаг удаления
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="deleted"></param>
        [OperationContract]
        void SetRecipientDeleted(int messageId, bool deleted); 
    }   
}
