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
using WPFClient.Additional;
using System.ServiceModel.Channels;
using ServiceInterface;


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
            this.Title = Properties.Resources.Login;
            UsernameTexbox.Text = "ivan1";
            PasswordTexbox.Password = "111";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (App.factory != null)
                {
                    App.factory.Close();
                }
                App.factory = new ChannelFactory<IService1>("*");
                App.factory.Credentials.UserName.UserName = UsernameTexbox.Text;
                App.factory.Credentials.UserName.Password = PasswordTexbox.Password;
                App.proxy =  App.factory.CreateChannel();
                App.proxy.CheckUser();        
                this.DialogResult = true;                           
                this.Close();
            }
            catch (System.ServiceModel.Security.MessageSecurityException ex)
            {
                LoginFiled.Show("Authentication filed!");
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.LoginWindow.Button_Click(object sender, RoutedEventArgs e)");
            }
                /// Сервис не отвечает
            catch (EndpointNotFoundException ex)
            {
                LoginFiled.Show(ex.Message);
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.LoginWindow.Button_Click(object sender, RoutedEventArgs e)");
            }
            catch (Exception ex)
            {
                LoginFiled.Show(ex.Message);
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.LoginWindow.Button_Click(object sender, RoutedEventArgs e)");
                throw;
            }
        }

    }
}
