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
            var lo = new LoginWindow();
            lo.Show();
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

        private void MessageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedMessage = (Message)MessageList.SelectedItem;
            SenderTextbox.Text = selectedMessage.SenderUsername;                       
            DateTextbox.Text = selectedMessage.Date.ToLongDateString();
            TitleTextbox.Text = selectedMessage.Title;
            MessageContent.Text = selectedMessage.Content;
            SetRecipientTextBox();
        }

        private void SetRecipientTextBox()
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
                    + recipientEmployee.Username + ">, " + "ooooooooooooooooooooooooooooooodddddddddddddddddddddddddddddddddddddddddddddddddddddddddffffffffffffffffffffffffffffffffffffffffffddddddddddddddddddddddddddddddddddddddfff"
                    + "ooooooooooooooooooooooooooooooodddddddddddddddddddddddddddddddddddddddddddddddddddddddddffffffffffffffffffffffffffffffffffffffffffddddddddddddddddddddddddddddddddddddddfff"
                    + "ooooooooooooooooooooooooooooooodddddddddddddddddddddddddddddddddddddddddddddddddddddddddffffffffffffffffffffffffffffffffffffffffffddddddddddddddddddddddddddddddddddddddfff"
                    + "ooooooooooooooooooooooooooooooodddddddddddddddddddddddddddddddddddddddddddddddddddddddddffffffffffffffffffffffffffffffffffffffffffddddddddddddddddddddddddddddddddddddddfff"
                    + "ooooooooooooooooooooooooooooooodddddddddddddddddddddddddddddddddddddddddddddddddddddddddffffffffffffffffffffffffffffffffffffffffffddddddddddddddddddddddddddddddddddddddfff"
                    + "ooooooooooooooooooooooooooooooodddddddddddddddddddddddddddddddddddddddddddddddddddddddddffffffffffffffffffffffffffffffffffffffffffddddddddddddddddddddddddddddddddddddddfff"
                    );
            }
            ///Удаляет последнюю запятую
            recipientsString = recipientsString.Substring(0, recipientsString.Length - 2);
            RecipientTextbox.Text = recipientsString;
        }
    }
}
