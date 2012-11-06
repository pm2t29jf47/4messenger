using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

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
        /// <param name="messageId"></param>
        /// <param name="title"></param>
        /// <param name="date"></param>
        /// <param name="recipients"></param>
        /// <param name="sender"></param>
        /// <param name="content"></param>           
        public Message(
            int? id,
            string title,
            DateTime date,
            List<Recipient> recipients,
            string senderUsername,
            string content,
            bool deleted)
        {
            Id = id;
            Title = title;
            Date = date;
            SenderUsername = senderUsername;
            Content = content;
            Recipients = recipients;
            Deleted = deleted;
        }

        /// <summary> 
        /// Первичный ключ 
        /// </summary>   
        [DataMember]
        public int? Id { get; internal set; }

        /// <summary> 
        /// Название письма 
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary> 
        /// Дата отправления 
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary> 
        /// Содержимое 
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary> 
        /// Отпраитель
        /// </summary>
        [DataMember]
        public string SenderUsername { get; internal set; }

        /// <summary> 
        /// Получатели
        /// </summary>
        [DataMember]
        public List<Recipient> Recipients { get; set; }

        /// <summary> 
        /// Флаг для переноса отправленных писем в папку удаленных
        /// </summary>
        [DataMember]
        public bool Deleted { get; set; }
    }
}
