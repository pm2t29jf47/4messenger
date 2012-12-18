using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Key, Column(Order = 1)]       
        [StringLength(50)]
        [ForeignKey("RecipientEmployee")]
        public string RecipientUsername { get; internal set; }

        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        [DataMember]
        [Key, Column(Order = 2)]
        [ForeignKey("Message")]
        public int? MessageId { get; internal set;}

        /// <summary>
        /// Флаг для переноса полученых писем в папку удаленных
        /// </summary>
        [DataMember]
        [Required]
        public bool Deleted { get; set; }

        /// <summary>
        /// Флаг прочтенности письма получателем
        /// </summary>
        [DataMember]
        [Required]
        public bool Viewed { get; set; }

        [DataMember]      
        public virtual Employee RecipientEmployee { get; set; } 

        [DataMember]       
        public virtual Message Message { get; set; }
   
    }
}
