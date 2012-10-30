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
            if (CheckRecipientTextbox(out errorMessage))
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

        private bool CheckRecipientTextbox(out string errorMessage )
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
                if (!CheckRecipientString(recipientString, out errorMessage))
                    return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        private bool CheckRecipientString(string recipientString, out string errorMessage)
        {
            string[] recipientFields = recipientString.Split(new char[1] {' '}, StringSplitOptions.RemoveEmptyEntries);
            switch (recipientFields.Length)
            {
                case 1:
                    return ProcessOneWordRecipientString(recipientString, out errorMessage);                    
                case 3:
                    return ProcessThreeWordRecipientString(recipientString, out errorMessage);
                default:
                        errorMessage = Properties.Resources.InvalidRecipientString + recipientString;
                        return false;
            }
        }

        private bool ProcessOneWordRecipientString(string recipientString, out string errorMessage)
        {
            string[] substrings = recipientString.Split(new char[2] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
            recipientString = substrings[0];
            Employee foundEmployee = AllEmployees.FirstOrDefault(i => (string.Compare(i.Username, recipientString) == 0));
            if (foundEmployee != null)
            {
                RecipientsEmployees.Add(foundEmployee);
                errorMessage = null;
                return true;
            }
            else
            {
                errorMessage = recipientString + " " + Properties.Resources.NotFound;
                return false;
            }
        }

        private bool ProcessThreeWordRecipientString(string recipientString, out string errorMessage)
        {
            string username;
            if (ParseUsername(recipientString, out username))
            {
                Employee foundEmployee = AllEmployees.FirstOrDefault(i => (string.Compare(i.Username, username) == 0));
                if (foundEmployee != null)
                {
                    RecipientsEmployees.Add(foundEmployee);
                    errorMessage = null;
                    return true;
                }
                else
                {
                    errorMessage = username + " " + Properties.Resources.NotFound;
                    return false;
                }
            }
            else
            {
                errorMessage = Properties.Resources.InvalidRecipientString + recipientString;
                return false;
            }
        }
        
        private string EmployeeToString(Employee employee)
        {
            return employee.FirstName + " "
                    + employee.SecondName + " <"
                    + employee.Username + ">";           
        }

        private bool ParseUsername(string recipientString , out string username)
        {
            int begin = recipientString.IndexOf('<'),
                            end = recipientString.IndexOf('>');
            if (begin == -1 || end == -1)
            {
                username = null;
                return false;
            }
            begin += 1;
            username = recipientString.Substring(begin, end - begin);
            return true;
        }
    }
}
