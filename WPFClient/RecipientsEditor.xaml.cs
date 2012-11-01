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
using Entities;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for RecipientsEditor.xaml
    /// </summary>
    public partial class RecipientsEditor : Window
    {
        public RecipientsEditor()
        {
            InitializeComponent();
            var a = new List<Employee>();
            a.Add(new Employee("u1", "f1", "s1", "", ""));
            a.Add(new Employee("u2", "f2", "s2", "", ""));
            a.Add(new Employee("u3", "f3", "s3", "", ""));
            ctrl.AllEmployeesList = a;
        }
    }
}
