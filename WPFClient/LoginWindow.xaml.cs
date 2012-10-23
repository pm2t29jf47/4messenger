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
using System.IdentityModel.Tokens;

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
            Button_Click(null, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ///quick login
                UsernameTexbox.Text = "Ivan1";
                PasswordTexbox.Password = "111";
                var factory1 = new ChannelFactory<IService1>("*");
                factory1.Credentials.UserName.UserName = UsernameTexbox.Text;
                factory1.Credentials.UserName.Password = PasswordTexbox.Password;
                App.Proxy = factory1.CreateChannel();
                App.Proxy.CheckUser();
                var mw = new MainWindow();
                mw.Show();
                ///После this.Close(); поидее должно закрываться и mw
                this.Close();
            }
            catch (System.ServiceModel.Security.MessageSecurityException)
            {
                MessageBox.Show("Authentication filed!");
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message);
            }
        }
    }
}
