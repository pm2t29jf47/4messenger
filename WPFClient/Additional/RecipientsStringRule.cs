using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Entities;
using System.Windows;

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
            string errorMessage = CheckRecipientString((string)value);
            return string.Compare(errorMessage, string.Empty) == 0 ?
                new ValidationResult(true, null):
                new ValidationResult(false, errorMessage);   
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
            string errorMessage = string.Empty;
            string result;
            ///перевести массив recipientsStringArray в массив username-ов
            foreach (var recipientString in recipientsStringArray)
            {
                result = ProcessRecipietnString(recipientString, recipientsStringArray);
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
        string ProcessRecipietnString(string recipientString, string[] recipientsStringArray)
        {
            string username = ParseUsername(recipientString);
            Employee foundEmployee = AllEmployees.FirstOrDefault(i => (string.Compare(i.Username, username) == 0));
            if(foundEmployee == null)
            {
                return username
                    + space
                    + Properties.Resources.NotFound;
            }
            else if(recipientsStringArray.Count(row => string.Compare(row, recipientString) == 0) > 1)
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

    }
}
