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
            FormLoad();
        }

        private void FormLoad()
        {
            var receivedMessages = App.Proxy.ReceiveMessages();
            MessageList.Items.Add(receivedMessages[0]);
            MessageList.Items.Add(receivedMessages[1]);
            //MessageList.
            

        }

        private void NewLatter_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Loguot_Click(object sender, RoutedEventArgs e)
        {
            App.Proxy = null;
            var lo = new LoginWindow();
            lo.Show();
            this.Close();
        }

        private void SentFolder2_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void MessageList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int A = 12;

        }
    }
}
