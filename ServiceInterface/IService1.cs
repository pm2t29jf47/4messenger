using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Entities;
using Entities.Additional;

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
        EmployeePack GetAllEmployees(byte[] recentVersion);

        [OperationContract]
        List<string> GetAllEmployeesIds();

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
        [FaultContract(typeof(ArgumentException))]
        void SetMessageDeleted(int messageId, bool deleted);

        [OperationContract]
        void PermanentlyDeleteRecipient(int messageId);

        [OperationContract]
        void PermanentlyDeleteMessage(int messageId);

        [OperationContract]
        MessagesPack GetMessages(FolderType folderType, MessageTypes messageTypes, Byte[] recentVersion);

        [OperationContract]
        List<int> GetMessagesIds(FolderType folderType, MessageTypes messageTypes);
         
    }

    public enum FolderType { inbox, sentbox }; 
    
    [ Flags ]
    public enum MessageTypes { deleted = 1, viewed = 2};
    
}
