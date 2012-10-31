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

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="senderTextboxText"></param>
        /// <param name="recipientTextboxText"></param>
        /// <param name="titleTextboxText"></param>
        public MessageCreator(string senderTextboxText, string recipientTextboxText, string titleTextboxText)
        {
            this.senderTextboxText = senderTextboxText;
            this.recipientTextboxText = recipientTextboxText;
            this.titleTextboxText = titleTextboxText;
            InitializeComponent();
            PrepareWindow();
            PrepareRecipientCombobox();
        }

        /// <summary>
        /// Подготавливает combobox с получателями
        /// </summary>
        private void PrepareRecipientCombobox()
        {
            AllEmployees = App.Proxy.GetEmployeeList();
            MessageControl1.RecipientCombobox.ItemsSource = AllEmployees;
        }

        /// <summary>
        /// Подготавливает все окно
        /// </summary>
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
            MessageControl1.RecipientTextbox.TextChanged += new TextChangedEventHandler(OnRecipientTextboxTextChanged);
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Send"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSendMessageButtonClick(object sender, RoutedEventArgs e)
        {
            string errorMessage;
            if (CheckRecipientTextbox(out errorMessage))
            {
                SendMessage();
                this.Close();
            }
            else
            {
                MessageBox.Show(errorMessage);
                SendMessageButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Добавляет выбраного получателя в список рассылки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnRecipientComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employee selectedEmployee = (Employee)MessageControl1.RecipientCombobox.SelectedItem;

            if (string.Compare(MessageControl1.RecipientTextbox.Text, string.Empty) != 0)
                MessageControl1.RecipientTextbox.Text += ";";

            MessageControl1.RecipientTextbox.Text += EmployeeToString(selectedEmployee);
        }

        /// <summary>
        /// Запускает проверку списка рассылки после завершения ввода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRecipientTextboxTextChanged(object sender, TextChangedEventArgs e)
        {
            string errorMessage;
            if (CheckRecipientTextbox(out errorMessage))
            {
                string result = string.Empty;
                foreach (var employee in RecipientsEmployees)
                    result += EmployeeToString(employee) + ";";

                result = result.Substring(0, result.Length - 1);
                MessageControl1.RecipientTextbox.Text = result;
                SendMessageButton.IsEnabled = true;
            }
            else
            {
                //MessageBox.Show(errorMessage);
                SendMessageButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Производит проверку списка рассылки
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool CheckRecipientTextbox(out string errorMessage )
        {
            RecipientsEmployees.Clear();
            string[] recipientsStringArray = MessageControl1.RecipientTextbox.Text.Split(new char[1] {';'}, StringSplitOptions.RemoveEmptyEntries);

            if (recipientsStringArray.Length == 0)
            {
                errorMessage = Properties.Resources.RecipientsStringNotBeEmpty;
                return false;
            }
            return CheckRecipientStringArray(recipientsStringArray, out errorMessage);   
        }

        /// <summary>
        /// Проверяет список рассылки построчно
        /// </summary>
        /// <param name="recipientsStringArray"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool CheckRecipientStringArray(string[] recipientsStringArray, out string errorMessage)
        {
            foreach (var recipientString in recipientsStringArray)
            {
                if (!ProcessRecipietnString(recipientString, out errorMessage))
                    return false;                
            }
            errorMessage = string.Empty;
            return true;           
        }

        /// <summary>
        /// Обрабатывает одну строку из списка рассылки
        /// </summary>
        /// <param name="recipientString"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool ProcessRecipietnString(string recipientString, out string errorMessage)
        {
            string username = ParseUsername(recipientString);            
            Employee foundEmployee = AllEmployees.FirstOrDefault(i => (string.Compare(i.Username, username) == 0));
            if (foundEmployee != null)
            {
                RecipientsEmployees.Add(foundEmployee);
                errorMessage = string.Empty;
                return true;
            }
            else
            {
                errorMessage = username + " " + Properties.Resources.NotFound;
                return false;
            }
        }
        
        /// <summary>
        /// Преобразует поля класса Employee в строку
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private string EmployeeToString(Employee employee)
        {
            return employee.FirstName + " "
                    + employee.SecondName + " <"
                    + employee.Username + ">";           
        }

        /// <summary>
        /// Проверяет строку содержащую <username>
        /// </summary>
        /// <param name="recipientString"></param>
        /// <returns></returns>
        private string ParseUsername(string recipientString)
        {
            int begin = recipientString.IndexOf('<'),
                end = recipientString.IndexOf('>');

            if (begin == -1 || end == -1)
            {
                return recipientString;
            }
            else
            {
                begin += 1;
                return recipientString.Substring(begin, end - begin);
            }
        }

        private void SendMessage()
        {
            var recipients = CreateRecipientsList(RecipientsEmployees);
            App.Proxy.SendMessage(
                new Message(
                    null,
                    MessageControl1.TitleTextbox.Text,
                    DateTime.Parse(MessageControl1.DateTextbox.Text),
                    recipients,
                    App.Username,
                    MessageControl1.MessageContent.Text,
                    false));

        }

        private List<Recipient> CreateRecipientsList(List<Employee> employees)
        {
            List<Recipient> recipietns = new List<Recipient>();
            foreach (var item in employees)
            {
                recipietns.Add(
                    new Recipient(item.Username, null, false, false));
            }
            
            return recipietns;
        }
    }
}
