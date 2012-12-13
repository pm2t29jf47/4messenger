using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Entities;

namespace ServiceInterface
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
        [FaultContract(typeof(ArgumentNullException))]
        Employee GetEmployee(string username);
               
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
        [FaultContract(typeof(ArgumentException))]
        [FaultContract(typeof(ArgumentNullException))]
        void SendMessage(Message message);

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

        [OperationContract]
        List<Message> GetMessages(Folder folder, bool deleted, bool viewed);
         
    }
    public enum Folder { inbox, sentbox };       
    
}
