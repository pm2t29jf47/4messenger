using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFClient.Models
{
    class ConnectionErrorDetailsModel
    {
        public Exception Exception { get; set; }
        void s()
        {
           // Exception.InnerException.Message
        }
    }
}
