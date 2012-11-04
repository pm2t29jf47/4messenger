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

        public ObservableCollection<Employee> AllEmployees
        {///ObservableCollection реализует уведомление прицепленых контролов об изменениях
            get
            {
                if (this.AllEmployeesListBox.ItemsSource == null)
                    this.AllEmployeesListBox.ItemsSource = new ObservableCollection<Employee>();

                return (ObservableCollection<Employee>)this.AllEmployeesListBox.ItemsSource;
            }
            set
            {                
                this.AllEmployeesListBox.ItemsSource = value;
            }
        }

        public ObservableCollection<Employee> SelectedEmployees
        {
            get
            {
                if (this.SelectedEmployeesListBox.ItemsSource == null)
                    this.SelectedEmployeesListBox.ItemsSource = new ObservableCollection<Employee>();

                return (ObservableCollection<Employee>)this.SelectedEmployeesListBox.ItemsSource;
            }
            set
            {
                this.SelectedEmployeesListBox.ItemsSource = value;
            }
        }

        void OnAddToSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            Employee selectedItem = (Employee)AllEmployeesListBox.SelectedItem;
            this.SelectedEmployees.Add(selectedItem);
            this.AllEmployees.Remove(selectedItem);
            CheckAddDeleteButtonsEnabled();
        }
   
        void CheckAddDeleteButtonsEnabled()
        {
            AddToSelectedButton.IsEnabled = ((this.AllEmployees != null)
                && (this.AllEmployees.Count != 0)
                && (this.AllEmployeesListBox.IsMouseCaptured));

            RemoveFromSelectedButton.IsEnabled = ((this.SelectedEmployees != null)
                && (this.SelectedEmployees.Count != 0)
                && (this.SelectedEmployeesListBox.IsMouseCaptured));
        }

        void OnRemoveFromSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            Employee selectedItem = (Employee)SelectedEmployeesListBox.SelectedItem;
            this.SelectedEmployees.Remove(selectedItem);
            this.AllEmployees.Add(selectedItem);
            CheckAddDeleteButtonsEnabled();
        }

        void OnSelectedEmployeesListBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            CheckAddDeleteButtonsEnabled();
        }

        void OnAllEmployeesListBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            CheckAddDeleteButtonsEnabled();
        }
    }
}
