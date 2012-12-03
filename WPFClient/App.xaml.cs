using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using DBService;
using WPFClient.Additional;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {  
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public static string Username
        { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public static string Password
        { get; set; }

        public static LoginWindow LW;

        public static MainWindow MW;

        public static ServiceWatcher ServiceWatcher {get; set;}  

        public static TimeSpan timeBetweenUpdating = new TimeSpan(0, 0, 20);

        public static TimeSpan timePerMessageIsViewed = new TimeSpan(0, 0, 10); 
    
    }
}
