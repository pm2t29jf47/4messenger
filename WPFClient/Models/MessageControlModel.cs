using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace WPFClient.Models
{
    class MessageControlModel
    { 
        /// <summary>
        /// Объект сообщния для отображения или изменения
        /// </summary>    
        Message message = new Message();

        /// <summary>
        /// Объект сообщния для отображения или изменения
        /// </summary>    
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

        /// <summary>
        /// Коллекция содержащая всех возможных для выбора сотрудников
        /// </summary>
        List<Employee> allEmployees = new List<Employee>();

        /// <summary>
        /// Коллекция содержащая всех возможных для выбора сотрудников
        /// </summary>
        public List<Employee> AllEmployees 
        { 
            get
            {
                return allEmployees;
            }
            set
            {
                if(allEmployees == value)
                    return;

                allEmployees = value;
            }
        }

        public void FillMessageEDRecipient_MessageId()
        {
            if(message.EDRecipient_MessageId == null)
                return;

            foreach(Recipient item in message.EDRecipient_MessageId)
            {
                item.FKMessage_MessageId = message;
                item.FKEmployee_RecipientUsername = allEmployees.FirstOrDefault(
                    row => string.Compare(row.Username, item.RecipientUsername) == 0);
            }
        }
    }
}
