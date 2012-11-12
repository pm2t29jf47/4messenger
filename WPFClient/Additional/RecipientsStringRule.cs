using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Entities;
using System.Windows;
using System.Windows.Data;
using WPFClient.Models;

namespace WPFClient.Additional
{
    public class RecipientsStringRule : ValidationRule
    {
        public RecipientsStringRule()
        {
            AllEmployees = new List<Employee>();
        }

        string userDataDevider = ";",
            space = " ";  

        public List<Employee> AllEmployees { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var errorMessage = (RecipientsControlModel)((BindingExpression)value).DataItem;

            if (errorMessage.IsValid)
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "");
            }
        }


        /// <summary>
        /// Проверяет список рассылки построчно
        /// </summary>
        /// <param name="recipientsStringArray"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        string CheckRecipientString(string recipientsString)
        {
            string[] recipientsStringArray = recipientsString.Split(userDataDevider.ToCharArray());
            if (recipientsStringArray.Length == 0)
                return Properties.Resources.RecipientsStringNotBeEmpty;
            string[] usernameArray = ParseToUsernames(recipientsStringArray);
            string errorMessage = string.Empty,
                result;
            foreach (var recipientString in usernameArray)
            {
                result = ProcessUsername(recipientString, usernameArray);
                errorMessage = JoinToErrorMessage(errorMessage, result);
            }
            return errorMessage;
        }

        /// <summary>
        /// Обрабатывает одну строку из списка рассылки
        /// </summary>
        /// <param name="recipientString"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        string ProcessUsername(string username, string[] usernameArray)
        {           
            Employee foundEmployee = AllEmployees.FirstOrDefault(i => (string.Compare(i.Username, username) == 0));
            if(foundEmployee == null)
            {
                return username
                    + space
                    + Properties.Resources.NotFound;
            }
            else if (usernameArray.Count(row => string.Compare(row, username) == 0) > 1)
            {
                return Properties.Resources.UnuniqueUsername
                                    + space
                                    + username;      
            }
            else
            {
                return string.Empty;
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
        /// Добавляет описание новой ошибки к строке ошибок
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="aditionalErrorMessage"></param>
        /// <returns></returns>
        string JoinToErrorMessage(string errorMessage, string additionalErrorMessage)
        {
            if (string.Compare(additionalErrorMessage, string.Empty) == 0)
                return errorMessage;

            if(errorMessage.Length == 0)
            {
                return additionalErrorMessage;
            }
            else
            {
                return errorMessage 
                    + userDataDevider 
                    + space 
                    + additionalErrorMessage;
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
