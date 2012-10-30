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
    /// Interaction logic for MessageCreator.xaml
    /// </summary>
    public partial class MessageCreator : Window
    {
        string recipientTextboxText;
        string titleTextboxText;
        string senderTextboxText;
        List<Employee> RecipientsEmployees = new List<Employee>();
        List<Employee> AllEmployees = new List<Employee>();

        public MessageCreator(string senderTextboxText, string recipientTextboxText, string titleTextboxText)
        {
            this.senderTextboxText = senderTextboxText;
            this.recipientTextboxText = recipientTextboxText;
            this.titleTextboxText = titleTextboxText;
            InitializeComponent();
            PrepareWindow();
            PrepareRecipientCombobox();
        }

        private void PrepareRecipientCombobox()
        {
            AllEmployees = App.Proxy.GetEmployeeList();
            MessageControl1.RecipientCombobox.ItemsSource = AllEmployees;
        }

        private void PrepareWindow()
        {
            MessageControl1.SenderTextbox.Text = senderTextboxText;
            MessageControl1.RecipientTextbox.IsReadOnly = false;
            MessageControl1.RecipientTextbox.Text = recipientTextboxText;
            MessageControl1.DateTextbox.Text = DateTime.Now.ToString();
            MessageControl1.TitleTextbox.IsReadOnly = false;
            MessageControl1.TitleTextbox.Text = titleTextboxText;
            MessageControl1.MessageContent.IsReadOnly = false;
            MessageControl1.RecipientCombobox.SelectionChanged += new SelectionChangedEventHandler(OnRecipientComboboxSelectionChanged);
            MessageControl1.RecipientTextbox.LostFocus +=new RoutedEventHandler(OnRecipientTextboxLostFocus);
        }

        private void SendMessageClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("aaaa");
        }

        public void OnRecipientComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employee selectedEmployee = (Employee)MessageControl1.RecipientCombobox.SelectedItem;
            if (string.Compare(MessageControl1.RecipientTextbox.Text, string.Empty) != 0)
                MessageControl1.RecipientTextbox.Text += ";";
            MessageControl1.RecipientTextbox.Text +=
                selectedEmployee.FirstName
                + " "
                + selectedEmployee.SecondName
                + " <"
                + selectedEmployee.Username
                + ">";
        }

        private void OnRecipientTextboxLostFocus(object sender, RoutedEventArgs e)
        {
            ///Имя Фамилия <username>
            ///Имя Фамилия
            ///username
            ///Проверить данные поля получателя
            string[] recipientsStringArray = MessageControl1.RecipientTextbox.Text.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            RecipientsEmployees.Clear();
            foreach(var recipientString in recipientsStringArray)
            {
                bool searhedByFirsrtname = false,
                    searchedBySecondName = false,
                    searchedByUsername = false;
                List<Employee> intersectionEmplyees = null;                
                string[] recipientFields = recipientString.Split(new char[3] { '<','>',' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var field in recipientFields)
                {
                    ////////////////////////////////////////
                    List<Employee> buf = new List<Employee>();
                    if (!searhedByFirsrtname)
                    {
                        searhedByFirsrtname = SearchEmployeesByFirstName(field, buf);
                        if (!searchedBySecondName & searhedByFirsrtname)
                        {
                            searchedBySecondName = SearchEmployeesBySecondName(field, buf);
                            if (!searchedByUsername)
                            {
                                searchedByUsername = SearchEmployeesByUsername(field, buf);
                            }
                        }
                    }              
                    if (intersectionEmplyees == null)                    
                        intersectionEmplyees = buf;                    
                    else
                        intersectionEmplyees = CalcEmployeesIntersection(intersectionEmplyees, buf);
                    if (intersectionEmplyees.Count == 0) 
                        break;
                }
                if (intersectionEmplyees.Count == 1)
                {
                    RecipientsEmployees.AddRange(intersectionEmplyees);
                    SendMessageButton.IsEnabled = true;
                    MessageControl1.RecipientTextbox.Text = CreateResipientString();

                }
                else
                {
                    ///ошибка
                    SendMessageButton.IsEnabled = false;
                    ///привязвть к валидации
                }
            }        
        }
        
        /////////////////////////////////////////////////////////////////////////что записать в результат?
        private void RemoveEqualEmployees(List<Employee> Employees)
        {
            for (int i = 0; i < Employees.Count; i++)
            {
                ///ищем только вниз, чтобы убрать уже проверенные элементы
                for (int j = i + 1; j < Employees.Count; )
                {
                    if (string.Compare(Employees[i].FirstName, Employees[j].FirstName) == 0
                        && string.Compare(Employees[i].SecondName, Employees[j].SecondName) == 0
                        && string.Compare(Employees[i].Username, Employees[j].Username) == 0)
                        Employees.RemoveAt(j);
                    else
                        j++;
                }
            }
        }

        private string CreateResipientString()
        {
            string recipientsString = string.Empty;
            foreach(var item in RecipientsEmployees)
                recipientsString +=
                    item.FirstName + " "
                    + item.SecondName + " <"
                    + item.Username + ">;";
            return recipientsString.Substring(0, recipientsString.Length - 1);
        }

        private List<Employee> CalcEmployeesIntersection(List<Employee> employeeSet1, List<Employee> employeeSet2)
        {
            List<Employee> result = new List<Employee>();
            foreach(var item1 in employeeSet1)
            {
                foreach (var item2 in employeeSet2)
                {
                    if( string.Compare(item1.FirstName, item2.FirstName) == 0
                        && string.Compare(item1.SecondName, item2.SecondName) == 0
                        && string.Compare(item1.Username, item2.Username) == 0)
                        result.Add(item1);                    
                }
            }
            return result;
        }

        private bool SearchEmployeesByFirstName(string firstName, List<Employee> FoundEmployees)
        {
            var queryResult = AllEmployees.Where(i => (string.Compare(i.FirstName, firstName) == 0)).ToList();
            if (queryResult != null)
            {
                FoundEmployees.AddRange(queryResult);
                return (queryResult.Count == 0) ? false : true;
            }
            return false;     
        }

        private bool SearchEmployeesBySecondName(string secondName, List<Employee> FoundEmployees)
        {
            var queryResult = AllEmployees.Where(i => (string.Compare(i.SecondName, secondName) == 0)).ToList();
            if (queryResult != null)
            {
                FoundEmployees.AddRange(queryResult);
                return (queryResult.Count == 0) ? false : true;
            }
            return false;
        }

        private bool SearchEmployeesByUsername(string username, List<Employee> FoundEmployees)
        {
            var queryResult = AllEmployees.Where(i => (string.Compare(i.Username, username) == 0)).ToList();
            if (queryResult != null)
            {
                FoundEmployees.AddRange(queryResult);
                return (queryResult.Count == 0) ? false : true;
            }
            return false;
        }


    }
}
