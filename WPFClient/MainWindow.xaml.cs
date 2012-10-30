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

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<SidebarFolder> folders = new List<SidebarFolder>();

        public MainWindow()
        {
            ///Выбирает локаль
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en");
            InitializeComponent();
            PrepareWindow();
            ShowLoginWindow(); 
        }

        private void PrepareWindow()
        {
            SetHandlers();
            PreareSidebar();
            PrepareMessageControl1();
            HideMessageControl1(true);
            HideToolbarControl1Buttons(true);
            HideMessageList(true);
        }

        void PrepareMessageControl1()
        {
            MessageControl1.RecipientCombobox.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void HideMessageList(bool state)
        {
            MessageList.Visibility = state ? Visibility.Collapsed : Visibility.Visible;
            
        }

        private void HideToolbarControl1Buttons(bool state)
        {
            ToolbarControl1.ReplyMessageButton.Visibility = state ? Visibility.Collapsed : Visibility.Visible;
            ToolbarControl1.DeleteMessageButton.Visibility = state ? Visibility.Collapsed : Visibility.Visible; 
        }

        private void HideMessageControl1(bool state)
        {
            if (state)
            {
                MessageControl1Column.Width = GridLength.Auto;
                MessageControl1.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                MessageControl1Column.Width = new GridLength(1, GridUnitType.Star);
                MessageControl1.Visibility = System.Windows.Visibility.Visible;                
            }

        }

        void SetHandlers()
        {
            ToolbarControl1.CreateMessageButton.Click += new RoutedEventHandler(OnCreateMessageButtonClick);
            ToolbarControl1.ReplyMessageButton.Click += new RoutedEventHandler(OnCreateMessageButtonClick);        
        }

        private void PreareSidebar()
        {
            FillFoldersNames();
            foreach (var folder in folders)
            {
                Button folderButton = new Button();
                StackPanel folderStackPanel = new StackPanel();
                Uri uri = new Uri("pack://application:,,,/Images/folder.png");
                BitmapImage source = new BitmapImage(uri);
                folderStackPanel.Children.Add(new Image { Source = source });
                folderStackPanel.Children.Add(new Label { Content = folder.FolderName });
                folderButton.Content = folderStackPanel;
                folderButton.Click += new RoutedEventHandler(OnFolderClick);
                folderButton.Name = folder.FolderName;
                Thickness a = new Thickness(5.0);
                folderButton.Margin = a;
                Sidebar.Children.Add(folderButton);
            }            
        }

        private void FillFoldersNames()
        {
            folders.Add(new SidebarFolder(Properties.Resources.InboxFolderLable));
            folders.Add(new SidebarFolder(Properties.Resources.SentFolderLable));
            folders.Add(new SidebarFolder(Properties.Resources.DeletedFolderLable));
        }

        private void ShowLoginWindow()
        {
            this.Hide();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        private string GetRecipientsString()
        {
            var selectedMessage = (Message)MessageList.SelectedItem;
            string recipientsString = string.Empty;
            foreach (var recipient in selectedMessage.Recipients)
            {
                var recipientEmployee = App.Proxy.GetEmployee(recipient.RecipientUsername);
                recipientsString +=
                    (recipientEmployee == null)
                    ?
                    string.Empty
                    :
                    (recipientEmployee.FirstName + " "
                    + recipientEmployee.SecondName + " <"
                    + recipientEmployee.Username + ">;");
            }
            ///Удаляет последнюю запятую
            return recipientsString.Substring(0, recipientsString.Length - 1);      
        }

        private void ReplyButton_Click(object sender, RoutedEventArgs e)
        {
            MessageControl1.RecipientTextbox.Text = MessageControl1.SenderTextbox.Text;
            MessageControl1.SenderTextbox.Text = "Me: <" + App.Username + ">";
            MessageControl1.DateTextbox.Text = DateTime.Now.ToString();
            MessageControl1.TitleTextbox.IsReadOnly = false;
            MessageControl1.MessageContent.IsReadOnly = false;
            MessageControl1.TitleTextbox.Text = "re: [" + MessageControl1.TitleTextbox.Text + "]";
            MessageControl1.MessageContent.Text = "Введите сообщение";
        }

        private void OnMessageListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var selectedMessage = (Message)MessageList.SelectedItem;
            if (selectedMessage == null) return;    ///Ложные срабатывания
            MessageControl1.SenderTextbox.Text = selectedMessage.SenderUsername;
            MessageControl1.DateTextbox.Text = selectedMessage.Date.ToString();
            MessageControl1.TitleTextbox.Text = selectedMessage.Title;
            MessageControl1.MessageContent.Text = selectedMessage.Content;
            MessageControl1.RecipientTextbox.Text = GetRecipientsString();
            HideToolbarControl1Buttons(false);
            HideMessageControl1(false);        
        }

        /// <summary>
        /// Общий обработчик для нажатий на папки в Sidebar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnFolderClick(object sender, RoutedEventArgs e)
        {
            var selecteFolder = (Button)sender;
            HideToolbarControl1Buttons(true);
            HideMessageControl1(true);
            HideMessageList(false);
            if (string.Compare(selecteFolder.Name, Properties.Resources.DeletedFolderLable) == 0)
                MessageList.ItemsSource = App.Proxy.GetDeletedFolder();   
            else if (string.Compare(selecteFolder.Name, Properties.Resources.InboxFolderLable) == 0)
                MessageList.ItemsSource = App.Proxy.GetInboxFolder();
            else if (string.Compare(selecteFolder.Name, Properties.Resources.SentFolderLable) == 0)
                MessageList.ItemsSource = App.Proxy.GetSentFolder();
            else OnUserFolderClick();
        }

        /// <summary>
        /// Обработка нажатия на пользовательскую папку
        /// </summary>
        private void OnUserFolderClick()
        {
            throw new NotImplementedException();
        }

        public void OnCreateMessageButtonClick(object sender, RoutedEventArgs e) 
        {
            MessageCreator newMessage = new MessageCreator("Me: <" + App.Username + ">", "", "");
            newMessage.Title = Properties.Resources.MessageCreatorTitle;
            newMessage.Show();            
        }

        public void OnReplyMessageButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedMessage = (Message)MessageList.SelectedItem;
            var recipientEmployee = App.Proxy.GetEmployee(selectedMessage.SenderUsername);
            string recipientString = recipientEmployee.FirstName + " "
                    + recipientEmployee.SecondName + " <"
                    + recipientEmployee.Username + ">, ";
            string senderString = "Me: <" + App.Username + ">";
            string titleString = "re: [" + selectedMessage.Title + "]";
            MessageCreator newMessage = new MessageCreator(senderString, recipientString, titleString);
            newMessage.Show();
        }
    }
}
