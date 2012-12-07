using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InformatonTips
{
    public static class SomeError
    {
        public static void Show(string ErrorMessage)
        {
            MessageBox.Show(ErrorMessage);
        }
    }
}
