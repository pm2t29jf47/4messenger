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
    /// <summary>
    /// Производит валидацию строки получателей в соответсвии с коллекцией всех сотрудников.
    /// </summary>
    /// <remarks>
    /// !!!Вызывается только при вводе ч-з GUI, не вызывается при программном изменении RecipientsString!!!
    /// </remarks>
    class RecipientsStringRule : ValidationRule
    {
        public RecipientsStringRule()
        {
            AllEmployees = new List<Employee>();
        }

        char userDataDevider = ';',
            space = ' ';  

        public List<Employee> AllEmployees { get; set; }

        /// <summary>
        /// Производит валидацию
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            RecipientsControlModel  updatedModel = (RecipientsControlModel)((BindingExpression)value).DataItem;
            string resut = CheckRecipientString(updatedModel.RecipientsString);
            if (string.Compare(resut,string.Empty) == 0)
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, resut);
            }
        }
        
        /// <summary>
        /// Проверяет список рассылки построчно
        /// </summary>
        /// <param name="recipientsStringArray"></param>
        /// <param name="errorMessage"></param>
        /// <returns>
        /// Строка содержащяя все найденные ошибки, если строка пустая - ошибок нет!
        /// </returns>
        string CheckRecipientString(string recipientsString)
        {
            string[] recipients = recipientsString.Split(userDataDevider);
            string errorMessage = string.Empty,
                result;

            if (recipients.Length == 0)
                return Properties.Resources.RecipientsStringNotBeEmpty;

            string[] usernames = ParseToUsernames(recipients);
            foreach (var username in usernames)
            {
                result = ProcessUsername(username, usernames);
                errorMessage = JoinToString(errorMessage, result);
            }
            return errorMessage;
        }

        /// <summary>
        /// Обрабатывает одну строку из списка рассылки
        /// </summary>
        /// <param name="recipientString"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        string ProcessUsername(string username, string[] usernames)
        {           
            Employee foundEmployee = AllEmployees.FirstOrDefault(i => (string.Compare(i.Username, username) == 0));
            if(foundEmployee == null)
            {
                return username
                    + space
                    + Properties.Resources.NotFound;
            }
            else if (usernames.Count(row => string.Compare(row, username) == 0) > 1)
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
        string ParseUsername(string recipient)
        {
            int begin = recipient.IndexOf('<'),
                end = recipient.IndexOf('>');

            if (begin == -1 || end == -1)
            {
                return recipient;
            }
            else
            {
                begin += 1;
                return recipient.Substring(begin, end - begin);
            }
        }

        /// <summary>
        /// Добавляет описание новой ошибки к строке ошибок
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="aditionalErrorMessage"></param>
        /// <returns></returns>
        string JoinToString(string baseString, string additionalString)
        {
            if (string.Compare(additionalString, string.Empty) == 0)
                return baseString;

            if (baseString.Length == 0)
            {
                return additionalString;
            }
            else
            {
                return baseString 
                    + userDataDevider 
                    + additionalString;
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

    }
}
