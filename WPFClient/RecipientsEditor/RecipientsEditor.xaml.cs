using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for RecipientsEditor.xaml
    /// </summary>
    public partial class RecipientsEditor : Window
    {
        public RecipientsEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Коллекция содержащая всех возможных для выбора сотрудников
        /// </summary>
        public List<Employee> AllEmployees
        {
            set                
            {              
                if (value == null) 
                    return;

                foreach (var item in value)
                    this.RecipientsEditorControl.AllEmployees.Add(item);                    
            }
        }

        /// <summary>
        /// Коллекция содержащая выбраных пользователем сотрудников
        /// </summary>
        public List<Employee> RecipientsEmployees
        {
            get
            {
                List<Employee> result = new List<Employee>();
                foreach (var item in this.RecipientsEditorControl.SelectedEmployees)
                    result.Add(item);
                return result;
            }
        }
    }
}
