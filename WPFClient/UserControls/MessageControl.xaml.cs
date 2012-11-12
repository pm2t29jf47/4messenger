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
using WPFClient.Models;

namespace WPFClient.UserControls
{
    /// <summary>
    /// Interaction logic for MessageControl.xaml
    /// </summary>
    public partial class MessageControl : UserControl
    {
        public MessageControl()
        {     
            InitializeComponent();   
            DataContextChanged += new DependencyPropertyChangedEventHandler(OnMessageControlDataContextChanged);           
        }

        private MessageControlModel MessageControlModel
        {
            get
            {
                if (this.DataContext == null)
                    this.DataContext = new MessageControlModel();

                return (MessageControlModel)this.DataContext;
            }
        }

        void OnMessageControlDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RecipientsControl.DataContext = new RecipientsControlModel()
            {
                AllEmployees = MessageControlModel.AllEmployees,
                Recipients = MessageControlModel.Message.Recipients
            };           
        }

        /// <summary>
        /// Два состояния отображения контрола
        /// </summary>
        public enum state { IsReadOnly, IsEditable }        

        /// <summary>
        /// Определяет вариант отображения контрола
        /// </summary>
        private state controlState;
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
                this.RecipientsControl.ControlState = RecipientsControl.state.IsReadOnly;
                this.DateLable.Visibility = System.Windows.Visibility.Visible;
                this.DateTextbox.Visibility = System.Windows.Visibility.Visible;
                this.TitleTextbox.IsReadOnly = true;
                this.MessageContentTextBox.IsReadOnly = true;
              
            }
            else
            {
                this.RecipientsControl.ControlState = RecipientsControl.state.IsEditable;
                this.DateLable.Visibility = System.Windows.Visibility.Collapsed;
                this.DateTextbox.Visibility = System.Windows.Visibility.Collapsed;
                this.TitleTextbox.IsReadOnly = false;
                this.MessageContentTextBox.IsReadOnly = false;
            }
        }
    }
}
