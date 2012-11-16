using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace WPFClient.Models
{
    class RecipientsEditorModel
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

        /// <summary>
        /// Коллекция содержащая выбраных пользователем сотрудников
        /// </summary>
        List<Employee> recipientsEmployees = new List<Employee>();

        /// <summary>
        /// Коллекция содержащая выбраных пользователем сотрудников
        /// </summary>
        public List<Employee> RecipientsEmployees 
        {
            get
            {
                return recipientsEmployees;
            }
            set
            {
                if (recipientsEmployees == value)
                    return;

                recipientsEmployees = value;
            }
        }

    }
}

            
        
  


