using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace WPFClient.Models
{
    class MessageModel
    {
        public MessageModel(Message message, Employee senderEmployee)         
        {
            this.message = message;
            this.senderEmployee = senderEmployee;
        }

        Employee senderEmployee;

        public Employee SenderEmployee
        {
            get
            {
                return senderEmployee;
            }
            internal set
            {
                if (senderEmployee == value)
                    return;

                senderEmployee = value;
            }
        }

        Message message;

        /// <summary> 
        /// Первичный ключ 
        /// </summary>   
        public int? Id 
        {
            get
            {
                if (message == null)
                    return null;
                
                return message.Id;
                
             
            }
        }

        /// <summary> 
        /// Название письма 
        /// </summary>
        public string Title 
        {
            get
            {
                if (message == null)
                    return null;
                
                return message.Title;
                
            }
            set
            {
                if (message == null)
                    return;
          
                if (string.Compare(message.Title, value) == 0)
                    return;

                message.Title = value;
            }
        }

        /// <summary> 
        /// Дата отправления 
        /// </summary>
        public DateTime Date 
        {
            get
            {
                if (message == null)
                    return new DateTime();

                return message.Date;
            }
            set
            {
                if (message == null)
                    return;

                if (message.Date == value)
                    return;

                message.Date = value;
            }
        }

        /// <summary> 
        /// Содержимое 
        /// </summary>
        public string Content 
        {
            get
            {
                if (message == null)
                    return null;

                return message.Content;
            }
            set
            {
                if (message == null)
                    return;

                if (string.Compare(message.Content, value) == 0)
                    return;

                message.Content = value;
            }
        }

        /// <summary> 
        /// Отпраитель
        /// </summary>
        public string SenderUsername 
        {
            get
            {
                if (message == null)
                    return null;

                return message.SenderUsername;
            }
        }

        ///Переделать под новую сущность
        ///// <summary> 
        ///// Получатели
        ///// </summary>
        //public List<Recipient> Recipients 
        //{
        //    get
        //    {
        //        if (message == null)
        //            return null;

        //        return message.Recipients;
        //    }
        //    set
        //    {
        //        if (message == null)
        //            return;

        //        if (message.Recipients == value)
        //            return;

        //        message.Recipients = value;
        //    }
        //}

        /// <summary> 
        /// Флаг для переноса отправленных писем в папку удаленных
        /// </summary>
        public bool Deleted 
        {
            get
            {
                if (message == null)
                    return true;

                return message.Deleted;
            }
            set
            {
                if (message == null)
                    return;          

                message.Deleted = value;
            }
        }             
    }
}
