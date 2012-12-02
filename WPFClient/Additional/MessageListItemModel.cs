using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace WPFClient.Additional
{
    class MessageListItemModel : Message
    {  
        public Message Message
        {
            get
            {
                return new Message(this.Id)
                {
                    Content = this.Content,
                    Date = this.Date,
                    Deleted = this.Deleted,
                    EDRecipient_MessageId = this.EDRecipient_MessageId,
                    FKEmployee_SenderUsername = this.FKEmployee_SenderUsername,
                    SenderUsername = this.SenderUsername,
                    Title = this.Title
                };
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
                }                
            }
        }

        /// <summary>
        /// Необходимо для триггера в шаблоне элемента MessageList.
        /// </summary>
        public bool IsUnViewed { get; set; }
    }
}
