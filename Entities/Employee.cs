using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;



namespace Entities
{
    /// <summary>
    /// Класс представляющий строку таблицы Employee
    /// </summary>  
    [DataContract]
    public class Employee
    {
        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="EmploeeId"></param>
        /// <param name="Name"></param>
        public Employee(
            string username,
            string firstName,
            string secondName,
            string role,
            string password)
        {
            Username = username;
            FirstName = firstName;
            SecondName = secondName;
            Role = role;
            Password = password;
        }

        public Employee() { }

        /// <summary>
        /// Уникальное имя пользователя
        /// </summary>   
        [DataMember]
        public string Username
        {
            get;
            internal set;
        }

        /// <summary>
        /// Пароль
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// Имя сотрудника
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия сотрудника
        /// </summary>
        [DataMember]
        public string SecondName { get; set; }

        /// <summary>
        /// Роль(и)
        /// </summary>
        [DataMember]
        public string Role { get; set; }

        /************************************************************/

        static string currentUsername = string.Empty;

        public static string CurrentUsername
        {
            get
            {
                return currentUsername;
            }
            set
            {
                if (string.Compare(currentUsername, value) == 0)
                    return;

                currentUsername = value;
            }
        }

        static string namePrefix
        {
            get
            {
                return namePrefix;
            }
            set
            {
                if (string.Compare(namePrefix, value) == 0)
                    return;

                namePrefix = value;
            }
        }

        public static string NamePrefix { get; set; }

        public string ToLongString
        {
            get
            {
                string result = this.ToString;
                if (string.Compare(CurrentUsername, Username) == 0)
                {
                    return NamePrefix
                        + SpecialSymbols.SpecialSymbols.space
                        + SpecialSymbols.SpecialSymbols.leftTitleStopper
                        + result
                        + SpecialSymbols.SpecialSymbols.rightTitleStopper;
                }
                return result;
            }
        }

        public string ToString
        {
            get
            {
                string result = string.Empty;
                return FirstName
                    + SpecialSymbols.SpecialSymbols.space
                    + SecondName
                    + SpecialSymbols.SpecialSymbols.space
                    + SpecialSymbols.SpecialSymbols.leftUsernameStopper
                    + Username
                    + SpecialSymbols.SpecialSymbols.rightUsernameStopper;
            }
        }

        public static string CollectionToString(List<Employee> employees)
        {
            string result = string.Empty;
            if (employees != null
                && employees.Count != 0)
            {
                foreach (var item in employees)
                    result += item.ToString + SpecialSymbols.SpecialSymbols.userDataDevider;

                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        public List<Message> EDMessage_SenderUsername { get; set; }

        public List<Recipient> EDRecipient_RecipientUsername { get; set; }
    }
}
