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

        void SetHandlers()
        {
            ToolbarControl1.CreateMessageButton.Click += new RoutedEventHandler(OnCreateMessageButtonClick);
            ToolbarControl1.ReplyMessageButton.Click += new RoutedEventHandler(OnReplyMessageButtonClick);
        }

        private void PrepareWindow()
        {            
            SetSidebar();
            SetHandlers();     
            PrepareMessageContrl1();
        }

        void PrepareMessageContrl1()
        {
       
            MessageList.Visibility = System.Windows.Visibility.Collapsed;
            HideMessageControl();
        }

        private void SetSidebar()
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
                    + recipientEmployee.Username + ">, ");
            }
            ///Удаляет последнюю запятую
            return recipientsString.Substring(0, recipientsString.Length - 2);      
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
            MessageControl1Column.Width = new GridLength(1, GridUnitType.Star);
            var selectedMessage = (Message)MessageList.SelectedItem;
            if (selectedMessage == null) return;
            MessageControl1.SenderTextbox.Text = selectedMessage.SenderUsername;
            MessageControl1.DateTextbox.Text = selectedMessage.Date.ToString();
            MessageControl1.TitleTextbox.Text = selectedMessage.Title;
            MessageControl1.MessageContent.Text = selectedMessage.Content;
            MessageControl1.RecipientTextbox.Text = GetRecipientsString();
            MessageControl1.Visibility = System.Windows.Visibility.Visible;
            ToolbarControl1.ReplyMessageButton.Visibility = System.Windows.Visibility.Visible;
            ToolbarControl1.DeleteMessageButton.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Общий обработчик для нажатий на папки в Sidebar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnFolderClick(object sender, RoutedEventArgs e)
        {
            var selecteFolder = (Button)sender;
            ///Папки с одинаковыми названиями будут отображать одинаковое содержимое 
            if (string.Compare(selecteFolder.Name, Properties.Resources.DeletedFolderLable) == 0)
                OnDeletedFolderClick();
            else if (string.Compare(selecteFolder.Name, Properties.Resources.InboxFolderLable) == 0)
                OnInboxFolderClick();
            else if (string.Compare(selecteFolder.Name, Properties.Resources.SentFolderLable) == 0)
                OnSentFolderClick();
            else UserFolderClick();
        }

        /// <summary>
        /// Обработка нажатия на пользовательскую папку
        /// </summary>
        private void UserFolderClick()
        {
            MessageList.Visibility = System.Windows.Visibility.Visible;
            HideMessageControl();
            throw new NotImplementedException();
        }

        private void OnDeletedFolderClick()
        {
            MessageList.ItemsSource = App.Proxy.GetDeletedFolder();
            HideMessageControl();
            MessageList.Visibility = System.Windows.Visibility.Visible;
        }

        private void OnSentFolderClick()
        {
            MessageList.ItemsSource = App.Proxy.GetSentFolder();
            HideMessageControl();
            MessageList.Visibility = System.Windows.Visibility.Visible;
        }

        private void OnInboxFolderClick()
        {
            MessageList.ItemsSource = App.Proxy.GetInboxFolder();
            HideMessageControl();
            MessageList.Visibility = System.Windows.Visibility.Visible;
        }

        private void HideMessageControl()
        {
            MessageControl1Column.Width = GridLength.Auto;
            MessageControl1.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void OnCreateMessageButtonClick(object sender, RoutedEventArgs e) 
        {
            MessageCreator newMessage = new MessageCreator();
            newMessage.Title = Properties.Resources.MessageCreatorTitle;
            newMessage.Show();
        }

        public void OnReplyMessageButtonClick(object sender, RoutedEventArgs e)
        {
            MessageCreator newMessage = new MessageCreator();
            newMessage.Title = Properties.Resources.MessageCreatorTitle;
            newMessage.Show();
        }
    }
}
