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

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MessageCreator.xaml
    /// </summary>
    public partial class MessageCreator : Window
    {
        string recipientTextboxText;
        string titleTextboxText;
        string senderTextboxText;

        public MessageCreator(string senderTextboxText, string recipientTextboxText, string titleTextboxText)
        {
            this.senderTextboxText = senderTextboxText;
            this.recipientTextboxText = recipientTextboxText;
            this.titleTextboxText = titleTextboxText;
            InitializeComponent();
            PrepareWindow();
        }

        private void PrepareWindow()
        {
            MessageControl1.SenderTextbox.Text = senderTextboxText;
            MessageControl1.RecipientTextbox.IsReadOnly = false;
            MessageControl1.RecipientTextbox.Text = recipientTextboxText;
            MessageControl1.DateTextbox.Text = DateTime.Now.ToString();
            MessageControl1.TitleTextbox.IsReadOnly = false;
            MessageControl1.TitleTextbox.Text = titleTextboxText;
            MessageControl1.MessageContent.IsReadOnly = false;
        }

        private void SendMessageClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("aaaa");
            
        }

        ///Проверять введенных адресатов
    }
}
