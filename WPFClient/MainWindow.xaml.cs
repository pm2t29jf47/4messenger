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
using WPFClient.Models;
using WPFClient.UserControls;
using WPFClient.Additional;
using WPFClient.OverEntities;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<SidebarFolder> folders = new List<SidebarFolder>();

        public MainWindow()
        {
            ///Выбирает локаль
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en");
            ///
            InitializeComponent();
            PrepareWindow();
            ShowLoginWindow();
        }

        public void PrepareWindow()
        {
            PreareSidebar();
            HideToolbarButtons(true);
            MessageControl.ControlState = MessageControl.state.IsReadOnly;
        }
        
        void HideToolbarButtons(bool state)
        {
            this.ReplyMessageButton.Visibility = state ? Visibility.Collapsed : Visibility.Visible;
            this.DeleteMessageButton.Visibility = state ? Visibility.Collapsed : Visibility.Visible; 
        }

        void PreareSidebar()
        {            
            FillFoldersNames();
            foreach (var folder in folders)
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
            folders.Add(new SidebarFolder(Properties.Resources.InboxFolderLabel));
            folders.Add(new SidebarFolder(Properties.Resources.SentboxFolderLabel));
            folders.Add(new SidebarFolder(Properties.Resources.DeletedFolderLabel));
        }

        void ShowLoginWindow()
        {
            this.Hide();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        void OnMessageListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MessageList.SelectedItem == null)
                return;
            MessageControlModel mcm = new MessageControlModel()
            {
                AllEmployees = App.Proxy.GetAllEmployees(),
                Message = ((MessageModel)MessageList.SelectedItem).Message,
                Recipients = ((MessageModel)MessageList.SelectedItem).Recipients,
                SenderEmployee = ((MessageModel)MessageList.SelectedItem).SenderEmployee
            };
            MessageControl.DataContext = mcm;
            HideToolbarButtons(false);       
        }

        /// <summary>
        /// Общий обработчик для нажатий на папки в Sidebar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnFolderClick(object sender, RoutedEventArgs e)
        {   
            var selectedFolder = (Button)sender;
            HideToolbarButtons(true);
            if (string.Compare(selectedFolder.Name, Properties.Resources.DeletedFolderLabel) == 0)
                PrepareMessageListForDeletedFolder();
            else if (string.Compare(selectedFolder.Name, Properties.Resources.InboxFolderLabel) == 0)
                PrepareMessageListForInboxFolder();
            else if (string.Compare(selectedFolder.Name, Properties.Resources.SentboxFolderLabel) == 0)
                PrepareMessageListForSentboxFolder();
            else
                OnUserFolderClick();
            
        }

        void PrepareMessageListForInboxFolder()
        {
            MessageList.ItemTemplate = (DataTemplate)FindResource("ForInboxFolderTemplate");
            List<Message> InboxMessages = App.Proxy.GetInboxMessages();
            List<MessageModel> InboxMessagesModel = new List<MessageModel>();
            foreach (var item in InboxMessages)
            {
                InboxMessagesModel.Add(new MessageModel()
                {
                    Message = item,
                    SenderEmployee = App.Proxy.GetEmployee(item.SenderUsername),
                    Recipients = App.Proxy.GetRecipients((int)item.Id)
                });
            }
            MessageList.ItemsSource = InboxMessagesModel;
        }

        void PrepareMessageListForSentboxFolder()
        {
            MessageList.ItemTemplate = (DataTemplate)FindResource("DefaultFolderTemplate");            
            List<Message> SentboxMessages = App.Proxy.GetSentboxMessages();
            List<MessageModel> SentboxMessagesModel = new List<MessageModel>();
            foreach (var item in SentboxMessages)
            {
                SentboxMessagesModel.Add(new MessageModel()
                {
                    Message = item,
                    SenderEmployee = App.Proxy.GetEmployee(item.SenderUsername),
                    Recipients = App.Proxy.GetRecipients((int)item.Id)
                });
            }
            MessageList.ItemsSource = SentboxMessagesModel;
        }

        void PrepareMessageListForDeletedFolder()
        {
            MessageList.ItemTemplate = (DataTemplate)FindResource("DefaultFolderTemplate");
            List<Message> DeletedMessages = App.Proxy.GetDeletedMessages();     
            List<MessageModel> DeletedMessagesModel = new List<MessageModel>();
            foreach (var item in DeletedMessages)
            {
                DeletedMessagesModel.Add(new MessageModel()
                {
                    Message = item,
                    SenderEmployee = App.Proxy.GetEmployee(item.SenderUsername),
                    Recipients = App.Proxy.GetRecipients((int)item.Id)
                });
            }
            MessageList.ItemsSource = DeletedMessagesModel;
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
            MessageCreator messageCreator = new MessageCreator();
            messageCreator.DataContext = new MessageCreatorModel()
            {
                Message = new Message(null, string.Empty, new DateTime(), App.Username, string.Empty, false),
                AllEmployees = App.Proxy.GetAllEmployees()
            };
            messageCreator.Title = Properties.Resources.MessageCreatorTitle; ///? тоже в дата контекст
            messageCreator.Show();            
        }

        public void OnReplyMessageButtonClick(object sender, RoutedEventArgs e)
        {
            //var selectedMessage = (Message)MessageList.SelectedItem;
            //var recipientEmployee = App.Proxy.GetEmployee(selectedMessage.SenderUsername);
            //string recipientString = recipientEmployee.FirstName + this.space
            //        + recipientEmployee.SecondName + space + leftUsernameStopper
            //        + recipientEmployee.Username + rightUsernameStopper;



            //string titleString = Properties.Resources.Re
            //    + space
            //    + leftTitleStopper
            //    + selectedMessage.Title
            //    + rightTitleStopper;       
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
                result = employee.FirstName
                    + SpecialSymbols.space
                    + employee.SecondName
                    + SpecialSymbols.space
                    + SpecialSymbols.leftUsernameStopper
                    + employee.Username
                    + SpecialSymbols.rightUsernameStopper;
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
                    result += EmployeeToString(item) + SpecialSymbols.userDataDevider;

                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }
    }
}
