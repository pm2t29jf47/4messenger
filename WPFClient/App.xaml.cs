using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using DBService;
using WPFClient.Additional;
using System.ServiceModel;
using ServiceInterface;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static TimeSpan timeBetweenUpdating = new TimeSpan(0, 0, 10);

        public static TimeSpan timePerMessageSetViewed = new TimeSpan(0, 0, 5);

        public static ChannelFactory<IService1> factory = new ChannelFactory<IService1>("*");

        public static IService1 proxy;

        public static MainWindow mainWindow;
    }
}
