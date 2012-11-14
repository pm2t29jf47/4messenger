﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.ComponentModel;
using WPFClient.Additional;

namespace WPFClient.Models
{
    public class RecipientsControlModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Common code

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

        /// <summary>
        /// Все сотрудники
        /// </summary>
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

        /// <summary>
        /// Оставшиеся сотрудники (AllEmployees - RecipientsEmployees)
        /// </summary>
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

        /// <summary>
        /// Получатели
        /// </summary>  
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
                if (string.Compare(recipientsString, value) == 0)
                    return;                
                recipientsString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RecipientsString"));             
            }
        }

        /// <summary>
        /// автоматически сгенерированный код
        /// </summary>
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Добавляет описание новой ошибки к строке ошибок!!!!!!!!!!!!!!!
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="aditionalErrorMessage"></param>
        /// <returns></returns>
        string JoinToString(string baseString, string additionalString)
        {
            if (baseString.Length != 0)
            {
                if (additionalString.Length != 0)
                {
                    return baseString
                        + userDataDevider
                        + additionalString;
                }
                else
                {
                    return baseString;
                }
            }
            else
            {
                if (additionalString.Length != 0)
                {
                    return additionalString;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Проверяет строку содержащую <username>
        /// </summary>
        /// <param name="recipientString"></param>
        /// <returns></returns>
        string ParseUsername(string recipientString)
        {
            int begin = recipientString.IndexOf(leftUsernameStopper),
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
        /// Преобразует в массив username-ов
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        string[] ParseToUsernames(string[] array)
        {
            string[] result = new string[array.Count()];
            for (int i = 0; i < array.Count(); i++)
                result[i] = ParseUsername(array[i]);

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
            UpdateRecipients();
        }

        /// <summary>
        /// Заполнякт Recipients по коллекции RecipientsEmployees
        /// </summary>
        /// <remarks>
        /// Необходимо обновлять !объект! recipients при каждом изменении коллекции RecipientsEmployees.
        /// </remarks>
        private void UpdateRecipients()
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

        #endregion

        #region Update recipients defenition

        /// <summary>
        /// Обработчик события изменения RecipientsTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateRecipientsDefenitionInRecipientsString()
        {
            string[] recipientsSubStrings = recipientsString.Split(userDataDevider);              
            string result = string.Empty,
                fullRecipientDefenition;
            string[] usernames = ParseToUsernames(recipientsSubStrings);
            foreach (var item in usernames)
            {
                fullRecipientDefenition = ProcessUsername(item);
                result = JoinToString(result, fullRecipientDefenition);
            }
            RecipientsString = result;
        }

        /// <summary>
        /// Обрабатывает одну строку из списка рассылки
        /// </summary>
        /// <param name="recipientString"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        string ProcessUsername(string username)
        {
            Employee foundEmployee = AllEmployees.FirstOrDefault(i => (string.Compare(i.Username, username) == 0));
            if (foundEmployee != null)                  
                return EmployeeToString(foundEmployee);                             
            
            return username;
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

        #endregion
     
        #region RecipientsString validation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public string this[string property]
        {
            get
            {
                string msg = null;
                if (string.Compare(property, "RecipientsString") == 0)
                {
                    string result = CheckRecipientString(RecipientsString);
                    if (string.Compare(result, string.Empty) != 0)
                        msg = result;               
                }
                else
                {
                    throw new ArgumentException("Unrecognized property: " + property);
                }
                return msg;            
            }
        }      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipientsString"></param>
        /// <returns></returns>
        string CheckRecipientString(string recipientsString)
        {
            RecipientsEmployees.Clear();
            string[] recipients = recipientsString.Split(userDataDevider);
            string errorMessage = string.Empty;
            if ((recipients.Length == 1)
                && (string.Compare(recipients[0], string.Empty) == 0))
            {
                return Properties.Resources.RecipientsStringNotBeEmpty;
            }
            string[] usernames = ParseToUsernames(recipients);          
            return ProcessUsernames(usernames);
        }

        /// <summary>
        /// Обрабатывает одну строку из списка рассылки
        /// </summary>
        /// <param name="recipientString"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        string ProcessUsernames(string[] usernames)
        {
            string errorMessage = string.Empty;
            string checkUniquenessUsenamesErrorMessage = CheckUniquenessUsenames(usernames);
            string checkUsernameExistenceErrorMessage = CheckUsernameExistence(usernames);
            errorMessage = JoinToString(errorMessage, checkUniquenessUsenamesErrorMessage);
            errorMessage = JoinToString(errorMessage, checkUsernameExistenceErrorMessage);
            return errorMessage;
        }  

        /// <summary>
        /// Проверяет массив username-ов на уникальность
        /// </summary>
        /// <param name="usernames"></param>
        /// <returns>
        /// Возвращает строку содержащую ошибки проверки
        /// </returns>
        string CheckUniquenessUsenames(string[] usernames)
        {
            string errorMessage = string.Empty;
            if(usernames != null)
            {
                List<string> buf = new List<string>();
                foreach(string item in usernames)
                {
                    if (buf.Contains(item, new CustomStringComparer()))
                    {
                        string msg = Properties.Resources.UnuniqueUsername
                            + space
                            + item;

                        errorMessage = JoinToString(errorMessage, msg);
                    }
                    else
                    {
                        buf.Add(item);
                    }
                }
            }
            return errorMessage;
        }

        /// <summary>
        /// Проверяет существование  пользователя по его username
        /// </summary>
        /// <param name="usernames"></param>
        /// <returns>
        /// Возвращает строку содержащую ошибки проверки
        /// </returns>
        string CheckUsernameExistence(string[] usernames)
        {
            string errorMessage = string.Empty;
            if (usernames != null)
            {
                string[] distinctUsernames = usernames.Distinct(new CustomStringComparer()).ToArray();
                foreach (var item in distinctUsernames)
                {
                    string msg;
                    Employee foundEmployee = AllEmployees.FirstOrDefault(i => (string.Compare(i.Username, item) == 0));
                    if (foundEmployee == null)
                    {
                        msg = item
                            + space
                            + Properties.Resources.NotFound;
                    }
                    else
                    {
                        RecipientsEmployees.Add(foundEmployee);
                        msg = string.Empty;
                    }
                    errorMessage = JoinToString(errorMessage, msg);
                }
            }
            return errorMessage;
        }
         
        #endregion        
    }
}