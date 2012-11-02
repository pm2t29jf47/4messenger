using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Entities;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MessageControl.xaml
    /// </summary>
    public partial class MessageControl : UserControl
    {
        public MessageControl()
        {     
            InitializeComponent();      
        }

        public Message Message 
        { 
            get
            {
                return (Message) this.DataContext;
            }
            set
            {
                this.DataContext = value;
            }
        }     

        public bool DateIsVisible
        {
            get
            {
                return (DateTextbox.Visibility == System.Windows.Visibility.Visible);
            }
            set
            {
                DateTextbox.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
        }

        public bool TitleIsReadOnly
        {
            get
            {
                return TitleTextbox.IsReadOnly;
            }
            set
            {
                TitleTextbox.IsReadOnly = value;
            }
        }

    }
}
