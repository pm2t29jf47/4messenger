using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace WPFClient.Models
{
    class MessageControlModel
    {
        /// <summary>
        /// Объект сообщния для отображения или изменения
        /// </summary>    
        public Message Message { get; set; }   

        /// <summary>
        /// Коллекция содержащая всех возможных для выбора сотрудников
        /// </summary>
        public List<Employee> AllEmployees { get; set; }
    }
}
