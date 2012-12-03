using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace WPFClient.Additional
{
    public class MessageListItemModel : Message
    {
        Message message = new Message();

        public Message Message
        {
            get
            {
                message.Content = this.Content;
                message.Date = this.Date;
                message.Deleted = this.Deleted;
                message.EDRecipient_MessageId = this.EDRecipient_MessageId;
                message.FKEmployee_SenderUsername = this.FKEmployee_SenderUsername;
                message.SenderUsername = this.SenderUsername;
                message.Title = this.Title;
                return message;                
            }
            set
            {
                if (value != null)
                {                 
                    this.Id = value.Id;
                    this.Content = value.Content;
                    this.Date = value.Date;
                    this.Deleted = value.Deleted;
                    this.EDRecipient_MessageId = value.EDRecipient_MessageId;
                    this.FKEmployee_SenderUsername = value.FKEmployee_SenderUsername;
                    this.SenderUsername = value.SenderUsername;
                    this.Title = value.Title;
                    this.message = value;
                }                
            }
        }

        /// <summary>
        /// Необходимо для триггера в шаблоне элемента MessageList.
        /// </summary>
        bool isViewed = true;

        /// <summary>
        /// Необходимо для триггера в шаблоне элемента MessageList.
        /// </summary>
        public bool IsViewed 
        {
            get
            {
                return isViewed;
            }
            set
            {
                if (isViewed != value)
                    isViewed = value;
            }
        }
    }
}
