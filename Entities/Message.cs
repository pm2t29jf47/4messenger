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
        #region new
        ///// <summary>
        ///// Основной конструктор
        ///// </summary>
        ///// <param name="MessageId"></param>
        ///// <param name="Title"></param>
        ///// <param name="Date"></param>
        ///// <param name="Recipients"></param>
        ///// <param name="Sender"></param>
        ///// <param name="Content"></param>           
        //public Message(
        //    int MessageId,
        //    string Title,
        //    DateTime Date,
        //    List<Employee> Recipients,
        //    Employee Sender,
        //    string Content)
        //{
        //    this.MessageId = MessageId;
        //    this.Title = Title;
        //    this.Date = Date;
        //    this.Recipients = Recipients;
        //    this.Sender = Sender;
        //    this.Content = Content;
        //}
    
        ///// <summary> 
        ///// Первичный ключ 
        ///// </summary>   
        //[DataMember]
        //public int MessageId
        //{
        //    get;
        //    internal set;           
        //}

        ///// <summary> 
        ///// Название письма 
        ///// </summary>
        //[DataMember]
        //public string Title
        //{
        //    get;
        //    set;
        //}

        ///// <summary> 
        ///// Дата отправления 
        ///// </summary>
        //[DataMember]
        //public DateTime Date
        //{
        //    get;
        //    set;
        //}  

        ///// <summary> 
        ///// Содержимое 
        ///// </summary>
        //[DataMember]
        //public string Content
        //{
        //    get;
        //    set;
        //}

        ///// <summary> 
        ///// Получатели
        ///// </summary>     
        //[DataMember]
        //public List<Employee> Recipients
        //{
        //    get;
        //    internal set;
        //}

        ///// <summary> 
        ///// Отпраитель
        ///// </summary>
        //[DataMember]
        //public Employee Sender
        //{
        //    get;
        //    internal set;
        //}
        #endregion
        #region old

        /// <summary>
        /// Конструктор для десереализации
        /// </summary>
        public Message() { }

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
            string Recipient,
            string Sender,
            string Content)
        {
            this.MessageId = MessageId;
            this.Title = Title;
            this.Date = Date;
            this.Recipient = Recipient;
            this.Sender = Sender;
            this.Content = Content;
        }

        /// <summary> 
        /// Первичный ключ 
        /// </summary>   
        [DataMember]
        public int MessageId
        {
            get;
            set;
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
        /// Получател id
        /// </summary>     
        [DataMember]
        public int RecipientId
        {
            get;
            set;
        }

        /// <summary> 
        /// Отпраитель id
        /// </summary>
        [DataMember]
        public int SenderId
        {
            get;
            set;
        }

        /// <summary> 
        /// Получател
        /// </summary>     
        
        [DataMember]
        public string Recipient
        {
            get;
            set;
        }

        /// <summary> 
        /// Отпраитель
        /// </summary>
        [DataMember]
        public string Sender
        {
            get;
            set;
        }
        #endregion
    }
}
