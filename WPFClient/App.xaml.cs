using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using DBService;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Хранит прокси класс для обращения к методам сервиса
        /// </summary>
        public static IService1 Proxy
        { get; set; }

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
    }
}
