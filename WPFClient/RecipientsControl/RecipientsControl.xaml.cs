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

        string leftUsernameStopper = " <",
            rightUsernameStopper = ">",
            usernameDevider = ";",
            space = " ";

        /// <summary>
        /// Два состояния отображения контрола
        /// </summary>
        public enum state { IsReadOnly, IsEditable }

        /// <summary>
        /// Определяет вариант отображения контрола
        /// </summary>
        state controlState;

        /// <summary>
        /// Определяет вариант отображения контрола
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
        /// Все сотрудники
        /// </summary>
        List<Employee> allEmployees;

        /// <summary>
        /// Все сотрудники
        /// </summary>
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

        /// <summary>
        /// Сотрудники получатели
        /// </summary>
        List<Employee> recipientsEmployees;

        /// <summary>
        /// Сотрудники получатели
        /// </summary>
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

        /// <summary>
        /// Получатели
        /// </summary>
        List<Recipient> recipients;

        /// <summary>
        /// Получатели
        /// </summary>
        public List<Recipient> Recipients 
        { 
            get
            {
                return recipients;
               
            }
            set
            {
                if (value == null)
                    return;

                recipients = value;
                RecipientsEmployees.Clear();
                foreach (var item in value)
                {
                    Employee employee = AllEmployees.FirstOrDefault(row => (string.Compare(row.Username,item.RecipientUsername) == 0));
                    if(employee != null)
                        RecipientsEmployees.Add(employee);                   
                }
                ShowRecipientsEmployees();    
            }
        }

        /// <summary>
        /// Отлов события закрытия окна RecipientsEditor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnrecipientsEditorClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RecipientsEditor recipientsEditor = (RecipientsEditor)sender;
            RecipientsEmployees.AddRange(recipientsEditor.RecipientsEmployees);
            this.RecipientsTextBox.Text = EmployeesToString(RecipientsEmployees);            
        }

        /// <summary>
        /// Обработчик события изменения RecipientsTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRecipientsTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            string errorMessage;
            if (CheckRecipientTextbox(out errorMessage))
            {
                this.RecipientsTextBox.Text = EmployeesToString(RecipientsEmployees);
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
           RecipientsEmployees.Clear();
            string[] recipientsStringArray = this.RecipientsTextBox.Text.Split(usernameDevider.ToCharArray());

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
            Employee foundEmployee = AllEmployees.FirstOrDefault(i => (string.Compare(i.Username, username) == 0));
            if (foundEmployee != null)
            {
               // if (!this.RecipientsEmployees.Contains(foundEmployee))                
                    RecipientsEmployees.Add(foundEmployee);
                      
                errorMessage = string.Empty; 
                return true;
            }
            else
            {
                errorMessage = username + space + Properties.Resources.NotFound;
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
                result = employee.FirstName + space
                    + employee.SecondName + leftUsernameStopper
                    + employee.Username + rightUsernameStopper;
            }
            return result;
        }

        /// <summary>
        /// Преобразует коллекцию объектов Employee в строку
        /// </summary>
        /// <param name="Employees"></param>
        /// <returns></returns>
        string EmployeesToString(List<Employee> Employees)
        {
            string result = string.Empty;
            if (Employees != null
                && Employees.Count != 0)
            {
                foreach (var item in Employees)
                    result += EmployeeToString(item) + usernameDevider;

                result = result.Substring(0, result.Length - 1);
            }
            return result;           
        }

        /// <summary>
        /// Отображает коллекция получателей в RecipientsTextBox
        /// </summary>
        void ShowRecipientsEmployees()
        {
            this.RecipientsTextBox.Text = EmployeesToString(RecipientsEmployees);
        }

        /// <summary>
        /// Подготавливает контрол для различных вариантов использования
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

        /// <summary>
        /// Обработчик кнопки добавления полуателей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            List<Employee> residueEmployees = CalcResidueEmployees();
            RecipientsEditor recipientsEditor = new RecipientsEditor();
            recipientsEditor.AllEmployees = residueEmployees;
            recipientsEditor.Show();
            recipientsEditor.Closing += new System.ComponentModel.CancelEventHandler(OnrecipientsEditorClosing);
        }

        /// <summary>
        /// Рассчитывает оставшихся для возмоного добавления сотрудников
        /// </summary>
        /// <returns></returns>
        List<Employee> CalcResidueEmployees()
        {
            List<Employee> residue = new List<Employee>();
            foreach (var item in AllEmployees)
                residue.Add(item);

            foreach (var item in RecipientsEmployees)
                residue.Remove(item);

            return residue;
        }
    }
}


