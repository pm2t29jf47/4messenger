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
            bool deleted)
        {
            this.Deleted = deleted;
            this.RecipientUsername = recipientUsername;
            this.MessageId = messageId;
        }

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
    }
}
