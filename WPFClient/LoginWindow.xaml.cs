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
            UsernameTexbox.Text = "Ivan1";
            PasswordTexbox.Password = "111";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.ServiceWatcher = new ServiceWatcher(App.timeBetweenUpdating)
                {
                    FactoryUsername = UsernameTexbox.Text,
                    FactoryPassword = PasswordTexbox.Password
                };

                ///quick login
                App.ServiceWatcher.FactoryUsername = "Ivan1";
                App.ServiceWatcher.FactoryPassword = "111";
                ///

                App.ServiceWatcher.CreateChannel();
                App.ServiceWatcher.CheckUser();
                this.DialogResult = true;                           
                this.Close();
            }
            catch (System.ServiceModel.Security.MessageSecurityException ex)
            {
                LoginFiled.Show("Authentication filed!");
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
