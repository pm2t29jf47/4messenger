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
            SendDataToMessageControl();
        }

        void SendDataToMessageControl()
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
        MessageCreatorModel MessageCreatorModel
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
        void OnSendMessageButtonClick(object sender, RoutedEventArgs e)
        {
            SendMessage();  
        }

        void SendMessage()
        {
            MessageControlModel mcm = (MessageControlModel)MessageControl.DataContext;
            try
            {
                mcm.Message.Sender = null;
                App.ServiceWatcher.SendMessage(mcm.Message);
                App.ServiceWatcher.ForceDataDownload();
                this.Close();
            }

            /// Сервис не отвечает
            catch (EndpointNotFoundException ex)
            {
                HandleSendMessageException(ex);
            }

            ///Креденшелы не подходят
            catch (System.ServiceModel.Security.MessageSecurityException ex)
            {
                HandleSendMessageException(ex);
            }

            /// Ошибка в сервисе
            /// (маловероятна, при таком варианте скорее сработает ошибка креденшелов,
            /// т.к. проверка паролей происходит на каждом запросе к сервису и ей необходима БД)
            catch (FaultException<System.ServiceModel.ExceptionDetail> ex)
            {
                HandleSendMessageException(ex);
            }

            /// Остальные исключения, в т.ч. ArgumentException, ArgumentNullException
            catch (Exception ex)
            {
                HandleSendMessageException(ex);
                throw;
            } 
        }

        void HandleSendMessageException(Exception ex)
        {
            InformatonTips.SomeError.Show(ex.Message);
            ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.MessageCreator.OnSendMessageButtonClick(object sender, RoutedEventArgs e)");
            StatusBarModel statusBarModel = (StatusBarModel)MessageCreatorModel.StatusBar.DataContext;
            statusBarModel.Exception = ex;
            statusBarModel.ShortMessage = Properties.Resources.ConnectionError;
        }
    }
}
