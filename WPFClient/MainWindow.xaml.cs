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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Loguot_Click(object sender, RoutedEventArgs e)
        {
            App.Proxy = null;
            App.Username = null;
            App.Password = null;            
            App.Current.MainWindow.Show();
            this.Close();
        }

        private void InboxFolder_Selected(object sender, RoutedEventArgs e)
        {
           
            MessageList.ItemsSource = App.Proxy.GetInboxFolder();    
        }

        private void SentFolder_Selected(object sender, RoutedEventArgs e)
        {
           
            MessageList.ItemsSource = App.Proxy.GetSentFolder();
        }

        private void DeletedFolder_Selected(object sender, RoutedEventArgs e)
        {
          
            MessageList.ItemsSource = App.Proxy.GetDeletedFolder();
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
            RecipientTextbox.Text = SenderTextbox.Text;
            SenderTextbox.Text = "Me: <" + App.Username + ">";
            DateTextbox.Text = DateTime.Now.ToString();
            TitleTextbox.IsReadOnly = false;
            MessageContent.IsReadOnly = false;
            TitleTextbox.Text = "re: [" + TitleTextbox.Text + "]";
            MessageContent.Text = "Введите сообщение";
            SendButton.Visibility = System.Windows.Visibility.Visible;
            DeleteButton.Visibility = System.Windows.Visibility.Collapsed;
            ReplyButton.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void MessageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedMessage = (Message)MessageList.SelectedItem;
            if (selectedMessage == null) return;
            SenderTextbox.Text = selectedMessage.SenderUsername;
            DateTextbox.Text = selectedMessage.Date.ToString();
            TitleTextbox.Text = selectedMessage.Title;
            MessageContent.Text = selectedMessage.Content;
            RecipientTextbox.Text = GetRecipientsString(); 
        }
    }
}
