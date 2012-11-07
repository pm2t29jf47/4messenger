using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.ComponentModel;

namespace WPFClient
{
    public class RecipientsControlData : INotifyPropertyChanged
    {

        public RecipientsControlData()
        {
            IsValidated = true;
        }

        string leftUsernameStopper = " <",
            rightUsernameStopper = ">",
            userDataDevider = ";",
            space = " ";

        /// <summary>
        /// Событие изменения строки с получателями
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);            
        }

        /// <summary>
        /// Все сотрудники
        /// </summary>
        public List<Employee> AllEmployees = new List<Employee>();

        /// <summary>
        /// Сотрудники получатели
        /// </summary>
        public List<Employee> RecipientsEmployees = new List<Employee>();

        /// <summary>
        /// Оставшиеся сотрудники (AllEmployees - RecipientsEmployees)
        /// </summary>
        public List<Employee> AllResidueEmployees
        {
            get
            {
                List<Employee> residue = new List<Employee>();
                foreach (var item in AllEmployees)
                    residue.Add(item);

                foreach (var item in RecipientsEmployees)
                    residue.Remove(item);

                return residue;
            }
        }

        /// <summary>
        /// Получатели
        /// </summary>
        public List<Recipient> Recipients
        {
            get
            {
                List<Recipient> recipients = new List<Recipient>();
                foreach (var item in RecipientsEmployees)
                    recipients.Add(new Recipient(item.Username, null, false, false));

                return recipients;
            }
            set
            {
                if (value == null)
                    return;

                RecipientsEmployees.Clear();
                foreach (var item in value)
                {
                    Employee employee = AllEmployees.FirstOrDefault(row => (string.Compare(row.Username, item.RecipientUsername) == 0));
                    if (employee != null)
                        RecipientsEmployees.Add(employee);
                }
                ShowRecipientsEmployees();
            }
        }

        /// <summary>
        /// Результат проверки введенных пользователем данных
        /// </summary>
        public bool IsValidated { get; set; }

        /// <summary>
        /// Строка пользовательского ввода получателей
        /// </summary>
        string recipientsString = string.Empty;

        /// <summary>
        /// Строка пользовательского ввода получателей
        /// </summary>
        public string RecipientsString
        {
            get
            {
                return recipientsString;
            }
            set
            {
                
                if (string.Compare(recipientsString, value) != 0)
                {
                    IsValidated = true;                    
                    recipientsString = OnRecipientsStringChanged(value);
                    OnPropertyChanged(new PropertyChangedEventArgs(""));       
                }
            }
        }

        /// <summary>
        /// Обработчик события изменения RecipientsTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        string OnRecipientsStringChanged(string recipientsString)
        {
            string errorMessage = string.Empty;
            string result = CheckRecipientString(ref errorMessage, recipientsString);            
            return result;
        } 

        /// <summary>
        /// Производит проверку списка рассылки
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        string CheckRecipientString(ref string errorMessage, string recipientsString)
        {
            RecipientsEmployees.Clear();
            string[] recipientsStringArray = recipientsString.Split(userDataDevider.ToCharArray());  
            if (recipientsStringArray.Length == 0)     
            {
                errorMessage = joinToErrorMessage(errorMessage, Properties.Resources.RecipientsStringNotBeEmpty);  
                IsValidated = false;
            }
            
            return CheckRecipientStringArray(recipientsStringArray, ref errorMessage);
        }

        /// <summary>
        /// Проверяет список рассылки построчно
        /// </summary>
        /// <param name="recipientsStringArray"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        string CheckRecipientStringArray(string[] recipientsStringArray, ref string errorMessage)
        {
            string result = string.Empty;
            foreach (var recipientString in recipientsStringArray)
            {
                result += ProcessRecipietnString(recipientString, ref errorMessage) + userDataDevider;                                 
            }
            if (result.Length != 0)
                result = result.Substring(0, result.Length - 1);
    
            return result;
        }

        /// <summary>
        /// Обрабатывает одну строку из списка рассылки
        /// </summary>
        /// <param name="recipientString"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        string ProcessRecipietnString(string recipientString, ref string errorMessage)
        {
            string username = ParseUsername(recipientString);
            Employee foundEmployee = AllEmployees.FirstOrDefault(i => (string.Compare(i.Username, username) == 0));
            if (foundEmployee != null)
            {
                if (!this.RecipientsEmployees.Contains(foundEmployee))
                {
                    RecipientsEmployees.Add(foundEmployee);
                    return EmployeeToString(foundEmployee);
                }
                else
                {
                    errorMessage = joinToErrorMessage(
                    errorMessage,
                    Properties.Resources.UnuniqueUsername 
                    + space 
                    + username);
                    IsValidated = false;
                    return recipientString;
                }
            }
            else
            {
                errorMessage = joinToErrorMessage(
                    errorMessage,
                    username 
                    + space 
                    + Properties.Resources.NotFound);
                IsValidated = false;
                return recipientString;
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
                    result += EmployeeToString(item) + userDataDevider;

                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        /// <summary>
        /// Отображает коллекция получателей в RecipientsTextBox
        /// </summary>
        void ShowRecipientsEmployees()
        {
            RecipientsString = EmployeesToString(RecipientsEmployees);
        }

        /// <summary>
        /// Обновляет строку адресатов
        /// </summary>
        /// <returns></returns>
        public void AddRecipientsEmplyeesToRecipientsString(List<Employee> recipientsEmployees)
        {
            string buf = EmployeesToString(recipientsEmployees);
            if ((RecipientsString.Length != 0) && (buf.Length != 0))
                buf = userDataDevider + buf;
            RecipientsString += buf;
        }

        /// <summary>
        /// Добавляет описание новой ошибки к строке ошибок
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="aditionalErrorMessage"></param>
        /// <returns></returns>
        string joinToErrorMessage(string errorMessage, string aditionalErrorMessage)
        {
            return (errorMessage.Length == 0) 
                ? aditionalErrorMessage 
                : (errorMessage + userDataDevider + space + aditionalErrorMessage);
        }
        
    }
}
