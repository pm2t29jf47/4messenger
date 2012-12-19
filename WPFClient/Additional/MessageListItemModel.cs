using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.SidebarFolders;

namespace WPFClient.Additional
{
    class MessageListItemModel : Message
    {
        Message message = new Message();

        public Message Message
        {
            get
            {
                message.Content = this.Content;
                message.Date = this.Date;
                message.Deleted = this.Deleted;
                message.Recipients = this.Recipients;
                message.Sender = this.Sender;
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
                    this.Recipients = value.Recipients;
                    this.Sender = value.Sender;
                    this.SenderUsername = value.SenderUsername;
                    this.Title = value.Title;
                    this.message = value;
                }                
            }
        }

        /// <summary>
        /// Необходимо для триггера в шаблоне элемента MessageList.
        /// </summary>
        bool viewed = true;

        /// <summary>
        /// Необходимо для триггера в шаблоне элемента MessageList.
        /// </summary>
        public bool Viewed 
        {
            get
            {
                return viewed;
            }
            set
            {
                if (viewed != value)
                    viewed = value;
            }
        }

        public MessageParentType Type { get; set; }
    }

    public enum MessageParentType 
    { 
        Inbox, 
        Sentbox
    }


}
