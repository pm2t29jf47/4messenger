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
        /// Конструктор для десериализации
        /// </summary>
        public Employee() { }
        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="EmploeeId"></param>
        /// <param name="Name"></param>
        public Employee(string username, string firstName, string secondName, string role)
        {
            this.Username = username;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.Role = role;
        }  

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
    }
}
