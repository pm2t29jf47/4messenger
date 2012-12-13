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
using System.Windows.Threading;
using Entities;
using WPFClient.Models;
using WPFClient.UserControls;
using WPFClient.Additional;
using WPFClient.SidebarFolders;
using System.ComponentModel;
using System.Threading;
using System.Globalization;
using System.Windows.Markup;
using System.Diagnostics;
using System.ServiceModel;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Common code       

        /// <summary>
        /// Сообщение выбранное в списке MessageList. Хранится для помечания просмотренным
        /// </summary>
        MessageListItemModel singleSelectedInMessageList;

        SidebarFolder selectedFolder;

        DispatcherTimer messageIsViewedTimer = new System.Windows.Threading.DispatcherTimer()
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
            ///Для элементов WPF культура задается отдельно o_0
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
            App.ServiceWatcher.CreateChannel();
            App.ServiceWatcher.StartWatching();            
        }

        void PrepareWindow()
        {
            PreareSidebar();
            this.ReplyMessageButton.IsEnabled = false;
            this.DeleteMessageButton.IsEnabled = false;
            MessageControl.ControlState = MessageControl.state.IsReadOnly;
            PrepareStatusBar();
        }

        void PrepareEmployeeClass()
        {
            Employee.CurrentUsername = App.ServiceWatcher.FactoryUsername;
            Employee.NamePrefix = Properties.Resources.Me;
        }
        
        void PreareSidebar()
        {
            List<SidebarFolder> folders = new List<SidebarFolder>();
            FillFoldersNames(folders);   
            Sidebar.ItemsSource = folders;
        }

        void PrepareStatusBar()
        {
            StatusBar.MouseDown += new MouseButtonEventHandler(OnStatusBarMouseDown);
            StatusBar.DataContext = new StatusBarModel();
        }

        void FillFoldersNames(List<SidebarFolder> folders)
        {
            folders.Add(new InboxFolder());
            folders.Add(new SentboxFolder());
            folders.Add(new DeletedFolder());
           
        }

        void ShowLoginWindow()
        {
            this.Hide();
            var loginWindow = new LoginWindow();
            bool? result = loginWindow.ShowDialog();
            if ( bool.Equals(result, null)
                || result.Equals(false))
            {
                App.Current.Shutdown();
            }
            this.Show();
        }

        void OnMessageListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MessageList.SelectedItem != null)
            {
                CheckDeleteReplyButtonsState();
                MessageListItemModel selectedMessageModel = (MessageListItemModel)MessageList.SelectedItem;                
                if ((selectedFolder is InboxFolder || selectedFolder is DeletedFolder)
                    && selectedMessageModel.IsViewed == false)
                {
                    messageIsViewedTimer.Stop();
                    this.singleSelectedInMessageList = selectedMessageModel;                    
                    messageIsViewedTimer.Start();
                }
                MessageControl.DataContext = new MessageControlModel()
                {
                    AllEmployees = App.ServiceWatcher.GetAllEmployees(),
                    Message = selectedMessageModel.Message
                };
            }
        }

        void CheckDeleteReplyButtonsState()
        {
            switch (MessageList.SelectedItems.Count)
            {
                case 0:
                    {
                        this.ReplyMessageButton.IsEnabled = false;
                        this.DeleteMessageButton.IsEnabled = false;
                        break;
                    }
                case 1:
                    {
                        this.ReplyMessageButton.IsEnabled = true;
                        this.DeleteMessageButton.IsEnabled = true;
                        break;
                    }
                default:
                    {
                        this.ReplyMessageButton.IsEnabled = false;
                        this.DeleteMessageButton.IsEnabled = true;
                        break;
                    }
            } 
        }

        void OnmessageIsViewedTimerTick(object sender, EventArgs e)
        {
            SetMessageViewed();     
        }

        void SetMessageViewed()
        {
            messageIsViewedTimer.Stop();
            MessageListItemModel selectedMessage = (MessageListItemModel)MessageList.SelectedItem;
            if (this.singleSelectedInMessageList.Equals(selectedMessage)
                && selectedMessage != null)
            {
                int selectedMessageId = (int)selectedMessage.Id;
                try
                {
                    App.ServiceWatcher.SetRecipientViewed(selectedMessageId, true);
                }

                /// Сервис не отвечает
                catch (EndpointNotFoundException ex)
                {
                    HandleSetMessageViewedException(ex);
                }

                ///Креденшелы не подходят
                catch (System.ServiceModel.Security.MessageSecurityException ex)
                {
                    HandleSetMessageViewedException(ex);
                }

                /// Ошибка в сервисе
                /// (маловероятна, при таком варианте скорее сработает ошибка креденшелов,
                /// т.к. проверка паролей происходит на каждом запросе к сервису и ей необходима БД)
                catch (FaultException<System.ServiceModel.ExceptionDetail> ex)
                {
                    HandleSetMessageViewedException(ex);
                }

                /// Остальные исключения
                catch (Exception ex)
                {
                    HandleSetMessageViewedException(ex);
                    throw;
                } 
                UploadToMessageList();
                if (selectedFolder is InboxFolder)
                {
                    ((InboxFolder)selectedFolder).RefreshCountOfUnViewedMessages();
                }
            }
        }

        void HandleSetMessageViewedException(Exception ex)
        {
            InformatonTips.SomeError.Show(ex.Message);
            ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.MainWindow.SetMessageViewed()");
            StatusBarModel statusBarModel = (StatusBarModel)StatusBar.DataContext;
            statusBarModel.Exception = ex;
            statusBarModel.ShortMessage = Properties.Resources.ConnectionError;
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
            messageIsViewedTimer.Stop();
            MessageList.SelectedItems.Clear();
            LoadFolderData(sender);
            
        }

        void LoadFolderData(object sender)
        {
            Button selectedFolderButton = (Button)sender;
            SidebarFolder sidebarFolder = (SidebarFolder)selectedFolderButton.DataContext;
            this.singleSelectedInMessageList = null;
            this.selectedFolder = sidebarFolder;            
            UploadToMessageList();
            this.ReplyMessageButton.IsEnabled = false;
            this.DeleteMessageButton.IsEnabled = false;
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
                SenderUsername = App.ServiceWatcher.FactoryUsername,
                Title = string.Empty,
                FKEmployee_SenderUsername = App.ServiceWatcher.GetAllEmployees().FirstOrDefault(row => string.Compare(App.ServiceWatcher.FactoryUsername, row.Username) == 0),
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
                SenderUsername = App.ServiceWatcher.FactoryUsername,
                Title = newTitle,
                EDRecipient_MessageId = recipients,
                FKEmployee_SenderUsername = App.ServiceWatcher.GetAllEmployees().FirstOrDefault(row => string.Compare(App.ServiceWatcher.FactoryUsername, row.Username) == 0)
            };
            CreateMessageCreatorWindow(message);
        }

        void CreateMessageCreatorWindow(Message message)
        {
            MessageCreator messageCreator = new MessageCreator();
            messageCreator.DataContext = new MessageCreatorModel()
            {
                AllEmployees = App.ServiceWatcher.GetAllEmployees(),
                Message = message,
                StatusBar = this.StatusBar
            };
            messageCreator.Title = Properties.Resources.MessageCreatorTitle; ///? тоже в дата контекст ?
            messageCreator.Show();
        }

        string PrepareReplyMssageTitle(string title)
        {
            return Properties.Resources.Re
                + SpecialWords.SpecialWords.Space
                + SpecialWords.SpecialWords.LeftSquareBrackets
                + title
                + SpecialWords.SpecialWords.RightSquareBracket;
        }

        #endregion

        #region ServiceWatcher        

        void CreateServiceWatcherHandler()
        {
            App.ServiceWatcher.DataUpdated += new Additional.ServiceWatcher.eventHandler(OnServiceWatcherDataUpdated);
        }

        void OnServiceWatcherDataUpdated(object sender, PropertyChangedEventArgs e)
        {
            UpdateWindow();
        }

        void UpdateWindow()
        {
            StatusBarModel statusBarModel = (StatusBarModel)StatusBar.DataContext;
            if (System.Exception.Equals(App.ServiceWatcher.DataDownloadException, null))
            {
                UpdateSelectionFolderContent();
                statusBarModel.ShortMessage = Properties.Resources.Connected;
                statusBarModel.Exception = null;
            }
            else
            {
                statusBarModel.ShortMessage = Properties.Resources.ConnectionError;
                statusBarModel.Exception = App.ServiceWatcher.DataDownloadException;
            }
        }

        void OnStatusBarMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowConnectionErrorDetailsWindow();
        }

        void ShowConnectionErrorDetailsWindow()
        {
            StatusBarModel StatusBarModel = (StatusBarModel)StatusBar.DataContext;
            if (!System.Exception.Equals(StatusBarModel.Exception, null))
            {
                ///окошко с текстом исключения!
                ConnectionErrorDetails сonnectionErrorDetails = new ConnectionErrorDetails();
                сonnectionErrorDetails.DataContext = new ConnectionErrorDetailsModel()
                {
                    Exception = StatusBarModel.Exception
                };
                bool? result = сonnectionErrorDetails.ShowDialog();
                if(! bool.Equals(result, null)
                    && result.Equals(true))
                {
                    ///Пересоздаем канал и перезапускаем таймер проверки сервиса
                    App.ServiceWatcher.StopWatching();
                    App.ServiceWatcher.DestroyCurrentChannel();
                    App.ServiceWatcher.CreateChannel();                    
                    App.ServiceWatcher.StartWatching();                    
                }
            }
        }

        void UpdateSelectionFolderContent()
        {            
            UpdateInboxFolderDisplay();
            if (selectedFolder is InboxFolder)
            {
                UploadToMessageList();
            }
            else if (selectedFolder is SentboxFolder)
            {
                UploadToMessageList();
            }
            else if (selectedFolder is DeletedFolder)
            {
                UploadToMessageList();
            }
        }

        void UpdateInboxFolderDisplay()
        {
            List<SidebarFolder> folders = (List<SidebarFolder>)Sidebar.ItemsSource;
            foreach (SidebarFolder item in folders)
            {
                if (item is InboxFolder)
                {
                    ((InboxFolder)item).RefreshCountOfUnViewedMessages();
                }
            }
        }

        #endregion
    }
}
