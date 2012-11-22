using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Entities
{
    [DataContract]
    public class Recipient
    {
        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="messageId"></param>
        /// <param name="delete"></param>
        public Recipient(string recipientUsername,
            int? messageId,
            bool deleted,
            bool viewed)
        {
            Deleted = deleted;
            RecipientUsername = recipientUsername;
            MessageId = messageId;
            Viewed = viewed;
        }

        public Recipient() { }

        /// <summary>
        /// Идентификатор сотрудника-получателя
        /// </summary>
        [DataMember]
        public string RecipientUsername
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        [DataMember]
        public int? MessageId
        {
            get;
            set;
        }

        /// <summary>
        /// Флаг для переноса полученых писем в папку удаленных
        /// </summary>
        [DataMember]
        public bool Deleted
        {
            get;
            set;
        }

        /// <summary>
        /// Флаг прочтенности письма получателем
        /// </summary>
        [DataMember]
        public bool Viewed
        {
            get;
            set;
        }

        public Employee FKEmployee_RecipientUsername { get; set; }

        public Message FKMessage_MessageId { get; set; }
    }
}
