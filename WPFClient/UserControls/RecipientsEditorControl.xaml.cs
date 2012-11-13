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
            Employee selectedItem = (Employee)AllEmployeesListBox.SelectedItem;
            RecipientsEditorControlModel.SelectedEmployees.Add(selectedItem);
            RecipientsEditorControlModel.AllEmployees.Remove(selectedItem);
            AddToSelectedButton.IsEnabled = false;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки переноса сотрудника в коллекцию всех возможных для выбора сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRemoveFromSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            Employee selectedItem = (Employee)SelectedEmployeesListBox.SelectedItem;
            RecipientsEditorControlModel.SelectedEmployees.Remove(selectedItem);
            RecipientsEditorControlModel.AllEmployees.Add(selectedItem);
            RemoveFromSelectedButton.IsEnabled = false;
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
    }
}
