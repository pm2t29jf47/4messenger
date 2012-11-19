using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.OverEntities;

namespace WPFClient.Models
{
    class MessageCreatorModel
    {
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
                if (allEmployees == value)
                    return;

                allEmployees = value;
            }
        }

        MessageModel messageModel = new MessageModel();

        public MessageModel MessageModel
        {
            get
            {
                return messageModel;
            }
            set
            {
                if (messageModel == value)
                    return;

                messageModel = value;
            }
        }
       
    }
}
