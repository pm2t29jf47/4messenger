using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Entities
{
    /// <summary> 
    /// Класс представляющий строку таблицы Message 
    /// </summary>
    [DataContract]
    public class Message
    {
        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="MessageId"></param>
        /// <param name="Title"></param>
        /// <param name="Date"></param>
        /// <param name="Recipients"></param>
        /// <param name="Sender"></param>
        /// <param name="Content"></param>           
        public Message(
            int MessageId,
            string Title,
            DateTime Date,
            List<Recipient> Recipients,
            Employee Sender,
            string Content)
        {
            this.MessageId = MessageId;
            this.Title = Title;
            this.Date = Date;
            this.Sender = Sender;
            this.Content = Content;
            this.Recipients = Recipients;
        }

        /// <summary> 
        /// Первичный ключ 
        /// </summary>   
        [DataMember]
        public int MessageId
        {
            get;
            internal set;
        }

        /// <summary> 
        /// Название письма 
        /// </summary>
        [DataMember]
        public string Title
        {
            get;
            set;
        }

        /// <summary> 
        /// Дата отправления 
        /// </summary>
        [DataMember]
        public DateTime Date
        {
            get;
            set;
        }

        /// <summary> 
        /// Содержимое 
        /// </summary>
        [DataMember]
        public string Content
        {
            get;
            set;
        }

        /// <summary> 
        /// Отпраитель
        /// </summary>
        [DataMember]
        public Employee Sender
        {
            get;
            internal set;
        }

        /// <summary> 
        /// Получатели
        /// </summary>
        [DataMember]
        public List<Recipient> Recipients
        {
            get;
            internal set;
        }

        /// <summary> 
        /// Флаг для переноса отправленных писем в папку удаленных
        /// </summary>
        [DataMember]
        public bool DeleteBySender
        {
            get;
            set;
        }
    }
}
