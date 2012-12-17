using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Entities
{
    [DataContract]
    public class Recipient : Entity
    {
        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="messageId"></param>
        /// <param name="delete"></param>
        public Recipient(string recipientUsername,
            int? messageId)
        {            
            RecipientUsername = recipientUsername;
            MessageId = messageId;            
        }

        public Recipient() { }

        /// <summary>
        /// Идентификатор сотрудника-получателя
        /// </summary>
        [DataMember]
        public string RecipientUsername { get; internal set; }

        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        [DataMember]
        public int? MessageId { get; internal set;}

        /// <summary>
        /// Флаг для переноса полученых писем в папку удаленных
        /// </summary>
        [DataMember]
        public bool Deleted { get; set; }

        /// <summary>
        /// Флаг прочтенности письма получателем
        /// </summary>
        [DataMember]
        public bool Viewed { get; set; }

        [DataMember]
        public Employee RecipientEmployee { get; set; } 

        [DataMember]
        public Message Message { get; set; }
   
    }
}
