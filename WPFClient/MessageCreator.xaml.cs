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
using System.Windows.Shapes;
using Entities;
using WPFClient.Models;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MessageCreator.xaml
    /// </summary>
    public partial class MessageCreator : Window
    { 
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="senderTextboxText"></param>
        /// <param name="recipientTextboxText"></param>
        /// <param name="titleTextboxText"></param>
        public MessageCreator()
        {
            InitializeComponent();
            DataContextChanged += new DependencyPropertyChangedEventHandler(OnMessageCreatorDataContextChanged);
            
        }

        /// <summary>
        /// Заполняет DataContext MessageControl-а
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMessageCreatorDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MessageControl.DataContext = new MessageControlModel()
            {
                AllEmployees = MessageCreatorModel.AllEmployees,
                Message = MessageCreatorModel.Message               
            };
            MessageControl.ControlState = WPFClient.UserControls.MessageControl.state.IsEditable;
        }
        
        /// <summary>
        /// Модель храняещаяся в DataContext-е
        /// </summary>
        private MessageCreatorModel MessageCreatorModel
        {
            get
            {
                if (this.DataContext == null)
                    this.DataContext = new MessageCreatorModel();

                return (MessageCreatorModel)this.DataContext;
            }
        }
        
        /// <summary>
        /// Посылвет сообщение при нажатии кнопки "Send"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSendMessageButtonClick(object sender, RoutedEventArgs e)
        {           
            MessageControlModel mcm = (MessageControlModel)MessageControl.DataContext;
            
            App.ServiceWatcher.SendMessage(mcm.Message);
            this.Close();
        }       
    }
}
