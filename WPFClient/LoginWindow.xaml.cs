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
using InformatonTips;
using System.ComponentModel;


namespace WPFClient
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            Closing += new CancelEventHandler(OnLoginWindowClosing);
            InitializeComponent();
            this.Title = Properties.Resources.Login;
            App.Username = "Ivan1";
            App.Password = "111";
            UsernameTexbox.Text = App.Username;
            PasswordTexbox.Password = App.Password;
        }

        void OnLoginWindowClosing(object sender, CancelEventArgs e)
        {
            CheckLoginResult();
        }

        void CheckLoginResult()
        {
            if (App.Proxy == null)
                App.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ///quick login
                App.Username = "Ivan1";
                App.Password = "111";
                UsernameTexbox.Text = App.Username;
                PasswordTexbox.Password = App.Password;
                var factory1 = new ChannelFactory<IService1>("*");
                factory1.Credentials.UserName.UserName = UsernameTexbox.Text;
                factory1.Credentials.UserName.Password = PasswordTexbox.Password;
                App.Proxy = factory1.CreateChannel();
                App.Proxy.CheckUser();
                App.Current.MainWindow.Show();               
                this.Close();
            }
            catch (System.ServiceModel.Security.MessageSecurityException ex1)
            {
                LoginFiled.Show("Authentication filed!");
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex1,"private void Button_Click(object sender, RoutedEventArgs e)");
            }
            catch (Exception ex2)
            {
                LoginFiled.Show(ex2.Message);
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex2, "private void Button_Click(object sender, RoutedEventArgs e)");
            }
        }
    }
}
