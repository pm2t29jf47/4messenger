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
            RecipientControl.AllEmployees = AllEmployees;
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

        public List<Employee> AllEmployees 
        {
            get
            {
                return RecipientControl.AllEmployees;
            }
            set
            {
                RecipientControl.AllEmployees = value;
            }
        }

        public enum state { IsReadOnly, IsEditable }

        state controlState;

        public state ControlState
        {
            get
            {
                return controlState;
            }
            set
            {
                controlState = value;
                PrepareControl();
            }
        }

        void PrepareControl()
        {
            if (controlState == state.IsReadOnly)
            {
                RecipientControl.ControlState = RecipientsControl.state.IsReadOnly;
                DateLable.Visibility = System.Windows.Visibility.Visible;
                DateTextbox.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                RecipientControl.ControlState = RecipientsControl.state.IsEditable;
                DateLable.Visibility = System.Windows.Visibility.Collapsed;
                DateTextbox.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
