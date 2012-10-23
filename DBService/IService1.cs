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
        /// Возвращает коллекцию содержащую всех сотрудников 
        /// </summary>
        [OperationContract]
        List<Employee> GetEmployeeList();

        /// <summary> 
        /// Производит вставку письма в таблицу Message 
        /// </summary>
        [OperationContract]
        void SendMessage(Message message);

        /// <summary>
        /// Получить письма
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Entities.Message> InboxMessages();

        /// <summary>
        /// Проверяет аутентификационные данные пользователя
        /// </summary>
        [OperationContract]
        void CheckUser();

        /// <summary>
        /// Возвращает коллекцию отправленных писем
        /// </summary>
        [OperationContract]
        List<Message> SentMessages();

        /// <summary>
        /// Задает сообщению флаг прочитанности
        /// </summary>
        /// <param name="MessageId"></param>
        void SetMessageViewed(int MessageId);

        /// <summary>
        /// Возвращает письма помеченные удаленными
        /// </summary>
        /// <returns></returns>
        List<Message> DeletedMessages();

    }   
}
