using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WPFClient.ToolbarButtons
{
    public class ButtonModel : INotifyPropertyChanged
    {
        public string DisabledImage { get; set; }

        public string EnabledImage  { get; set; }
        
        public string ImagePath
        {
            get
            {
                return isEnabled ? EnabledImage : DisabledImage;
            }
        }

        bool isEnabled = false;

        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                if(value != isEnabled)
                {
                    isEnabled = value;
                    CreatePropertyChanged(new PropertyChangedEventArgs("IsEnabled"));
                    CreatePropertyChanged(new PropertyChangedEventArgs("ImagePath"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void CreatePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
