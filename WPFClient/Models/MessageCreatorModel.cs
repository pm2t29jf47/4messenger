using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Windows.Controls.Primitives;

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

        Message message = new Message();

        public Message Message
        {
            get
            {
                return message;
            }
            set
            {
                if (message == value)
                    return;

                message = value;
            }
        }

        public StatusBar StatusBar { get; set; }
       
    }
}
