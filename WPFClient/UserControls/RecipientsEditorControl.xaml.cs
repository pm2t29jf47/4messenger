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

        /// <summary>
        /// Возвращает модель из DataContex-а
        /// </summary>
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
            Employee firstSelectedItem = (Employee)AllEmployeesListBox.SelectedItem;
            int nextSelection = AllEmployeesListBox.Items.IndexOf(firstSelectedItem);
            AllEmployeesListBox.SelectedItems.CopyTo(selectedItems, 0);            
            foreach (Employee item in selectedItems)
            {
                RecipientsEditorControlModel.SelectedEmployees.Add(item);
                RecipientsEditorControlModel.AllEmployees.Remove(item);
            }
            AllEmployeesListBox.SelectedIndex = nextSelection;
            AddToSelectedButton.IsEnabled = (nextSelection < AllEmployeesListBox.Items.Count) ? true : false;
            AllEmployeesListBox.Focus();

        }

        /// <summary>
        /// Обработчик события нажатия кнопки переноса сотрудника в коллекцию всех возможных для выбора сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRemoveFromSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            Employee[] selectedItems = new Employee[SelectedEmployeesListBox.SelectedItems.Count];
            Employee firstSelectedItem = (Employee)SelectedEmployeesListBox.SelectedItem;
            int nextSelection = SelectedEmployeesListBox.Items.IndexOf(firstSelectedItem);
            SelectedEmployeesListBox.SelectedItems.CopyTo(selectedItems, 0);
            foreach (Employee item in selectedItems)
            {               
                RecipientsEditorControlModel.AllEmployees.Add(item);
                RecipientsEditorControlModel.SelectedEmployees.Remove(item);
            }
            SelectedEmployeesListBox.SelectedIndex = nextSelection;
            RemoveFromSelectedButton.IsEnabled = (nextSelection < SelectedEmployeesListBox.Items.Count) ? true : false;
            AllEmployeesListBox.Focus();
        }

        /// <summary>
        /// Смена состояния кнопок (IsEditable) при переводе фокуса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllEmployeesListBoxIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AddToSelectedButton.IsEnabled = true;
            RemoveFromSelectedButton.IsEnabled = false;
        }

        /// <summary>
        /// Смена состояния кнопок (IsEditable) при переводе фокуса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedEmployeesListBoxIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AddToSelectedButton.IsEnabled = false;
            RemoveFromSelectedButton.IsEnabled = true;
        }

        /// <summary>
        /// Отмечает всех сотрудников в списке AllEmployeesListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllEmployeesSelectAllExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AllEmployeesListBox.SelectAll();     
        }

        /// <summary>
        /// Отмечает всех сотрудников в списке SelectedEmployeesListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedEmployeesSelectAllExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectedEmployeesListBox.SelectAll();
        }
    }
}
