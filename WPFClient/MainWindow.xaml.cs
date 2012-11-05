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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Entities;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<SidebarFolder> folders = new List<SidebarFolder>();

        string leftUsernameStopper = " <",
            rightUsernameStopper = ">",
            usernameDevider = ";",
            space = " ",
            leftTitleStopper = "[",
            rightTitleStopper  ="]",
            SenderPrefix = Properties.Resources.Me,
            titlePrefix = Properties.Resources.Re;

        public MainWindow()
        {
            ///Выбирает локаль
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en");
            InitializeComponent();            
            ShowLoginWindow();
            
          //  MessageControl.AllEmployees = new List<Employee>();
        }

        public void PrepareWindow()
        {
            SetHandlers();
            PreareSidebar();
            HideToolbarControl1Buttons(true);
            MessageControl.ControlState = WPFClient.MessageControl.state.IsEditable;
        }
        
        void HideToolbarControl1Buttons(bool state)
        {
            //ToolbarControl1.ReplyMessageButton.Visibility = state ? Visibility.Collapsed : Visibility.Visible;
            //ToolbarControl1.DeleteMessageButton.Visibility = state ? Visibility.Collapsed : Visibility.Visible; 
        }

        void SetHandlers()
        {
            //ToolbarControl1.CreateMessageButton.Click += new RoutedEventHandler(OnCreateMessageButtonClick);
            //ToolbarControl1.ReplyMessageButton.Click += new RoutedEventHandler(OnCreateMessageButtonClick);  
            //ToolbarControl1.DeleteMessageButton.Click += new RoutedEventHandler(OnDeleteMessageButtonClick);
        }

        void PreareSidebar()
        {
            ///переделать под шаблоны
            FillFoldersNames();
            foreach (var folder in this.folders)
            {
                Button folderButton = new Button();
                StackPanel folderStackPanel = new StackPanel();
                Uri uri = new Uri("pack://application:,,,/Images/folder.png");
                BitmapImage source = new BitmapImage(uri);
                folderStackPanel.Children.Add(new Image { Source = source });
                folderStackPanel.Children.Add(new Label { Content = folder.FolderLabel });
                folderButton.Content = folderStackPanel;
                folderButton.Click += new RoutedEventHandler(OnFolderClick);
                folderButton.Name = folder.FolderLabel;
                Thickness a = new Thickness(5.0);
                folderButton.Margin = a;
                Sidebar.Children.Add(folderButton);
            }            
        }

        void FillFoldersNames()
        {
            this.folders.Add(new SidebarFolder(Properties.Resources.InboxFolderLable));
            this.folders.Add(new SidebarFolder(Properties.Resources.SentFolderLable));
            this.folders.Add(new SidebarFolder(Properties.Resources.DeletedFolderLable));
        }

        void ShowLoginWindow()
        {
            this.Hide();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        string GetRecipientsString()
        {
            var selectedMessage = (Message)MessageList.SelectedItem;
            List<Employee> recipientsEmployees = new List<Employee>();
            foreach (var recipient in selectedMessage.Recipients)
                recipientsEmployees.Add(App.Proxy.GetEmployee(recipient.RecipientUsername));

            return EmployeesToString(recipientsEmployees); 
        }

        void OnMessageListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            //var selectedMessage = (Message)MessageList.SelectedItem;
            //if (selectedMessage == null) return;    ///Ложные срабатывания
            //MessageControl.SenderTextbox.Text = selectedMessage.SenderUsername;
            //MessageControl.DateTextbox.Text = selectedMessage.Date.ToString();
            //MessageControl.TitleTextbox.Text = selectedMessage.Title;
            //MessageControl.MessageContentTextBox.Text = selectedMessage.Content;
            //MessageControl.RecipientTextbox.Text = GetRecipientsString();
            //HideToolbarControl1Buttons(false);       
        }

        /// <summary>
        /// Общий обработчик для нажатий на папки в Sidebar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnFolderClick(object sender, RoutedEventArgs e)
        {
            MessageControl.AllEmployees = App.Proxy.GetEmployeeList();
            var selectedFolder = (Button)sender;
            HideToolbarControl1Buttons(true);
            if (string.Compare(selectedFolder.Name, Properties.Resources.DeletedFolderLable) == 0)          
                MessageList.ItemsSource = App.Proxy.GetDeletedFolder();            
            else if (string.Compare(selectedFolder.Name, Properties.Resources.InboxFolderLable) == 0)   
                MessageList.ItemsSource = App.Proxy.GetInboxFolder();           
            else if (string.Compare(selectedFolder.Name, Properties.Resources.SentFolderLable) == 0)     
                MessageList.ItemsSource = App.Proxy.GetSentFolder();            
            else 
                OnUserFolderClick();
        }

        /// <summary>
        /// Обработка нажатия на пользовательскую папку
        /// </summary>
        void OnUserFolderClick()
        {
            throw new NotImplementedException();
        }

        public void OnCreateMessageButtonClick(object sender, RoutedEventArgs e) 
        {
            string senderString = this.SenderPrefix
                + this.space
                + this.leftUsernameStopper
                + App.Username
                + this.rightUsernameStopper;

            MessageCreator newMessage = new MessageCreator(senderString, string.Empty, string.Empty);
            newMessage.Title = Properties.Resources.MessageCreatorTitle;
            newMessage.Show();            
        }

        public void OnReplyMessageButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedMessage = (Message)MessageList.SelectedItem;
            var recipientEmployee = App.Proxy.GetEmployee(selectedMessage.SenderUsername);
            string recipientString = recipientEmployee.FirstName + this.space
                    + recipientEmployee.SecondName + this.leftUsernameStopper
                    + recipientEmployee.Username + this.rightUsernameStopper;

            string senderString = this.SenderPrefix
                + this.space
                + this.leftUsernameStopper 
                + App.Username 
                + this.rightUsernameStopper;

            string titleString = this.titlePrefix
                + this.space
                + this.leftTitleStopper
                + selectedMessage.Title
                + this.rightUsernameStopper;

            MessageCreator newMessage = new MessageCreator(senderString, recipientString, titleString);
            newMessage.Show();
        }

        public void OnDeleteMessageButtonClick(object sender, RoutedEventArgs e)
        {
           // var selectedMessage = (Message)MessageList.SelectedItem;
        }

        /// <summary>
        /// Преобразует поля класса Employee в строку
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        string EmployeeToString(Employee employee)
        {
            string result = string.Empty;
            if (employee != null)
            {
                result = employee.FirstName + this.space
                    + employee.SecondName + this.leftUsernameStopper
                    + employee.Username + this.rightUsernameStopper;
            }
            return result;
        }

        string EmployeesToString(List<Employee> Employees)
        {
            string result = string.Empty;
            if (Employees != null
                && Employees.Count != 0)
            {
                foreach (var item in Employees)
                    result += EmployeeToString(item) + this.usernameDevider;

                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }
    }
}
