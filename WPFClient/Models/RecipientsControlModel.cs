using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.ComponentModel;

namespace WPFClient.Models
{
    public class RecipientsControlModel : INotifyPropertyChanged
    {
        char leftUsernameStopper = '<',
            rightUsernameStopper = '>',
            userDataDevider = ';',
            space = ' ';

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
        List<Employee> allEmployees = new List<Employee>();
        public List<Employee> AllEmployees
        {
            get
            {
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
        List<Employee> RecipientsEmployees = new List<Employee>();

        /// <summary>
        /// Оставшиеся сотрудники (AllEmployees - RecipientsEmployees)
        /// </summary>
        List<Employee> allResidueEmployees = new List<Employee>();
        public List<Employee> AllResidueEmployees
        {
            get
            {
                allResidueEmployees.Clear();
                foreach (var item in AllEmployees)
                    allResidueEmployees.Add(item);

                foreach (var item in RecipientsEmployees)
                    allResidueEmployees.Remove(item);

                return allResidueEmployees;
            }
        }

        /// <summary>
        /// Получатели
        /// </summary>  
        List<Recipient> recipients = new List<Recipient>();
        public List<Recipient> Recipients
        {
            set
            {
                if (value == null)
                {
                   return;
                }
                else
                {           
                    recipients = value;
                    RecipientsEmployees.Clear();
                    foreach (var item in value)
                    {
                        Employee employee = AllEmployees.FirstOrDefault(row => (string.Compare(row.Username, item.RecipientUsername) == 0));
                        if (employee != null)
                            RecipientsEmployees.Add(employee);
                    }
                }
            }
        }

        /// <summary>
        /// Результат проверки введенных пользователем данных
        /// </summary>
        public bool IsValid { get; private set; }

        private string validationErrorMessage = string.Empty;
        public string ValidationErrorMessage 
        {
            get
            {
                return validationErrorMessage;
            }
            set
            {
                validationErrorMessage = value;
            }
        }

        /// <summary>
        /// Строка пользовательского ввода получателей
        /// </summary>
        string recipientsString = string.Empty;
        public string RecipientsString
        {
            get
            {
                return recipientsString;
            }
            set
            {
                IsValid = true;                
                recipientsString = OnRecipientsStringChanged(value);
                OnPropertyChanged(new PropertyChangedEventArgs("RecipientsString"));             
            }
        }

        /// <summary>
        /// Обработчик события изменения RecipientsTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        string OnRecipientsStringChanged(string recipientsString)
        {
            ValidationErrorMessage = string.Empty;
            return CheckRecipientsString(ref validationErrorMessage, recipientsString);            
        } 

        /// <summary>
        /// Производит проверку списка рассылки
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        string CheckRecipientsString(ref string errorMessage, string recipientsString)
        {
            RecipientsEmployees.Clear();
            string[] recipientsStringArray = recipientsString.Split(userDataDevider);  
            if (recipientsStringArray.Length == 0)     
            {
                errorMessage = joinToString(errorMessage, Properties.Resources.RecipientsStringNotBeEmpty);  
                IsValid = false;
            }            
            return CheckRecipientsSubstrings(recipientsStringArray, ref errorMessage);
        }

        /// <summary>
        /// Проверяет список рассылки построчно
        /// </summary>
        /// <param name="recipientsStringArray"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        string CheckRecipientsSubstrings(string[] recipientsSubstrings, ref string errorMessage)
        {
            string result = string.Empty,
                processedEmployee;
            string[] usernames = ParseToUsernames(recipientsSubstrings);
            foreach (var username in usernames)
            {
                processedEmployee = ProcessUsername(username, ref errorMessage);
                result = joinToString(result, processedEmployee);
            }

            return result;
        }

        /// <summary>
        /// Обрабатывает одну строку из списка рассылки
        /// </summary>
        /// <param name="recipientString"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        string ProcessUsername(string username, ref string errorMessage)
        {          
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
                    errorMessage = joinToString(
                    errorMessage,
                    Properties.Resources.UnuniqueUsername 
                    + space 
                    + username);
                    IsValid = false;
                    return username;
                }
            }
            else
            {
                errorMessage = joinToString(
                    errorMessage,
                    username 
                    + space 
                    + Properties.Resources.NotFound);
                IsValid = false;
                return username;
            }
        }

        /// <summary>
        /// Проверяет строку содержащую <username>
        /// </summary>
        /// <param name="recipientString"></param>
        /// <returns></returns>
        string ParseUsername(string recipientString)
        {
            int begin = recipientString.IndexOf(leftUsernameStopper),////////////////////////////
                end = recipientString.IndexOf(rightUsernameStopper);

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
                result = employee.FirstName 
                    + space
                    + employee.SecondName 
                    + space 
                    + leftUsernameStopper
                    + employee.Username 
                    + rightUsernameStopper;
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
        /// Обновляет строку адресатов
        /// </summary>
        /// <returns></returns>
        public void AddEmployeesToRecipientsString(List<Employee> Employees)
        {
            string buf = EmployeesToString(Employees);
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
        string joinToString(string baseString, string aditionalString)
        {
            return (baseString.Length == 0)
                ? aditionalString
                : (baseString + userDataDevider + aditionalString);
        }

        

        public void UpdateRecipients()
        {
            recipients.Clear();
            foreach (var item in RecipientsEmployees)
            {
                recipients.Add(
                    new Recipient(
                        item.Username,
                        null,
                        false,
                        false));
            }
        }

        string[] ParseToUsernames(string[] array)
        {
            string[] result = new string[array.Count()];
            for (int i = 0; i < array.Count(); i++)
                result[i] = ParseUsername(array[i]);

            return result;
        }
     
    }
}