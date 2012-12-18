using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.ComponentModel.DataAnnotations;



namespace Entities
{
    /// <summary>
    /// Класс представляющий строку таблицы Employee
    /// </summary>  
    [DataContract]
    public class Employee : Entity
    {
        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="EmploeeId"></param>
        /// <param name="Name"></param>
        public Employee( string username )
        {
            Username = username;           
        }

        public Employee() { }

        /// <summary>
        /// Уникальное имя пользователя
        /// </summary>   
        [DataMember]
        [Key] 
        [StringLength (50)]
        public string Username
        {
            get;
            internal set;
        }

        /// <summary>
        /// Пароль
        /// </summary>
        [DataMember]
        [StringLength(50)]
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Имя сотрудника
        /// </summary>
        [DataMember]
        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия сотрудника
        /// </summary>
        [DataMember]
        [StringLength(50)]
        [Required]
        public string SecondName { get; set; }

        /// <summary>
        /// Роль(и)
        /// </summary>
        [DataMember]
        [StringLength(50)]
        [Required]
        public string Role { get; set; }

        [DataMember]
        public List<Message> Sent { get; set; }

        [DataMember]
        public List<Recipient> Recipients { get; set; }

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
                        + SpecialWords.SpecialWords.Space
                        + SpecialWords.SpecialWords.LeftSquareBrackets
                        + result
                        + SpecialWords.SpecialWords.RightSquareBracket;
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
                    + SpecialWords.SpecialWords.Space
                    + SecondName
                    + SpecialWords.SpecialWords.Space
                    + SpecialWords.SpecialWords.LeftPointyBracket
                    + Username
                    + SpecialWords.SpecialWords.RightPointyBracket;
            }
        }

        public static string CollectionToString(List<Employee> employees)
        {
            string result = string.Empty;
            if (employees != null
                && employees.Count != 0)
            {
                foreach (var item in employees)
                    result += item.ToString + SpecialWords.SpecialWords.Semicolon;

                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }
    }
}
