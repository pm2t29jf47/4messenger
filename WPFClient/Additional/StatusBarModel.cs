using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WPFClient.Additional
{
    class StatusBarModel : INotifyPropertyChanged
    {
        string errorImagePath = "Images/error.png";

        string connectedImagePath = "Images/check.png";

        Exception exception;

        public Exception Exception
        {
            get
            {
                return exception;
            }
            set
            {
                if (!System.Exception.Equals(exception, value))
                {
                    exception = value;
                    CreatePropertyChangedEvent(new PropertyChangedEventArgs("CurrentImagePath"));
                }
            }
        }          

        public string CurrentImagePath
        {
            get
            {
                if (!System.Exception.Equals(Exception, null))
                {
                    return errorImagePath;
                }
                else
                {
                    return connectedImagePath;
                }
            }           
        }


        string shortMessage;

        public string ShortMessage
        {
            get
            {
                return shortMessage;
            }
            set
            {
                if (string.Compare(shortMessage, value) != 0)
                {
                    shortMessage = value;
                    CreatePropertyChangedEvent(new PropertyChangedEventArgs("ShortMessage"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void CreatePropertyChangedEvent(PropertyChangedEventArgs e)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
