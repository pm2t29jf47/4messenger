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
using System.Windows.Shapes;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MessageCreator.xaml
    /// </summary>
    public partial class MessageCreator : Window
    {
        string recipientUsername = string.Empty;
        string title = string.Empty;
        string senderUsername = string.Empty;

        public MessageCreator()
        {
            InitializeComponent();
            PrepareWindow();
        }

        public MessageCreator(string senderUsername, string recipientUsername, string title)
        {
            this.senderUsername = senderUsername;
            this.recipientUsername = recipientUsername;
            this.title = title;
            InitializeComponent();
            PrepareWindow();
        }

        private void PrepareWindow()
        {
            MessageControl1.SenderTextbox.Text = senderUsername;
            MessageControl1.RecipientTextbox.IsReadOnly = false;
            MessageControl1.RecipientTextbox.Text = recipientUsername;
            MessageControl1.DateTextbox.Text = DateTime.Now.ToString();
            MessageControl1.TitleTextbox.IsReadOnly = false;
            MessageControl1.TitleTextbox.Text = title;
            MessageControl1.MessageContent.IsReadOnly = false;
        }

        private void SendMessageClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("aaaa");
        }
    }
}