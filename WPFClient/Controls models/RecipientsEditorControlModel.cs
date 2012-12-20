using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Collections.ObjectModel;

namespace WPFClient.ControlsModels
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
    }
}
