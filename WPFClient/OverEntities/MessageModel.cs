using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace WPFClient.OverEntities
{
    public class MessageModel
    {

        Employee senderEmployee = new Employee();

        public Employee SenderEmployee
        {
            get
            {
                return senderEmployee;
            }
            set
            {
                if (senderEmployee == value)
                    return;

                senderEmployee = value;
            }
        }

        Message message = new Message();

        public Message Message
        {
            get
            {
                return message;
            }
            set
            {
                if(message == value)
                    return;

                message = value;
            }
        }

        List<Recipient> recipients = new List<Recipient>();

        public List<Recipient> Recipients
        {
            get
            {
                return recipients;
            }
            set
            {
                if (recipients == value)
                    return;

                recipients = value;
            }
        }   

        public bool Viewed
        {
            get
            {
                return Recipients.FirstOrDefault(row => string.Compare(row.RecipientUsername, App.Username) == 0).Viewed;
            }
        }
    }
}
