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
            SendMessageButton.IsEnabled = false;
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
            MessageControl1.RecipientTextbox.Text += EmployeeToString(selectedEmployee);
        }

        private void OnRecipientTextboxLostFocus(object sender, RoutedEventArgs e)
        {
            string errorMessage;
            if (CheckRecipientString(out errorMessage))
            {
                MessageControl1.RecipientTextbox.Text = string.Empty;
                foreach (var employee in RecipientsEmployees)
                    MessageControl1.RecipientTextbox.Text += EmployeeToString(employee) + ";";
                MessageControl1.RecipientTextbox.Text = MessageControl1.RecipientTextbox.Text.Substring(0, MessageControl1.RecipientTextbox.Text.Length - 1);
                SendMessageButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show(errorMessage);
                SendMessageButton.IsEnabled = false;
            }
        }

        private bool CheckRecipientString(out string errorMessage )
        {
            RecipientsEmployees.Clear();
            string[] recipientsStringArray = MessageControl1.RecipientTextbox.Text.Split(new char[1] {';'}, StringSplitOptions.RemoveEmptyEntries);
            if (recipientsStringArray.Length == 0)
            {
                errorMessage = Properties.Resources.RecipientsStringNotBeEmpty;
                return false;
            }
            foreach (var recipientString in recipientsStringArray)
            {
                string[] recipientFields = recipientString.Split(new char[1]{' '}, StringSplitOptions.RemoveEmptyEntries);
                switch (recipientFields.Length)
                {
                    case 1:
                        {
                            Employee foundEmployee = SearchEmployeesByUsername(recipientString, out errorMessage);
                            if (foundEmployee != null)
                                RecipientsEmployees.Add(foundEmployee);
                            else
                                return false;
                            break;
                        }
                    case 3:
                        {
                            string username;
                            if (ContainUsername(recipientString, out errorMessage, out username))
                            {
                                Employee foundEmployee = SearchEmployeesByUsername(username, out errorMessage);
                                if (foundEmployee != null)
                                    RecipientsEmployees.Add(foundEmployee);
                                else
                                    return false;
                            }
                            else
                                return false;
                            break;
                        }
                    default:
                        {
                            errorMessage = Properties.Resources.InvalidRecipientString + recipientString;
                            break;
                        }
                }
            }
            errorMessage = string.Empty;
            return true;
        }
        
        private string EmployeeToString(Employee employee)
        {
            return employee.FirstName + " "
                    + employee.SecondName + " <"
                    + employee.Username + ">";           
        }

        private Employee SearchEmployeesByUsername(string username, out string errorMessage)
        {
            var FoundEmployee = AllEmployees.FirstOrDefault(i => (string.Compare(i.Username, username) == 0));
            if (FoundEmployee == null)     
                errorMessage = username + " " + Properties.Resources.NotFound;  
            else
                errorMessage = null;
            return FoundEmployee;            
        }

        private bool ContainUsername(string recipientString ,out string errorMessage, out string username)
        {
            int begin = recipientString.IndexOf('<'),
                            end = recipientString.IndexOf('>');
            if (begin == -1 || end == -1)
            {
                errorMessage = Properties.Resources.InvalidRecipientString + recipientString;
                username = null;
                return false;
            }
            begin += 1;
            username = recipientString.Substring(begin, end - begin);
            errorMessage = null;
            return true;
        }
    }
}
