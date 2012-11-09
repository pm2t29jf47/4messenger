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
            CheckAddDeleteButtonsEnabled();
        }
   
        /// <summary>
        /// Проверяет и зидиет возможность отображения кнопок активными
        /// </summary>
        void CheckAddDeleteButtonsEnabled()
        {
            AddToSelectedButton.IsEnabled = ((RecipientsEditorControlModel.AllEmployees != null)
                && (RecipientsEditorControlModel.AllEmployees.Count != 0)
                && (AllEmployeesListBox.IsMouseCaptured));

            RemoveFromSelectedButton.IsEnabled = ((RecipientsEditorControlModel.SelectedEmployees != null)
                && (RecipientsEditorControlModel.SelectedEmployees.Count != 0)
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
            RecipientsEditorControlModel.SelectedEmployees.Remove(selectedItem);
            RecipientsEditorControlModel.AllEmployees.Add(selectedItem);
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
