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

        void OnMessageControlDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MessageControlModel mcm = (MessageControlModel) this.DataContext;
            RecipientsControl.DataContext = new RecipientsControlModel()
            {
                AllEmployees = mcm.AllEmployees,
                Recipients = mcm.Message.Recipients
            };          
        }

        /// <summary>
        /// Два состояния отображения контрола
        /// </summary>
        public enum state { IsReadOnly, IsEditable }

        private state controlState;

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
