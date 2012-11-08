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
        List<Employee> RecipientsEmployees = new List<Employee>();
        List<Employee> AllEmployees = new List<Employee>();        

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="senderTextboxText"></param>
        /// <param name="recipientTextboxText"></param>
        /// <param name="titleTextboxText"></param>
        public MessageCreator(Message message, List<Employee> allEmployees)
        {
            InitializeComponent();
            MessageControl.DataContext = new MessageControlModel() 
            { 
                AllEmployees = allEmployees, 
                Message = message 
            };
            MessageControl.ControlState = WPFClient.UserControls.MessageControl.state.IsEditable;
        }
        
        /// <summary>
        /// Обработчик нажатия кнопки "Send"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSendMessageButtonClick(object sender, RoutedEventArgs e)
        {
            MessageControlModel mcm = (MessageControlModel)MessageControl.DataContext;
            App.Proxy.SendMessage(mcm.Message);
            this.Close();
        }     
    }
}
