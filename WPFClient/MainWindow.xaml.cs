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
using WPFClient.SidebarFolders;
using System.ComponentModel;
using System.Threading;
using System.Globalization;
using System.Windows.Markup;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Common code

        List<SidebarFolder> folders = new List<SidebarFolder>();

        /// <summary>
        /// Сообщение выбранное в списке MessageList. Хранится для помечания просмотренным
        /// </summary>
        MessageListItemModel singleSelectedInMessageList;

        SidebarFolder selectedFolder;

        System.Windows.Threading.DispatcherTimer messageIsViewedTimer = new System.Windows.Threading.DispatcherTimer()
        {
            Interval = App.timePerMessageSetViewed
        };

        public MainWindow()
        {
            Loaded += new RoutedEventHandler(OnMainWindowLoaded);
            messageIsViewedTimer.Tick += new EventHandler(OnmessageIsViewedTimerTick);
            SetCulture("ru");
            InitializeComponent();           
        }

        void SetCulture(string culture)
        {
            System.Globalization.CultureInfo ci = new
            System.Globalization.CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            ///Для элементов WPF культура задается отдельно
            FrameworkElement.LanguageProperty.OverrideMetadata(
              typeof(FrameworkElement),
              new FrameworkPropertyMetadata(
                  XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            PrepareWindow();
            ShowLoginWindow();
            PrepareEmployeeClass();
            CreateServiceWatcherHandler();
            App.ServiceWatcher.StartWatching();            
        }

        void PrepareWindow()
        {
            PreareSidebar();
            HideToolbarButtons(true);
            MessageControl.ControlState = MessageControl.state.IsReadOnly;
        }

        void PrepareEmployeeClass()
        {
            Employee.CurrentUsername = App.Username;
            Employee.NamePrefix = Properties.Resources.Me;
        }
        
        void HideToolbarButtons(bool state)
        {
            this.ReplyMessageButton.Visibility = state ? Visibility.Collapsed : Visibility.Visible;
            this.DeleteMessageButton.Visibility = state ? Visibility.Collapsed : Visibility.Visible; 
        }

        void PreareSidebar()
        {  
            FillFoldersNames();   
            Sidebar.ItemsSource = folders;
        }

        void FillFoldersNames()
        {
            folders.Add(new InboxFolder());
            folders.Add(new SentboxFolder());
            folders.Add(new DeletedFolder());
           
        }

        void ShowLoginWindow()
        {
            this.Hide();
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();      
        }

        void OnMessageListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MessageList.SelectedItem == null)
            {
                return;
            }
            MessageListItemModel selectedMessageModel = (MessageListItemModel)MessageList.SelectedItem;
            this.singleSelectedInMessageList = selectedMessageModel;
            if ((selectedFolder is InboxFolder  || selectedFolder is DeletedFolder)
                && selectedMessageModel.IsViewed == false)
            {
                messageIsViewedTimer.Stop();
                messageIsViewedTimer.Start();                
            }
            MessageControl.DataContext = new MessageControlModel()
            {
                AllEmployees = App.ServiceWatcher.GetAllEmployees(),
                Message = selectedMessageModel.Message                
            };
            HideToolbarButtons(false);       
        }

        void OnmessageIsViewedTimerTick(object sender, EventArgs e)
        {
            SetMessageViewed();     
        }

        void SetMessageViewed()
        {
            messageIsViewedTimer.Stop();
            Message selectedMessage = (MessageListItemModel)MessageList.SelectedItem;
            if (this.singleSelectedInMessageList.Equals(selectedMessage)
                && selectedMessage != null)
            {
                int selectedMessageId = (int)selectedMessage.Id;
                App.ServiceWatcher.SetRecipientViewed(selectedMessageId, true);
                UploadToMessageList();
                if (selectedFolder is InboxFolder)
                {
                    selectedFolder.CountOfUnviewedMessages = InboxFolder.CountOfUnViewed();
                }
            }
        }

        void UploadToMessageList()
        {
            List<MessageListItemModel> savedSelectedItems = new List<MessageListItemModel>();

            foreach (MessageListItemModel item in MessageList.SelectedItems)
            {
                savedSelectedItems.Add(item);
            }
            List<MessageListItemModel> loadedMessageModels = this.selectedFolder.GetFolderContent();
            MessageList.ItemsSource = loadedMessageModels;
            if (savedSelectedItems.Count != 0)
            {
                foreach (MessageListItemModel item in savedSelectedItems)
                {
                    MessageListItemModel findItem = loadedMessageModels.FirstOrDefault(row => row.Id == item.Id);
                    MessageList.SelectedItems.Add(findItem);
                }               
            }            
        }
        #endregion

        #region Prepare "MessageList" by foldel click

        /// <summary>
        /// Общий обработчик для нажатий на папки в Sidebar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnFolderClick(object sender, RoutedEventArgs e)
        {
            LoadFolderData(sender);
            messageIsViewedTimer.Stop();
        }

        void LoadFolderData(object sender)
        {
            Button selectedFolderButton = (Button)sender;
            SidebarFolder sidebarFolder = (SidebarFolder)selectedFolderButton.DataContext;
            this.singleSelectedInMessageList = null;
            this.selectedFolder = sidebarFolder;            
            UploadToMessageList();
            HideToolbarButtons(true); 
        }

        #endregion

        #region "Create" "Reply" "Delete" buttons

        void OnCreateMessageButtonClick(object sender, RoutedEventArgs e) 
        {
            CreateNewMessage();       
        }

        void OnReplyMessageButtonClick(object sender, RoutedEventArgs e)
        {
            CreateReplyMessage();
        }

        void OnDeleteMessageButtonClick(object sender, RoutedEventArgs e)
        {
   
        }
        
        void CreateNewMessage()
        {
             Message message = new Message()
            {
                Content = string.Empty,
                Date = new DateTime(),
                Deleted = false,
                SenderUsername = App.Username,
                Title = string.Empty,
                FKEmployee_SenderUsername = App.ServiceWatcher.GetEmployee(App.Username),
                EDRecipient_MessageId = new List<Recipient>()
            };
             CreateMessageCreatorWindow(message);
        }

        void CreateReplyMessage()
        {
            Message selectedMessage = ((MessageListItemModel)MessageList.SelectedItem).Message;
            string newTitle = PrepareReplyMssageTitle(selectedMessage.Title);          
            List<Recipient> recipients = new List<Recipient>();
            recipients.Add(
                new Recipient(selectedMessage.SenderUsername, null)
                {
                    Deleted = false,
                    Viewed = false
                });
            Message message = new Message()
            {
                Content = string.Empty,
                Date = new DateTime(),
                Deleted = false,
                SenderUsername = App.Username,
                Title = newTitle,
                EDRecipient_MessageId = recipients,
                FKEmployee_SenderUsername = App.ServiceWatcher.GetEmployee(App.Username)
            };
            CreateMessageCreatorWindow(message);
        }

        void CreateMessageCreatorWindow(Message message)
        {
            MessageCreator messageCreator = new MessageCreator();
            messageCreator.DataContext = new MessageCreatorModel()
            {
                AllEmployees = App.ServiceWatcher.GetAllEmployees(),
                Message = message
            };
            messageCreator.Title = Properties.Resources.MessageCreatorTitle; ///? тоже в дата контекст ?
            messageCreator.Show();
        }

        string PrepareReplyMssageTitle(string title)
        {
            return Properties.Resources.Re
                + SpecialSymbols.SpecialSymbols.space
                + SpecialSymbols.SpecialSymbols.leftTitleStopper
                + title
                + SpecialSymbols.SpecialSymbols.rightTitleStopper;
        }

        #endregion

        #region ServiceWatcher        

        void CreateServiceWatcherHandler()
        {
            App.ServiceWatcher.DataUpdated += new Additional.ServiceWatcher.eventHandler(OnServiceWatcherDataUpdated);
        }

        void OnServiceWatcherDataUpdated(object sender, PropertyChangedEventArgs e)
        {
            UpdateSelectionFolderContent(e.PropertyName);
        }

        void UpdateSelectionFolderContent(string propertyName)
        {
            if (string.Compare("allEmployees", propertyName) == 0)
                return;

            else if (string.Compare("inboxMessages", propertyName) == 0)
            {
                folders[0].CountOfUnviewedMessages = InboxFolder.CountOfUnViewed();
                if (selectedFolder is InboxFolder)
                    UploadToMessageList();
            }
            else if (string.Compare("sentboxMessages", propertyName) == 0 && selectedFolder is SentboxFolder)
                UploadToMessageList();

            else if ((string.Compare("deletedInboxMessages", propertyName) == 0 || string.Compare("deletedSentboxMessages", propertyName) == 0)
                && selectedFolder is DeletedFolder)
                UploadToMessageList();
        }
        #endregion
    }
}
