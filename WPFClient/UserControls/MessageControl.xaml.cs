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
using WPFClient.ControlsModels;
using System.ComponentModel;

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
            Binding bind = new Binding();
            bind.ElementName = "RecipientsControl";
            bind.Path = new PropertyPath("IsValid");
            SetBinding(IsValidProperty, bind); 
        }

        /// <summary>
        /// Объектная модель
        /// </summary>
        private MessageControlModel MessageControlModel
        {
            get
            {
                if (this.DataContext == null)
                    this.DataContext = new MessageControlModel();

                return (MessageControlModel)this.DataContext;
            }
        }

        /// <summary>
        /// Валидность данных MessageControl-a
        /// </summary>
        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool),
            typeof(MessageControl), new UIPropertyMetadata(false), null);

        /// <summary>
        /// Заполняет DataContext RecipientsControl-а
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMessageControlDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SendDataToRecipientsControl();
        }

        void SendDataToRecipientsControl()
        {
            MessageControlModel.FillMessageRecipients();
            RecipientsControlModel rcm = new RecipientsControlModel()
            {
                AllEmployees = MessageControlModel.AllEmployees,
                Recipients = MessageControlModel.Message.Recipients
            };
            rcm.UpdateByRecipients();
            RecipientsControl.DataContext = rcm;
        }
        
        /// <summary>
        /// Два состояния отображения контрола
        /// </summary>
        public enum state { IsReadOnly, IsEditable }        

        /// <summary>
        /// Определяет вариант отображения контрола
        /// </summary>
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
