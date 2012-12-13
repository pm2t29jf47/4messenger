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

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for ConnectionErrorDetails.xaml
    /// </summary>
    public partial class ConnectionErrorDetails : Window
    {
        public ConnectionErrorDetails()
        {
            InitializeComponent();            
        }

        private void OnReconnectButtonClick(object sender, RoutedEventArgs e)
        {
            SetDialogState(true);
            CloseWindow();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            SetDialogState(false);
            CloseWindow();
        }

        void SetDialogState(bool state)
        {
            this.DialogResult = state;
        }

        void CloseWindow()
        {
            this.Close();
        }
    }
}
