using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace WPFClient.Models
{
    class RecipientEditorModel
    {
        /// <summary>
        /// Коллекция содержащая всех возможных для выбора сотрудников
        /// </summary>
        public List<Employee> AllEmployees { get; set; }

        /// <summary>
        /// Коллекция содержащая выбраных пользователем сотрудников
        /// </summary>
        public List<Employee> RecipientsEmployees { get; set; }    
    }
}

