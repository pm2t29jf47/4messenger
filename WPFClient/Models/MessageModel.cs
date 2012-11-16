using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace WPFClient.Models
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

        List<Recipient> Recipients
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
    }
}
