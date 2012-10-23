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
            MessageList.ItemsSource = App.Proxy.InboxMessages();    
        }

        private void SentFolder_Selected(object sender, RoutedEventArgs e)
        {
            MessageList.ItemsSource = App.Proxy.SentMessages();
        }

        private void DeletedFolder_Selected(object sender, RoutedEventArgs e)
        {
            MessageList.ItemsSource = App.Proxy.DeletedMessages();
        }

        private void MessageList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedMessage = (Message) MessageList.SelectedItem;
            SenderTextbox.Text = selectedMessage.SenderUsername;
            RecipientTextbox.Text = "gjkexfntkb";
            DateTextbox.Text = selectedMessage.Date.ToLongDateString();
            TitleTextbox.Text = selectedMessage.Title;
        }

     


    }
}
