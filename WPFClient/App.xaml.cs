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
        public static LoginWindow LW;

        public static MainWindow MW;

        public static ServiceWatcher ServiceWatcher {get; set;}  

        public static TimeSpan timeBetweenUpdating = new TimeSpan(0, 0, 5);

        public static TimeSpan timePerMessageSetViewed = new TimeSpan(0, 0, 3);

    }
}
