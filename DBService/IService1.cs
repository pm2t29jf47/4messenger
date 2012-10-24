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
        /// Возвращает сотрудника по его иднтификатору
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Employee GetEmployee(string username);

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
        List<Entities.Message> GetInboxFolder();

        /// <summary>
        /// Проверяет аутентификационные данные пользователя
        /// </summary>
        [OperationContract]
        void CheckUser();

        /// <summary>
        /// Возвращает коллекцию отправленных писем
        /// </summary>
        [OperationContract]
        List<Message> GetSentFolder();

        /// <summary>
        /// Задает сообщению флаг прочитанности
        /// </summary>
        /// <param name="MessageId"></param>
        [OperationContract]
        void SetMessageViewed(int MessageId);

        /// <summary>
        /// Возвращает письма помеченные удаленными
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Message> GetDeletedFolder();

    }   
}
