using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary> 
    /// Класс представляющий строку таблицы Message 
    /// </summary>
    [DataContract]
    public class Message : Entity
    {
        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="title"></param>
        /// <param name="date"></param>
        /// <param name="recipients"></param>
        /// <param name="sender"></param>
        /// <param name="content"></param>           
        public Message(int? id)
        {
            Id = id;          
        }

        public Message() { }

        /// <summary> 
        /// Первичный ключ 
        /// </summary>   
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; protected set; }

        /// <summary> 
        /// Название письма 
        /// </summary>
        [DataMember]
        [StringLength(100)]
        [Required]
        public string Title { get; set; }

        /// <summary> 
        /// Дата отправления 
        /// </summary>
        [DataMember]
        [Required]
        public DateTime Date { get; set; }

        /// <summary> 
        /// Содержимое 
        /// </summary>
        [DataMember]
        [StringLength(1000)]
        [Required]
        public string Content { get; set; }

        /// <summary> 
        /// Отпраитель
        /// </summary>
        [DataMember]
        [Required]
        public string SenderUsername { get; set; }

        /// <summary> 
        /// Флаг для переноса отправленных писем в папку удаленных
        /// </summary>
        [DataMember]
        [Required]
        public bool Deleted { get; set; }

        [DataMember]
        public virtual Employee Sender { get; set; }

        [DataMember]       
        public virtual List<Recipient> Recipients { get; set; }
    }
}
