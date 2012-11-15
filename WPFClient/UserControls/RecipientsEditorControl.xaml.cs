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
using WPFClient.Models;

namespace WPFClient.UserControls
{
    public partial class RecipientsEditorControl : UserControl
    {
        public RecipientsEditorControl()
        {
            InitializeComponent();
        }

        RecipientsEditorControlModel RecipientsEditorControlModel
        {
            get
            {
                if (this.DataContext == null)
                    this.DataContext = new RecipientsEditorControlModel();

                return (RecipientsEditorControlModel)this.DataContext;
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки переноса сотрудника в коллекцию выбраных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAddToSelectedButtonClick(object sender, RoutedEventArgs e)
        {           
            Employee[] selectedItems = new Employee[AllEmployeesListBox.SelectedItems.Count];
            AllEmployeesListBox.SelectedItems.CopyTo(selectedItems, 0);
            foreach (Employee item in selectedItems)
            {
                RecipientsEditorControlModel.SelectedEmployees.Add(item);
                RecipientsEditorControlModel.AllEmployees.Remove(item);
            }
            AddToSelectedButton.IsEnabled = false;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки переноса сотрудника в коллекцию всех возможных для выбора сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRemoveFromSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            Employee[] selectedItems = new Employee[SelectedEmployeesListBox.SelectedItems.Count];
            SelectedEmployeesListBox.SelectedItems.CopyTo(selectedItems, 0);
            foreach (Employee item in selectedItems)
            {               
                RecipientsEditorControlModel.AllEmployees.Add(item);
                RecipientsEditorControlModel.SelectedEmployees.Remove(item);
            }
            AddToSelectedButton.IsEnabled = false;
        }

        private void OnAllEmployeesListBoxIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AddToSelectedButton.IsEnabled = true;
            RemoveFromSelectedButton.IsEnabled = false;
        }

        private void OnSelectedEmployeesListBoxIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AddToSelectedButton.IsEnabled = false;
            RemoveFromSelectedButton.IsEnabled = true;
        }

        private void OnAllEmployeesSelectAllExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AllEmployeesListBox.SelectAll();     
        }

        private void OnSelectedEmployeesSelectAllExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectedEmployeesListBox.SelectAll();
        }
    }
}
