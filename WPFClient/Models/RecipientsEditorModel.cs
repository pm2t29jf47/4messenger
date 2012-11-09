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
        public List<Employee> AllEmployees 
        {
            get
            {
                return allEmployees;
            }
            set
            {
                allEmployees = value;
            }
        }

        /// <summary>
        /// Коллекция содержащая выбраных пользователем сотрудников
        /// </summary>
        List<Employee> recipientsEmployees = new List<Employee>();
        public List<Employee> RecipientsEmployees 
        {
            get
            {
                return recipientsEmployees;
            }
            set
            {
                recipientsEmployees = value;
            }
        }
    }
}

            
        
  


