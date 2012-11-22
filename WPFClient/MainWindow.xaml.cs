﻿using System;
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
            List<Message> InboxMessages = App.Proxy.GetInboxMessages();
            FillMessages(InboxMessages);
            MessageList.ItemsSource = InboxMessages;

        }

        void PrepareMessageListForSentboxFolder()
        {
            //inboxFolderPressed = false;
            //MessageList.ItemTemplate = (DataTemplate)FindResource("DefaultFolderTemplate");
            //List<Message> SentboxMessages = App.Proxy.GetSentboxMessages();
            //List<MessageModel> SentboxMessagesModel = new List<MessageModel>();
            //foreach (var item in SentboxMessages)
            //{
            //    SentboxMessagesModel.Add(new MessageModel()
            //    {
            //        Message = item,
            //        SenderEmployee = App.Proxy.GetEmployee(item.SenderUsername),
            //        Recipients = App.Proxy.GetRecipients((int)item.Id)
            //    });
            //}
            //MessageList.ItemsSource = SentboxMessagesModel;
        }

        void PrepareMessageListForDeletedFolder()
        {
            //inboxFolderPressed = false;
            //MessageList.ItemTemplate = (DataTemplate)FindResource("DefaultFolderTemplate");
            //List<Message> DeletedMessages = App.Proxy.GetDeletedMessages();
            //List<MessageModel> DeletedMessagesModel = new List<MessageModel>();
            //foreach (var item in DeletedMessages)
            //{
            //    DeletedMessagesModel.Add(new MessageModel()
            //    {
            //        Message = item,
            //        SenderEmployee = App.Proxy.GetEmployee(item.SenderUsername),
            //        Recipients = App.Proxy.GetRecipients((int)item.Id)
            //    });
            //}
            //MessageList.ItemsSource = DeletedMessagesModel;
        }

        /// <summary>
        /// Обработка нажатия на пользовательскую папку
        /// </summary>
        void OnUserFolderClick()
        {
            MessageBox.Show("Не нажимать!");
        }

        public void OnCreateMessageButtonClick(object sender, RoutedEventArgs e) 
        {
            Message message = new Message(null, string.Empty, new DateTime(), App.Username, string.Empty, false)
            {
               FKEmployee_SenderUsername = allEmployees.FirstOrDefault(row => string.Compare(row.Username, App.Username) == 0),
               EDRecipient_MessageId = new List<Recipient>()
            };
            MessageCreator messageCreator = new MessageCreator();
            messageCreator.DataContext = new MessageCreatorModel()
            {                
                AllEmployees = allEmployees,
                Message = message
            };
            messageCreator.Title = Properties.Resources.MessageCreatorTitle; ///? тоже в дата контекст ?
            messageCreator.Show();            
        }

        public void OnReplyMessageButtonClick(object sender, RoutedEventArgs e)
        {
            //MessageModel selectedMessage = (MessageModel)MessageList.SelectedItem;            
            //string titleString = Properties.Resources.Re
            //    + SpecialSymbols.SpecialSymbols.space
            //    + SpecialSymbols.SpecialSymbols.leftTitleStopper
            //    + selectedMessage.Message.Title
            //    + SpecialSymbols.SpecialSymbols.rightTitleStopper;
            //List<Employee> allEmployees = App.Proxy.GetAllEmployees();
            //Message message = new Message(null, titleString, new DateTime(), App.Username, string.Empty, false);
            //Employee senderEmployee = allEmployees.FirstOrDefault(row => string.Compare(row.Username, message.SenderUsername) == 0);
            //List<Recipient> recipients = new List<Recipient>();
            //recipients.Add(new Recipient(selectedMessage.Message.SenderUsername, null, false, false));            
            //MessageModel messageModel = new MessageModel()
            //{
            //    Message = message,
            //    SenderEmployee = senderEmployee,
            //    Recipients = recipients
            //};
            //MessageCreator messageCreator = new MessageCreator();
            //messageCreator.DataContext = new MessageCreatorModel()
            //{
            //    AllEmployees = allEmployees,
            //    MessageModel = messageModel
            //};
            //messageCreator.Title = Properties.Resources.MessageCreatorTitle; ///? тоже в дата контекст
            //messageCreator.Show();
        }

        public void OnDeleteMessageButtonClick(object sender, RoutedEventArgs e)
        {
   
        }

        void FillMessages(List<Message> messages)
        {
            foreach (var item in messages)
            {
                item.FKEmployee_SenderUsername = App.Proxy.GetEmployee(item.SenderUsername);
                item.EDRecipient_MessageId = App.Proxy.GetRecipients((int)item.Id);
            }
        }

    }
}
