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

        public List<Employee> AllEmployees { get; set; }

        List<Employee> RecipientsEmployees { get; set; }

        public List<Recipient> Recipients { get; set; }

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
            string recipients = string.Empty;
            foreach(var item in RecipientsEmployees)
                recipients += EmployeeToString(item) + ";";

            recipients = recipients.Substring(0, recipients.Length - 1);
            RecipientsTextBox.Text = recipients;
            
        }

        private void OnRecipientsTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            string errorMessage;
            if (CheckRecipientTextbox(out errorMessage))
            {
                string result = string.Empty;
                foreach (var employee in RecipientsEmployees)
                    result += EmployeeToString(employee) + ";";

                result = result.Substring(0, result.Length - 1);
                RecipientsTextBox.Text = result;
               /// SendMessageButton.IsEnabled = true;
               /// Событие валидации
            }
            else
            {
              ///  SendMessageButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Производит проверку списка рассылки
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool CheckRecipientTextbox(out string errorMessage)
        {
            RecipientsEmployees.Clear();
            string[] recipientsStringArray = RecipientsTextBox.Text.Split(new char[1] { ';' });

            if (recipientsStringArray.Length == 0)
            {
                errorMessage = Properties.Resources.RecipientsStringNotBeEmpty;
                return false;
            }
            return CheckRecipientStringArray(recipientsStringArray, out errorMessage);
        }

        /// <summary>
        /// Проверяет список рассылки построчно
        /// </summary>
        /// <param name="recipientsStringArray"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool CheckRecipientStringArray(string[] recipientsStringArray, out string errorMessage)
        {
            foreach (var recipientString in recipientsStringArray)
            {
                if (!ProcessRecipietnString(recipientString, out errorMessage))
                    return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Обрабатывает одну строку из списка рассылки
        /// </summary>
        /// <param name="recipientString"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool ProcessRecipietnString(string recipientString, out string errorMessage)
        {
            string username = ParseUsername(recipientString);
            Employee foundEmployee = AllEmployees.FirstOrDefault(i => (string.Compare(i.Username, username) == 0));
            if (foundEmployee != null)
            {
                RecipientsEmployees.Add(foundEmployee);
                errorMessage = string.Empty;
                return true;
            }
            else
            {
                errorMessage = username + " " + Properties.Resources.NotFound;
                return false;
            }
        }

        /// <summary>
        /// Проверяет строку содержащую <username>
        /// </summary>
        /// <param name="recipientString"></param>
        /// <returns></returns>
        private string ParseUsername(string recipientString)
        {
            int begin = recipientString.IndexOf('<'),
                end = recipientString.IndexOf('>');

            if (begin == -1 || end == -1)
            {
                return recipientString;
            }
            else
            {
                begin += 1;
                return recipientString.Substring(begin, end - begin);
            }
        }

        /// <summary>
        /// Преобразует поля класса Employee в строку
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private string EmployeeToString(Employee employee)
        {
            return employee.FirstName + " "
                    + employee.SecondName + " <"
                    + employee.Username + ">";
        }
    }
}


