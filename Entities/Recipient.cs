using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideExceptionHandler
{
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

        /// <summary>
        /// Идентификатор сотрудника-получателя
        /// </summary>
        public int EmployeeId
        {
            get;
            internal set;
        }

        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public int MessageId
        {
            get;
            internal set;
        }

        /// <summary>
        /// Флаг для переноса полученых писем в папку удаленных
        /// </summary>
        public bool DeleteByRecipient
        {
            get;
            set;
        }
    }
}
