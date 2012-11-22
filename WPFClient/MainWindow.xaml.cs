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
using WPFClient.UserControls;
using WPFClient.Additional;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Common code

        /// <summary>
        /// Коллекция содержащая всех сотрудников
        /// </summary>
        /// <remarks>
        /// Обновляется при запуске и при получении сообщения от ногового сотрудника, который еще не содержится в ней
        /// </remarks>
        List<Employee> allEmployees;

        List<SidebarFolder> folders = new List<SidebarFolder>();

        bool inboxFolderPressed = false;

        public MainWindow()
        {
            Loaded += new RoutedEventHandler(OnMainWindowLoaded);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en");
            InitializeComponent();
        }

        void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            PrepareWindow();
            ShowLoginWindow();
            PrepareEmployeeClass();
            allEmployees = App.Proxy.GetAllEmployees();
        }

        public void PrepareWindow()
        {
            PreareSidebar();
            HideToolbarButtons(true);
            MessageControl.ControlState = MessageControl.state.IsReadOnly;
        }

        void PrepareEmployeeClass()
        {
            Employee.CurrentUsername = App.Username;
            Employee.NamePrefix = Properties.Resources.Me;
        }
        
        void HideToolbarButtons(bool state)
        {
            this.ReplyMessageButton.Visibility = state ? Visibility.Collapsed : Visibility.Visible;
            this.DeleteMessageButton.Visibility = state ? Visibility.Collapsed : Visibility.Visible; 
        }

        void PreareSidebar()
        {  
            FillFoldersNames();
            Sidebar.ItemsSource = folders;
        }

        void FillFoldersNames()
        {
            folders.Add(new SidebarFolder(Properties.Resources.InboxFolderLabel));
            folders.Add(new SidebarFolder(Properties.Resources.SentboxFolderLabel));
            folders.Add(new SidebarFolder(Properties.Resources.DeletedFolderLabel));
            folders.Add(new SidebarFolder("большая кнопка"));
        }

        void ShowLoginWindow()
        {
            this.Hide();
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }

        void OnMessageListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MessageList.SelectedItem == null)
                return;

            Message selectedMessage = (Message)MessageList.SelectedItem;            
            //помечает открытое письмо прочитанным
            //if (inboxFolderPressed)
            //{
            //    Recipient recipient = messagemodel.Recipients.FirstOrDefault(row => string.Compare(row.RecipientUsername, App.Username) == 0);
            //    if ((recipient != null)
            //        && (!recipient.Viewed))
            //    {
            //        App.Proxy.SetInboxMessageViewed((int)messagemodel.Message.Id);
            //    }
            //}
            MessageControl.DataContext = new MessageControlModel()
            {
                AllEmployees = allEmployees,
                Message = selectedMessage                
            };
            HideToolbarButtons(false);       
        }

        #endregion

        #region Prepare "MessageList" by foldel click

        /// <summary>
        /// Общий обработчик для нажатий на папки в Sidebar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnFolderClick(object sender, RoutedEventArgs e)
        {
            Button selectedFolder = (Button)sender;
            StackPanel buttonStackPanel = (StackPanel)selectedFolder.Content;
            TextBlock buttonTextBlock = (TextBlock)buttonStackPanel.Children[1];
            
            HideToolbarButtons(true);
            if (string.Compare(buttonTextBlock.Text, Properties.Resources.DeletedFolderLabel) == 0)
                PrepareMessageListForDeletedFolder();
            else if (string.Compare(buttonTextBlock.Text, Properties.Resources.InboxFolderLabel) == 0)
                PrepareMessageListForInboxFolder();
            else if (string.Compare(buttonTextBlock.Text, Properties.Resources.SentboxFolderLabel) == 0)
                PrepareMessageListForSentboxFolder();
            else
                OnUserFolderClick();            
        }

        void PrepareMessageListForInboxFolder()
        {
            inboxFolderPressed = true;
            MessageList.ItemTemplate = (DataTemplate)FindResource("ForInboxFolderTemplate");
            List<Message> inboxMessages = App.Proxy.GetInboxMessages();
            FillMessages(inboxMessages);
            MessageList.ItemsSource = inboxMessages;

        }

        void PrepareMessageListForSentboxFolder()
        {
            inboxFolderPressed = false;
            MessageList.ItemTemplate = (DataTemplate)FindResource("DefaultFolderTemplate");
            List<Message> sentboxMessages = App.Proxy.GetSentboxMessages();
            FillMessages(sentboxMessages);
            MessageList.ItemsSource = sentboxMessages;
        }

        void PrepareMessageListForDeletedFolder()
        {
            inboxFolderPressed = false;
            MessageList.ItemTemplate = (DataTemplate)FindResource("DefaultFolderTemplate");
            List<Message> deletedMessages = App.Proxy.GetDeletedMessages();
            FillMessages(deletedMessages);
            MessageList.ItemsSource = deletedMessages;
        }

        /// <summary>
        /// Обработка нажатия на пользовательскую папку
        /// </summary>
        void OnUserFolderClick()
        {
            MessageBox.Show("Не нажимать!");
        }

        void FillMessages(List<Message> messages)
        {
            foreach (var item in messages)
            {
                item.FKEmployee_SenderUsername = App.Proxy.GetEmployee(item.SenderUsername);
                item.EDRecipient_MessageId = App.Proxy.GetRecipients((int)item.Id);
            }
        }
        #endregion

        #region "Create" "Reply" "Delete" buttons

        void OnCreateMessageButtonClick(object sender, RoutedEventArgs e) 
        {
            CreateNewMessage();       
        }

        void OnReplyMessageButtonClick(object sender, RoutedEventArgs e)
        {
            CreateReplyMessage();
        }

        void OnDeleteMessageButtonClick(object sender, RoutedEventArgs e)
        {
   
        }
        
        void CreateNewMessage()
        {
             Message message = new Message(null, string.Empty, new DateTime(), App.Username, string.Empty, false)
            {
               FKEmployee_SenderUsername = allEmployees.FirstOrDefault(row => string.Compare(row.Username, App.Username) == 0),
               EDRecipient_MessageId = new List<Recipient>()
            };
             CreateMessageCreatorWindow(message);
        }

        void CreateReplyMessage()
        {
            Message selectedMessage = (Message)MessageList.SelectedItem;
            string newTitle = PrepareReplyMssageTitle(selectedMessage.Title);
            Employee senderEmployee = allEmployees.FirstOrDefault(row => string.Compare(row.Username, App.Username) == 0);
            List<Recipient> recipients = new List<Recipient>();
            recipients.Add(new Recipient(selectedMessage.SenderUsername, null, false, false));
            Message message = new Message(null, newTitle, new DateTime(), App.Username, string.Empty, false)
            {
                EDRecipient_MessageId = recipients,
                FKEmployee_SenderUsername = senderEmployee
            };
            CreateMessageCreatorWindow(message);
        }

        void CreateMessageCreatorWindow(Message message)
        {
            MessageCreator messageCreator = new MessageCreator();
            messageCreator.DataContext = new MessageCreatorModel()
            {
                AllEmployees = allEmployees,
                Message = message
            };
            messageCreator.Title = Properties.Resources.MessageCreatorTitle; ///? тоже в дата контекст ?
            messageCreator.Show();
        }

        string PrepareReplyMssageTitle(string title)
        {
            return Properties.Resources.Re
                + SpecialSymbols.SpecialSymbols.space
                + SpecialSymbols.SpecialSymbols.leftTitleStopper
                + title
                + SpecialSymbols.SpecialSymbols.rightTitleStopper;
        }

        #endregion
    }
}
