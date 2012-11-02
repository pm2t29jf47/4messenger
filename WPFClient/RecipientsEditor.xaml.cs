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
using System.Collections.ObjectModel;

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
            a.Add(new Employee("Admin", "Admin", "Админ", "", ""));
            a.Add(new Employee("u2", "f2", "s2", "", ""));
            a.Add(new Employee("u3", "f3", "s3", "", ""));
            var b = new ObservableCollection<Employee>();
            foreach (var item in a)
                b.Add(item);

            ctrl.AllEmployeesList = b;
            var c = new List<Employee>();
            c.Add(new Employee("Admin", "Admin", "Админ", "", ""));
            c.Add(new Employee("u2", "f2", "s2", "", ""));
     
            var d = new ObservableCollection<Employee>();
            foreach (var item in c)
                d.Add(item);
            
            ctrl.SelectedEmployeesList = d;
           
        }
    }
}
