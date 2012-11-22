using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.ComponentModel;
using WPFClient.Additional;
using SpecialSymbols;

namespace WPFClient.Models
{
    public class RecipientsControlModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Common code

        bool isValid = false;

        public bool IsValid
        {
            get
            {
                return isValid;
            }
            set
            {
                if (isValid == value)
                    return;

                isValid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsValid"));
            }
        }

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
                if (allEmployees == value)
                    return;

                allEmployees = value;
            }
        }

        /// <summary>
        /// Сотрудники получатели
        /// </summary>
        List<Employee> recipientsEmployees = new List<Employee>();

        /// <summary>
        /// Сотрудники получатели
        /// </summary>
        public List<Employee> RecipientsEmployees
        {
            get
            {
                return recipientsEmployees;
            }
            set
            {
                if (recipientsEmployees == value)
                    return;

                recipientsEmployees = value;
            }
        }

        string unrecognizedUsernames = string.Empty;

        public string UnrecognizedUsernames
        {
            get
            {
                return unrecognizedUsernames;
            }
            set
            {
                if (unrecognizedUsernames == value)
                    return;

                unrecognizedUsernames = value;
            }
        }

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
            get
            {
                return recipients;
            }
            set
            {
                if (value == null)                
                   return;

                recipients = value;                
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
                        + SpecialSymbols.SpecialSymbols.userDataDevider
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
            int begin = recipientString.IndexOf(SpecialSymbols.SpecialSymbols.leftUsernameStopper),
                end = recipientString.IndexOf(SpecialSymbols.SpecialSymbols.rightUsernameStopper);

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
        /// Обновляет строку RecipientsString  и коллекцию Recipients по коллекции RecipientsEmployees
        /// </summary>
        /// <remarks>
        /// Необхдимо вызввать при каждом изменении Recipients
        /// </remarks>
        public void UpdateByRecipientsEmployees()
        {
            string buf = EmployeesToString(recipientsEmployees);
            RecipientsString = JoinToString(UnrecognizedUsernames, buf);
            recipients.Clear();
            foreach (var item in recipientsEmployees)
            {
                recipients.Add(
                    new Recipient(
                        item.Username,
                        null,
                        false,
                        false));
            }
        }

        /// <summary>
        /// Обновляет строку RecipientsString  и коллекцию RecipientEmployees по коллекции Recipients
        /// </summary>
        /// <remarks>
        /// Необхдимо вызввать при каждом изменении Recipients
        /// </remarks>
        public void UpdateByRecipients()
        {
            recipientsEmployees.Clear();
            foreach (var item in recipients)
            {
                Employee employee = AllEmployees.FirstOrDefault(row => (string.Compare(row.Username, item.RecipientUsername) == 0));
                if (employee != null)
                    recipientsEmployees.Add(employee);
            }
            string buf = EmployeesToString(recipientsEmployees);
            RecipientsString = JoinToString(UnrecognizedUsernames, buf);  
        }

        /// <summary>
        /// Обновляет коллекцию Recipients по коллекции RecipientsEmployees
        /// </summary>
        /// <remarks>
        /// Вызывается при валидации RecipientString, заполнение коллекции RecipientsEmployees происходит в процессе этой валидации
        /// </remarks>
        void UpdateRecipientsByRecipientsEmployees()
        {
            recipients.Clear();
            foreach (var item in recipientsEmployees)
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
            string[] recipientsSubStrings = recipientsString.Split(SpecialSymbols.SpecialSymbols.userDataDevider);              
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
                    + SpecialSymbols.SpecialSymbols.space
                    + employee.SecondName
                    + SpecialSymbols.SpecialSymbols.space
                    + SpecialSymbols.SpecialSymbols.leftUsernameStopper
                    + employee.Username
                    + SpecialSymbols.SpecialSymbols.rightUsernameStopper;
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
                    result += EmployeeToString(item) + SpecialSymbols.SpecialSymbols.userDataDevider;

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
                    IsValid = true;
                    string result = CheckRecipientString(RecipientsString);
                    if (string.Compare(result, string.Empty) != 0)
                    {
                        msg = result;
                        IsValid = false;
                    }
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
            UnrecognizedUsernames = string.Empty;
            string[] recipients = recipientsString.Split(SpecialSymbols.SpecialSymbols.userDataDevider);
            string errorMessage = string.Empty;
            if ((recipients.Length == 1)
                && (string.Compare(recipients[0], string.Empty) == 0))
            {
                return Properties.Resources.RecipientsStringNotBeEmpty;
            }
            string[] usernames = ParseToUsernames(recipients);          
            string result = ProcessUsernames(usernames);
            UpdateRecipientsByRecipientsEmployees();
            return result;
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
                            + SpecialSymbols.SpecialSymbols.space
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
                foreach (var item in usernames)
                {
                    string msg = string.Empty;
                    Employee foundEmployee = AllEmployees.FirstOrDefault(i => (string.Compare(i.Username, item) == 0));
                    if (foundEmployee == null)                    
                        msg = AddUsernameToUnrecognizedUsernames(item);                   
                    else
                        AddEmployeeToRecipientsEmployees(foundEmployee); 
                    
                    errorMessage = JoinToString(errorMessage, msg);
                }
            }
            return errorMessage;
        }

        /// <summary>
        /// Добавляет пользователя в коллекцию RecipientsEmployees
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Если пользователь уже есть в списке добавление произведено не будет
        /// </remarks>
        void AddEmployeeToRecipientsEmployees(Employee employee)
        {
            if (!RecipientsEmployees.Contains(employee))
                RecipientsEmployees.Add(employee);   
        }

        /// <summary>
        /// Добавляет username нераспознанного пользователя в строку нераспознанных пользователей
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// Текст ошибки
        /// </returns>
        string AddUsernameToUnrecognizedUsernames(string username)
        {            
            UnrecognizedUsernames = JoinToString(UnrecognizedUsernames, username);
            return username
                + SpecialSymbols.SpecialSymbols.space
                + Properties.Resources.NotFound;
        }
         
        #endregion        
    }
}