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

        private void PrepareWindow()
        {
            FillFoldersNames();
            SetSidebar();
        }

        private void SetSidebar()
        {
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
            //MessageControl1.RecipientTextbox.Text = MessageControl1.SenderTextbox.Text;
            //MessageControl1.SenderTextbox.Text = "Me: <" + App.Username + ">";
            //MessageControl1.DateTextbox.Text = DateTime.Now.ToString();
            //MessageControl1.TitleTextbox.IsReadOnly = false;
            //MessageControl1.MessageContent.IsReadOnly = false;
            //MessageControl1.TitleTextbox.Text = "re: [" + MessageControl1.TitleTextbox.Text + "]";
            //MessageControl1.MessageContent.Text = "Введите сообщение";
            //MessageControl1.SendButton.Visibility = System.Windows.Visibility.Visible;
            //MessageControl1.DeleteButton.Visibility = System.Windows.Visibility.Collapsed;
            //MessageControl1.ReplyButton.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void MessageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var selectedMessage = (Message)MessageList.SelectedItem;
            //if (selectedMessage == null) return;
            //MessageControl1.SenderTextbox.Text = selectedMessage.SenderUsername;
            //MessageControl1.DateTextbox.Text = selectedMessage.Date.ToString();
            //MessageControl1.TitleTextbox.Text = selectedMessage.Title;
            //MessageControl1.MessageContent.Text = selectedMessage.Content;
            //MessageControl1.RecipientTextbox.Text = GetRecipientsString();
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
            throw new NotImplementedException();
        }

        private void OnDeletedFolderClick()
        {
            MessageList.ItemsSource = App.Proxy.GetDeletedFolder();
        }

        private void OnSentFolderClick()
        {
            MessageList.ItemsSource = App.Proxy.GetSentFolder();
        }

        private void OnInboxFolderClick()
        {
            MessageList.ItemsSource = App.Proxy.GetInboxFolder();
        }
    }
}
