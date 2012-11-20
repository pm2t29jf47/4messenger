using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.OverEntities;

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

        /// <summary>
        /// Коллекция получателей
        /// </summary>
        List<Recipient> recipients = new List<Recipient>();

        /// <summary>
        /// Коллекция получателей
        /// </summary>
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

        EmployeeModel senderEmployeeModel = new EmployeeModel();

        public EmployeeModel SenderEmployeeModel
        {
            get
            {
                return senderEmployeeModel;
            }
            set
            {
                if (senderEmployeeModel == value)
                    return;

                senderEmployeeModel = value;
            }
        }

        
    }
}
