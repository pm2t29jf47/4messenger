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
using System.Collections.ObjectModel;

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

        public ObservableCollection<Employee> AllEmployeesList
        {///ObservableCollection реализует уведомление прицепленых контролов об изменениях
            get
            {
                return (ObservableCollection<Employee>)this.AllEmployeesListBox.ItemsSource;
            }
            set
            {                
                this.AllEmployeesListBox.ItemsSource = value; //перенести в конструктор
            }
        }

        public ObservableCollection<Employee> SelectedEmployeesList
        {
            get
            {
                return (ObservableCollection<Employee>)this.SelectedEmployeesListBox.ItemsSource;
            }
            set
            {
                this.SelectedEmployeesListBox.ItemsSource = value;
            }
        }

        private void OnAddToSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            Employee selectedItem = (Employee)AllEmployeesListBox.SelectedItem;
            SelectedEmployeesList.Add(selectedItem);
            AllEmployeesList.Remove(selectedItem);
            CheckAddDeleteButtonsEnabled();
        }
   
        void CheckAddDeleteButtonsEnabled()
        {
            AddToSelectedButton.IsEnabled = ((AllEmployeesList != null) 
                && (AllEmployeesList.Count != 0) 
                &&(AllEmployeesListBox.IsMouseCaptured));

            RemoveFromSelectedButton.IsEnabled = ((SelectedEmployeesList != null)
                && (SelectedEmployeesList.Count != 0)
                && (SelectedEmployeesListBox.IsMouseCaptured));
        }

        private void OnRemoveFromSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            Employee selectedItem = (Employee)SelectedEmployeesListBox.SelectedItem;
            SelectedEmployeesList.Remove(selectedItem);
            AllEmployeesList.Add(selectedItem);
            CheckAddDeleteButtonsEnabled();
        }

        private void OnSelectedEmployeesListBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            CheckAddDeleteButtonsEnabled();
        }

        private void OnAllEmployeesListBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            CheckAddDeleteButtonsEnabled();
        }
    }
}
