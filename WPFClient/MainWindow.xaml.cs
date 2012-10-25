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
            this.Hide();
            var lw = new LoginWindow();
            lw.Show();
            var a = Properties.Resources.ololo;
            
        }

        private void OnInboxFolderSelected(object sender, RoutedEventArgs e)
        {
           
            MessageList.ItemsSource = App.Proxy.GetInboxFolder();
            
        }

        private void OnSentFolderSelected(object sender, RoutedEventArgs e)
        {
           
            MessageList.ItemsSource = App.Proxy.GetSentFolder();
        }

        private void OnDeletedFolderSelected(object sender, RoutedEventArgs e)
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

        private void MessageControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
