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
        public Employee(int EmploeeId, string Name)
        {
            this.EmployeeId = EmploeeId;
            this.Name = Name;
        }  

        /// <summary>
        /// Первичный ключ
        /// </summary>   
        [DataMember]
        public int EmployeeId
        {
            get;
            set;         
        }

        /// <summary>
        /// Имя сотрудника
        /// </summary>
        [DataMember]
        public string Name
        {
            get;
            set;
        }
    }
}
