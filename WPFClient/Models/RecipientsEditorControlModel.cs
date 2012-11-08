using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Collections.ObjectModel;

namespace WPFClient.Models
{
    class RecipientsEditorControlModel
    {
        /// <summary>
        /// Коллекция содержащая всех возможных для выбора сотрудников
        /// </summary>
        public ObservableCollection<Employee> AllEmployees { get; set; }
    
        /// <summary>
        /// Коллекция содержащая выбраных пользователем сотрудников
        /// </summary>
        public ObservableCollection<Employee> SelectedEmployees { get; set; }
    }
}
