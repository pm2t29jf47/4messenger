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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Entities;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for RecipientsEditor.xaml
    /// </summary>
    public partial class RecipientsEditorControl : UserControl
    {
        public RecipientsEditorControl()
        {
            InitializeComponent();
        }

        public List<Employee> AllEmployeesList
        {
            get
            {
                return (List<Employee>)this.AllEmployeesListBox.ItemsSource;
            }
            set
            {
                this.AllEmployeesListBox.DataContext = value;
            }
        }

        public List<Employee> SelectedEmployeesList
        {
            get
            {
                return (List<Employee>)this.SelectedEmployeesListBox.ItemsSource;
            }
            set
            {
                this.SelectedEmployeesListBox.ItemsSource = value;
            }
        }

        private void OnAllEmployeesListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            AddToSelectedButton.Visibility = System.Windows.Visibility.Visible;

        }

        private void AddToSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            AllEmployeesList.RemoveAt(1);
            this.AllEmployeesListBox.DataContext = AllEmployeesList;
            

        }    
    }
}
