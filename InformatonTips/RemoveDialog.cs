using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace InformatonTips
{
    public static class RemoveDialog
    {
        public static bool Show(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }
    }
}
