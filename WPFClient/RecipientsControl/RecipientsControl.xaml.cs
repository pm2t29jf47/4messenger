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

        List<Employee> allEmployees;

        public List<Employee> AllEmployees 
        {
            get
            {
                if (allEmployees == null)
                    allEmployees = new List<Employee>();

                return allEmployees;
            }
            set
            {
                allEmployees = value;
            }
        }

        List<Employee> recipientsEmployees;

        List<Employee> RecipientsEmployees 
        {
            get
            {
                if (recipientsEmployees == null)
                recipientsEmployees = new List<Employee>();

                return recipientsEmployees;
            }
            set
            {
                recipientsEmployees = value;
            }
        }

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
            List<Employee> residueEmployees = ResidueEmployees();        
            RecipientsEditor recipientsEditor = new RecipientsEditor();
            recipientsEditor.AllEmployees = residueEmployees;
            recipientsEditor.Show();
            recipientsEditor.Closing +=new System.ComponentModel.CancelEventHandler(OnrecipientsEditorClosing);
        }

        List<Employee> ResidueEmployees()
        {
            List<Employee> residue = new List<Employee>();
            foreach (var item in this.AllEmployees)
                residue.Add(item);

            foreach (var item in this.RecipientsEmployees)
                residue.Remove(item);

            return residue;
        }

        /// <summary>
        /// Отлов события закрытия окна RecipientsEditor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnrecipientsEditorClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var recipientsEditor = (RecipientsEditor)sender;            
            this.RecipientsEmployees = recipientsEditor.RecipientsEmployees;
            string result = string.Empty;
            if (RecipientsEmployees.Count == 0) 
                return;            

            foreach(var item in RecipientsEmployees)
                result += EmployeeToString(item) + ";";

            result = result.Substring(0, result.Length - 1);
            this.RecipientsTextBox.Text = result;            
        }

        void OnRecipientsTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            string errorMessage;
            if (CheckRecipientTextbox(out errorMessage))
            {
                string result = string.Empty;
                foreach (var employee in RecipientsEmployees)
                    result += EmployeeToString(employee) + ";";

                result = result.Substring(0, result.Length - 1);
                this.RecipientsTextBox.Text = result;
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
        bool CheckRecipientTextbox(out string errorMessage)
        {
           this.RecipientsEmployees.Clear();
            string[] recipientsStringArray = this.RecipientsTextBox.Text.Split(new char[1] { ';' });

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
        bool CheckRecipientStringArray(string[] recipientsStringArray, out string errorMessage)
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
        bool ProcessRecipietnString(string recipientString, out string errorMessage)
        {
            string username = ParseUsername(recipientString);
            Employee foundEmployee = this.AllEmployees.FirstOrDefault(i => (string.Compare(i.Username, username) == 0));
            if (foundEmployee != null)
            {
               // if (!this.RecipientsEmployees.Contains(foundEmployee))                
                    this.RecipientsEmployees.Add(foundEmployee);
                      
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
        string ParseUsername(string recipientString)
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
        string EmployeeToString(Employee employee)
        {
            string result = string.Empty;
            if (employee != null)
            {
                result = employee.FirstName + " " 
                    + employee.SecondName + " <"
                    + employee.Username + ">";
            }
            return result;
        }

        string EmployeesToString(List<Employee> Employees)
        {
            string result = string.Empty;
            if (Employees != null
                && Employees.Count != 0)
            {
                foreach (var item in Employees)
                    result += EmployeeToString(item) + ";";

                result = result.Substring(0, result.Length - 1);
            }
            return result;           
        }
    }
}


