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
        ObservableCollection<Employee> allEmployees = new ObservableCollection<Employee>();

        /// <summary>
        /// Коллекция содержащая всех возможных для выбора сотрудников
        /// </summary>
        public ObservableCollection<Employee> AllEmployees 
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
        /// Начальное состояние коллекции содержащей всех возможных для выбора сотрудников
        /// </summary>
        ObservableCollection<Employee> savedAllEmployees = new ObservableCollection<Employee>();
    
        /// <summary>
        /// Коллекция содержащая выбраных пользователем сотрудников
        /// </summary>
        ObservableCollection<Employee> selectedEmployees = new ObservableCollection<Employee>();

        /// <summary>
        /// Коллекция содержащая выбраных пользователем сотрудников
        /// </summary>
        public ObservableCollection<Employee> SelectedEmployees 
        {
            get
            {
                return selectedEmployees;
            }
            set
            {
                if (selectedEmployees == value)
                    return;

                selectedEmployees = value;
            }
        }

        /// <summary>
        /// Начальное состояние коллекции содержащей выбраных пользователем сотрудников
        /// </summary>
        ObservableCollection<Employee> savedSelectedEmployees = new ObservableCollection<Employee>();      
    }
}
