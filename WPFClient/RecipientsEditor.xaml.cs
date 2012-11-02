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
        public RecipientsEditor(List<Employee> allEmployees)
        {
            InitializeComponent();
            RecipientsEditorControl.AllEmployeesList = new ObservableCollection<Employee>();
            RecipientsEditorControl.SelectedEmployeesList = new ObservableCollection<Employee>();
            foreach (var item in allEmployees)
                RecipientsEditorControl.AllEmployeesList.Add(item);
        }

        /// <summary>
        /// Коллекция получателей
        /// </summary>
        public List<Employee> RecipientsEmployees
        {
            get
            {
                List<Employee> result = new List<Employee>();
                foreach (var item in RecipientsEditorControl.SelectedEmployeesList)
                    result.Add(item);
                return result;
            }
        }
    }
}
