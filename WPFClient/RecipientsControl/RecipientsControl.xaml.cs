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
    /// Interaction logic for RecipientsControl.xaml
    /// </summary>
    public partial class RecipientsControl : UserControl
    {
        public RecipientsControl()
        {
            InitializeComponent();
        }

        public enum state { IsReadOnly, IsEditable }

        public List<Employee> AllEmployees 
        { get; set; }

        public List<Employee> RecipientsEmployees { get; set; }

        state controlState;

        /// <summary>
        /// Вариант отображения контрола
        /// </summary>
        public state ControlState
        {
            get
            {
                return controlState;
            }
            set
            {
                controlState = value;
                PrepareControl();
            }
        }

        /// <summary>
        /// Подготавливает контрол для разных вариантов использования
        /// </summary>
        void PrepareControl()
        {
            if (controlState == state.IsReadOnly)
            {
                AddButton.Visibility = System.Windows.Visibility.Collapsed;
                RecipientsTextBox.IsReadOnly = true;

            }
            else
            {
                AddButton.Visibility = System.Windows.Visibility.Visible;
                RecipientsTextBox.IsReadOnly = false;
            }
        }

        void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            RecipientsEditor recipientsEditor = new RecipientsEditor(AllEmployees);
            recipientsEditor.Show();
            recipientsEditor.Closing +=new System.ComponentModel.CancelEventHandler(OnrecipientsEditorClosing);
        }

        /// <summary>
        /// Отлов события закрытия окна RecipientsEditor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnrecipientsEditorClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var recipientsEditor = (RecipientsEditor)sender;
            RecipientsEmployees = recipientsEditor.RecipientsEmployees;            
        }
    }
}
