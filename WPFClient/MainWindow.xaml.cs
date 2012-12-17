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
using ServiceInterface;
using WPFClient.ToolbarButtons;
using System.Collections;


namespace WPFClient
{
    [Flags]
    public enum MessageFlags
    {
        Deleted = 1,
        Viewed = 2,
        Flagged = 4,
        ResponceRequired = 8
    }

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

        DispatcherTimer messageViewedTimer = new System.Windows.Threading.DispatcherTimer()
        {
            Interval = App.timePerMessageSetViewed
        };     

        public MainWindow()
        {
            Loaded += new RoutedEventHandler(OnMainWindowLoaded);
            messageViewedTimer.Tick += new EventHandler(OnmessageViewedTimerTick);
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
            PrepareToolbar();
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

        void PrepareToolbar()
         {
             CreateMessageButton.DataContext = new ButtonModel()
             {
                 DisabledImage = @"Images\mail_new_disabled.png",
                 EnabledImage = @"Images\mail_new.png",
                 IsEnabled = false
             };
             ReplyMessageButton.DataContext = new ButtonModel()
             {
                 DisabledImage = @"Images\mail_forward_disabled.png",
                 EnabledImage = @"Images\mail_forward.png",
                 IsEnabled = false
             };
             DeleteMessageButton.DataContext = new ButtonModel()
             {
                 DisabledImage = @"Images\mail_delete_disabled.png",
                 EnabledImage = @"Images\mail_delete.png",
                 IsEnabled = false
             };
             RecoverMessageButton.DataContext = new ButtonModel()
             {
                 DisabledImage = @"Images\mail_ok_disabled.png",
                 EnabledImage = @"Images\mail_ok.png",
                 IsEnabled = false
             };
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
                CheckToolbarButtonsStateByMessageListClick();
                MessageListItemModel selectedMessageModel = (MessageListItemModel)MessageList.SelectedItem;                
                if ((selectedFolder is InboxFolder || selectedFolder is DeletedFolder)
                    && selectedMessageModel.Viewed == false)
                {
                    messageViewedTimer.Stop();
                    this.singleSelectedInMessageList = selectedMessageModel;                    
                    messageViewedTimer.Start();
                }
                MessageControl.DataContext = new MessageControlModel()
                {
                    AllEmployees = App.ServiceWatcher.AllEmployees,
                    Message = selectedMessageModel.Message
                };
            }
        }

        void CheckToolbarButtonsStateByMessageListClick()
        {
            switch (MessageList.SelectedItems.Count)
            {
                case 0:
                    {
                        this.ReplyMessageButton.IsEnabled = false;
                        this.DeleteMessageButton.IsEnabled = false;
                        this.RecoverMessageButton.IsEnabled = false;
                        break;
                    }
                case 1:
                    {
                        this.ReplyMessageButton.IsEnabled = true;
                        this.DeleteMessageButton.IsEnabled = true;
                        this.RecoverMessageButton.IsEnabled = selectedFolder is DeletedFolder;
                        break;
                    }
                default:
                    {
                        this.ReplyMessageButton.IsEnabled = false;
                        this.DeleteMessageButton.IsEnabled = true;
                        this.RecoverMessageButton.IsEnabled = selectedFolder is DeletedFolder;
                        break;
                    }
            } 
        }

        void OnmessageViewedTimerTick(object sender, EventArgs e)
        {
            SetMessageViewed();     
        }

        void SetMessageViewed()
        {
            messageViewedTimer.Stop();
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
                    if (findItem != null)
                    {
                        MessageList.SelectedItems.Add(findItem);
                    }
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
            messageViewedTimer.Stop();
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
            CheckToolbarButtonsStateByFolderClick();
        }

        void CheckToolbarButtonsStateByFolderClick()
        {
            this.ReplyMessageButton.IsEnabled = false;
            this.DeleteMessageButton.IsEnabled = false;
            this.RecoverMessageButton.IsEnabled = false;   
        }

        #endregion

        #region "Create" "Reply" "Delete" "Recover" buttons

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
            DeleteMessages();   
        }

        void DeleteMessages()
        {
            try
            {
                if (selectedFolder is DeletedFolder)
                {
                    if (!InformatonTips.RemoveDialog.Show(Properties.Resources.RemoveWarningText, Properties.Resources.Warning))
                    {
                        return;
                    }
                    RemoveMessagesPermanently(MessageList.SelectedItems);
                }
                else
                {
                    RemoveMessagesTemporarily(MessageList.SelectedItems);
                }
            }
            /// Сервис не отвечает
            catch (EndpointNotFoundException ex)
            {
                HandleException(ex);
            }

            ///Креденшелы не подходят
            catch (System.ServiceModel.Security.MessageSecurityException ex)
            {
                HandleException(ex);
            }

            /// Ошибка в сервисе
            /// (маловероятна, при таком варианте скорее сработает ошибка креденшелов,
            /// т.к. проверка паролей происходит на каждом запросе к сервису и ей необходима БД)
            catch (FaultException<System.ServiceModel.ExceptionDetail> ex)
            {
                HandleException(ex);
            }

            /// Остальные исключения, в т.ч. ArgumentException, ArgumentNullException
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }

        void HandleException(Exception ex)
        {
            InformatonTips.SomeError.Show(ex.Message);
            ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.MessageCreator.OnSendMessageButtonClick(object sender, RoutedEventArgs e)");
            StatusBarModel statusBarModel = (StatusBarModel)StatusBar.DataContext;
            statusBarModel.Exception = ex;
            statusBarModel.ShortMessage = Properties.Resources.ConnectionError;
        }

        void RemoveMessagesPermanently(IList models)
        {
            foreach (MessageListItemModel item in models)
            {
                if (item.Type == MessageType.inbox)
                {
                    App.ServiceWatcher.PermanentlyDeleteRecipient((int)item.Id);
                }
                else
                {
                    App.ServiceWatcher.PermanentlyDeleteMessage((int)item.Id);
                }
            }
        }

        void RemoveMessagesTemporarily(IList models)
        {
            foreach (MessageListItemModel item in models)
            {
                if (item.Type == MessageType.inbox)
                {
                    App.ServiceWatcher.SetRecipientDeleted((int)item.Id, true);
                }
                else
                {
                    App.ServiceWatcher.SetMessageDeleted((int)item.Id, true);
                }
            }
        }

        void OnRecoverMessageButtonClick(object sender, RoutedEventArgs e)
        {
            RecoverMessages();
        }

        private void RecoverMessages()
        {
            try
            {
                foreach (MessageListItemModel item in MessageList.SelectedItems)
                {
                    if (item.Type == MessageType.inbox)
                    {
                        App.ServiceWatcher.SetRecipientDeleted((int)item.Id, false);
                    }
                    else
                    {
                        App.ServiceWatcher.SetMessageDeleted((int)item.Id, false);
                    }
                }
            }
            /// Сервис не отвечает
            catch (EndpointNotFoundException ex)
            {
                HandleException(ex);
            }

            ///Креденшелы не подходят
            catch (System.ServiceModel.Security.MessageSecurityException ex)
            {
                HandleException(ex);
            }

            /// Ошибка в сервисе
            /// (маловероятна, при таком варианте скорее сработает ошибка креденшелов,
            /// т.к. проверка паролей происходит на каждом запросе к сервису и ей необходима БД)
            catch (FaultException<System.ServiceModel.ExceptionDetail> ex)
            {
                HandleException(ex);
            }

            /// Остальные исключения, в т.ч. ArgumentException, ArgumentNullException
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
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
                Sender = App.ServiceWatcher.AllEmployees.FirstOrDefault(row => string.Compare(App.ServiceWatcher.FactoryUsername, row.Username) == 0),
                Recipients = new List<Recipient>()
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
                Recipients = recipients,
                Sender = App.ServiceWatcher.AllEmployees.FirstOrDefault(row => string.Compare(App.ServiceWatcher.FactoryUsername, row.Username) == 0)
            };
            CreateMessageCreatorWindow(message);
        }

        void CreateMessageCreatorWindow(Message message)
        {
            MessageCreator messageCreator = new MessageCreator();
            messageCreator.DataContext = new MessageCreatorModel()
            {
                AllEmployees = App.ServiceWatcher.AllEmployees,
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
