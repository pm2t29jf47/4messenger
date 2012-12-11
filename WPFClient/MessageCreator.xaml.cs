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
using System.ServiceModel;
using WPFClient.Additional;
using System.Windows.Controls.Primitives;

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
            try
            {
                App.ServiceWatcher.SendMessage(mcm.Message);
                this.Close();
            }
            catch (EndpointNotFoundException ex1)
            {
                InformatonTips.SomeError.Show(ex1.InnerException.Message);
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex1, "()WPFClient.MessageCreator.OnSendMessageButtonClick(object sender, RoutedEventArgs e)");
                if (! StatusBar.Equals(MessageCreatorModel.StatusBar, null) 
                    && ! MessageCreatorModel.StatusBar.DataContext.Equals(null))
                {
                    StatusBarModel statusBarModel = (StatusBarModel)MessageCreatorModel.StatusBar.DataContext;
                    statusBarModel.Exception = ex1;
                    statusBarModel.ShortMessage = Properties.Resources.ConnectionError;
                }
            }
            catch (Exception ex2)
            {
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex2, "()WPFClient.MessageCreator.OnSendMessageButtonClick(object sender, RoutedEventArgs e)");
                throw;
            }
            
        }       
    }
}
