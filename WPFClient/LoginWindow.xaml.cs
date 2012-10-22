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
using System.ServiceModel;
using DBService;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var factory1 = new ChannelFactory<IService1>("*");
            factory1.Credentials.UserName.UserName = "Admin";
            factory1.Credentials.UserName.Password = "22h2";
            var proxy = factory1.CreateChannel();
            var a = proxy.ReceiveMessages();

        }
    }
}
