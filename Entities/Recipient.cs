using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Entities
{
    [DataContract]
    public class Recipient
    {
        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="MessageId"></param>
        /// <param name="DeleteByRecipient"></param>
        public Recipient(int EmployeeId,
            int MessageId,
            bool DeleteByRecipient)
        {
            this.DeleteByRecipient = DeleteByRecipient;
            this.EmployeeId = EmployeeId;
            this.MessageId = MessageId;
        }

        public Recipient() { }

        /// <summary>
        /// Идентификатор сотрудника-получателя
        /// </summary>
        [DataMember]
        public int EmployeeId
        {
            get;
            internal set;
        }

        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        [DataMember]
        public int MessageId
        {
            get;
            internal set;
        }

        /// <summary>
        /// Флаг для переноса полученых писем в папку удаленных
        /// </summary>
        [DataMember]
        public bool DeleteByRecipient
        {
            get;
            set;
        }
    }
}
