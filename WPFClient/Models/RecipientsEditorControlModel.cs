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
        public ObservableCollection<Employee> AllEmployees 
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
        ObservableCollection<Employee> selectedEmployees = new ObservableCollection<Employee>();
        public ObservableCollection<Employee> SelectedEmployees 
        {
            get
            {
                return selectedEmployees;
            }
            set
            {
                selectedEmployees = value;
            }
        }

        c A = new c();

        public c a
        {
            get
            {
                return A;
            }
        }
       
    }

    public class c
    {
        public bool b
        {
            get
            {
                return false;
            }
        }
    }
}
