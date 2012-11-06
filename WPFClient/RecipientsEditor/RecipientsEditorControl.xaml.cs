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

        /// <summary>
        /// Коллекция содержащая всех возможных для выбора сотрудников
        /// </summary>
        public ObservableCollection<Employee> AllEmployees
        {
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

        /// <summary>
        /// Коллекция содержащая выбраных пользователем сотрудников
        /// </summary>
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

        /// <summary>
        /// Обработчик события нажатия кнопки переноса сотрудника в коллекцию выбраных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAddToSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            Employee selectedItem = (Employee)AllEmployeesListBox.SelectedItem;
            SelectedEmployees.Add(selectedItem);
            AllEmployees.Remove(selectedItem);
            CheckAddDeleteButtonsEnabled();
        }
   
        /// <summary>
        /// Проверяет и зидиет возможность отображения кнопок активными
        /// </summary>
        void CheckAddDeleteButtonsEnabled()
        {
            AddToSelectedButton.IsEnabled = ((AllEmployees != null)
                && (AllEmployees.Count != 0)
                && (AllEmployeesListBox.IsMouseCaptured));

            RemoveFromSelectedButton.IsEnabled = ((SelectedEmployees != null)
                && (SelectedEmployees.Count != 0)
                && (SelectedEmployeesListBox.IsMouseCaptured));
        }

        /// <summary>
        /// Обработчик события нажатия кнопки переноса сотрудника в коллекцию всех возможных для выбора сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRemoveFromSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            Employee selectedItem = (Employee)SelectedEmployeesListBox.SelectedItem;
            SelectedEmployees.Remove(selectedItem);
            AllEmployees.Add(selectedItem);
            CheckAddDeleteButtonsEnabled();
        }

        /// <summary>
        /// Обработчик события клика по SelectedEmployeesListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSelectedEmployeesListBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            CheckAddDeleteButtonsEnabled();
        }

        /// <summary>
        /// Обработчик события клика по AllEmployeesListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAllEmployeesListBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            CheckAddDeleteButtonsEnabled();
        }
    }
}
