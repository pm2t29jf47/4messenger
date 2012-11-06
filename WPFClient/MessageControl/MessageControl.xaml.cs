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

        /// <summary>
        /// Объект сообщния для отображения или изменения
        /// </summary>
        public Message Message 
        { 
            get
            {
                Message message = (Message)this.DataContext;
                message.Recipients = RecipientControl.Data.Recipients;
                return message;       
            }
            set
            {
                if (value == null)
                    return;
                this.DataContext = value;
                this.RecipientControl.Data.Recipients = value.Recipients;
            }
        }

        /// <summary>
        /// Коллекция содержащая всех возможных для выбора сотрудников
        /// </summary>
        public List<Employee> AllEmployees
        {
            get
            {
                return RecipientControl.Data.AllEmployees;
            }
            set
            {
                RecipientControl.Data.AllEmployees = value;
            }
        }

        /// <summary>
        /// Два состояния отображения контрола
        /// </summary>
        public enum state { IsReadOnly, IsEditable }

        /// <summary>
        /// Определяет вариант отображения контрола
        /// </summary>
        state controlState;

        /// <summary>
        /// Определяет вариант отображения контрола
        /// </summary>
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

        /// <summary>
        /// Подготавливает контрол для различных вариантов использования
        /// </summary>
        void PrepareControl()
        {
            if (controlState == state.IsReadOnly)
            {
                this.RecipientControl.ControlState = RecipientsControl.state.IsReadOnly;
                this.DateLable.Visibility = System.Windows.Visibility.Visible;
                this.DateTextbox.Visibility = System.Windows.Visibility.Visible;
                this.TitleTextbox.IsReadOnly = true;
                this.MessageContentTextBox.IsReadOnly = true;
              
            }
            else
            {
                this.RecipientControl.ControlState = RecipientsControl.state.IsEditable;
                this.DateLable.Visibility = System.Windows.Visibility.Collapsed;
                this.DateTextbox.Visibility = System.Windows.Visibility.Collapsed;
                this.TitleTextbox.IsReadOnly = false;
                this.MessageContentTextBox.IsReadOnly = false;
            }
        }
    }
}
