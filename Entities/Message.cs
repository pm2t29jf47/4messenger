﻿using System;
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
            string senderUsername,
            string content,
            bool deleted)
        {
            Id = id;
            Title = title;
            Date = date;
            SenderUsername = senderUsername;
            Content = content;
            Deleted = deleted;
        }

        public Message() { }

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
        /// Флаг для переноса отправленных писем в папку удаленных
        /// </summary>
        [DataMember]
        public bool Deleted { get; set; }

        public Employee FKSenderUsername { get; set; }

        public List<Recipient> ExternalDependensyMessageId { get; set; }
    }
}
