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
using WPFClient.ControlsModels;
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
using System.Collections;
using Entities.Additional;
using WPFClient.OtherModels;


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

        public AllEmployeesModel AllEmployeesModel { get; set; }

        DispatcherTimer messageViewedTimer = new System.Windows.Threading.DispatcherTimer()
        {
            Interval = App.timePerMessageSetViewed
        };
        DispatcherTimer serviceWatcherTimer = new System.Windows.Threading.DispatcherTimer()
        {
            Interval = App.timeBetweenUpdating
        }; 

        public MainWindow()
        {
            Loaded += new RoutedEventHandler(OnMainWindowLoaded);            
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

        public void DisplayExceptionDetailInStatusBar(Exception ex)
        {
            StatusBarModel statusBarModel = (StatusBarModel)StatusBar.DataContext;
            statusBarModel.ShortMessage = (ex == null) ? Properties.Resources.Connected : Properties.Resources.ConnectionError;
            statusBarModel.Exception = ex;
        }

        #endregion

        #region Prepare window components

        void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            SetApp();
            PrepareWindow();
            ShowLoginWindow();
            PrepareEmployeeClass();
            CreateServiceWatcherHandler();
            FirstDataLoad();            
            CreateMessageViewedTimerHandler();
        }

        void CreateMessageViewedTimerHandler()
        {
            messageViewedTimer.Tick += new EventHandler(OnmessageViewedTimerTick);
        }

        void FirstDataLoad()
        {
            try
            {                
                UpdateWindow();
            }
            catch (EndpointNotFoundException ex)
            {
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.MainWindow.UpdateWindow()");
                DisplayExceptionDetailInStatusBar(ex);
            }

            ///Креденшелы не проходят проверку
            catch (System.ServiceModel.Security.MessageSecurityException ex)
            {
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.MainWindow.UpdateWindow()");
                DisplayExceptionDetailInStatusBar(ex);
            }

            /// Ошибка в сервисе
            /// (маловероятна, при таком варианте скорее сработает ошибка креденшелов,
            /// т.к. проверка паролей происходит на каждом запросе к сервису и ей необходима БД)
            catch (FaultException<System.ServiceModel.ExceptionDetail> ex)
            {
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.MainWindow.UpdateWindow()");
                DisplayExceptionDetailInStatusBar(ex);
            }

            /// Остальные исключения
            catch (Exception ex)
            {            
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.MainWindow.UpdateWindow()");
                DisplayExceptionDetailInStatusBar(ex);              
                throw; ///Неизвестное исключение пробасывается дальше
            }
            serviceWatcherTimer.Start();
        }

        void CreateServiceWatcherHandler()
        {
            this.serviceWatcherTimer.Tick += new EventHandler(OnserviceWatcherTimerTick);
        }

        void SetApp()
        {
            App.mainWindow = this;            
        }

        void PrepareWindow()
        {
            PreareSidebar();
            PrepareToolbar();
            MessageControl.ControlState = MessageControl.state.IsReadOnly;
            PrepareStatusBar();
            PrepareAllEmployeesModel();
        }

        void PrepareAllEmployeesModel()
        {
            AllEmployeesModel = new AllEmployeesModel();
        }

        void PrepareEmployeeClass()
        {
            Employee.CurrentUsername = App.factory.Credentials.UserName.UserName;
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
                 IsEnabled = true
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

        #endregion

        #region MessageList selection changed

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
                    AllEmployees = this.AllEmployeesModel.Employees,
                    Message = selectedMessageModel.Message
                };
            }
            else
            {
                CheckToolbarButtonsStateByFolderClick(); 
                MessageControl.DataContext = null;
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

        #endregion

        #region Set message vieved by timer tick

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
                    App.proxy.SetRecipientViewed(selectedMessageId, true);
                    UpdateWindow();
                }

                /// Сервис не отвечает
                catch (EndpointNotFoundException ex)
                {
                    HandleException(ex, "()WPFClient.MainWindow.SetMessageViewed()");
                }

                ///Креденшелы не подходят
                catch (System.ServiceModel.Security.MessageSecurityException ex)
                {
                    HandleException(ex, "()WPFClient.MainWindow.SetMessageViewed()");
                }

                /// Ошибка в сервисе
                /// (маловероятна, при таком варианте скорее сработает ошибка креденшелов,
                /// т.к. проверка паролей происходит на каждом запросе к сервису и ей необходима БД)
                catch (FaultException<System.ServiceModel.ExceptionDetail> ex)
                {
                    HandleException(ex, "()WPFClient.MainWindow.SetMessageViewed()");
                }

                /// Остальные исключения
                catch (Exception ex)
                {
                    HandleException(ex, "()WPFClient.MainWindow.SetMessageViewed()");
                    throw;
                } 
                UploadToMessageList();
                if (selectedFolder is InboxFolder)
                {
                    ((InboxFolder)selectedFolder).RefreshCountOfUnViewedMessages();
                }
            }
        }

        void UploadToMessageList()
        {
            if (this.selectedFolder != null)
            {
                List<MessageListItemModel> savedSelectedModels = new List<MessageListItemModel>();
                foreach (MessageListItemModel item in MessageList.SelectedItems)
                {
                    savedSelectedModels.Add(item);
                }
                List<MessageListItemModel> loadedMessageModels = this.selectedFolder.GetFolderContent();
                MessageList.ItemsSource = loadedMessageModels;
                MessageList.SelectedItems.Clear();
                if (savedSelectedModels.Count != 0)
                {
                    foreach (MessageListItemModel savedSelectedModel in savedSelectedModels)
                    {
                        foreach (var item in MessageList.Items)
                        {
                            if (((MessageListItemModel)item).Id == savedSelectedModel.Id)
                            {
                                MessageList.SelectedItems.Add(item);
                            }
                        }
                        MessageList.Items.Refresh();
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

        #region "Create" "Reply" "Delete" "Recover" click

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
            try
            {
                DeleteMessages();
                UpdateWindow();
            }
            /// Сервис не отвечает
            catch (EndpointNotFoundException ex)
            {
                DisplayExceptionDetailInStatusBar(ex);
                HandleException(ex, "()WPFClient.MainWindow.DeleteMessages()");
            }

            ///Креденшелы не подходят
            catch (System.ServiceModel.Security.MessageSecurityException ex)
            {
                DisplayExceptionDetailInStatusBar(ex);
                HandleException(ex, "()WPFClient.MainWindow.DeleteMessages()");
            }

            /// Ошибка в сервисе
            /// (маловероятна, при таком варианте скорее сработает ошибка креденшелов,
            /// т.к. проверка паролей происходит на каждом запросе к сервису и ей необходима БД)
            catch (FaultException<System.ServiceModel.ExceptionDetail> ex)
            {
                DisplayExceptionDetailInStatusBar(ex);
                HandleException(ex, "()WPFClient.MainWindow.DeleteMessages()");
            }

            /// Остальные исключения, в т.ч. ArgumentException, ArgumentNullException
            catch (Exception ex)
            {
                DisplayExceptionDetailInStatusBar(ex);
                HandleException(ex, "()WPFClient.MainWindow.DeleteMessages()");
                throw;
            }
        }

        void DeleteMessages()
        {            
            if (selectedFolder is DeletedFolder)
            {
                if (!InformatonTips.RemoveDialog.Show(Properties.Resources.RemoveWarningText, Properties.Resources.Warning))
                {
                    return;
                }
                DeleteMessagesPermanently(MessageList.SelectedItems);
            }
            else
            {
                DeleteMessagesTemporarily(MessageList.SelectedItems);
            }           
        }

        void HandleException(Exception ex, string methodDescriptor)
        {
            InformatonTips.SomeError.Show(ex.Message);
            ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, methodDescriptor);
            DisplayExceptionDetailInStatusBar(ex);
        }

        void DeleteMessagesPermanently(IList models)
        {
            foreach (MessageListItemModel item in models)
            {
                if (item.Type == MessageParentType.Inbox)
                {
                    App.proxy.PermanentlyDeleteRecipient((int)item.Id);
                }
                else
                {
                    App.proxy.PermanentlyDeleteMessage((int)item.Id);
                }
            }
        }

        void DeleteMessagesTemporarily(IList models)
        {
            foreach (MessageListItemModel item in models)
            {
                if (item.Type == MessageParentType.Inbox)
                {
                    App.proxy.SetRecipientDeleted((int)item.Id, true);
                }
                else
                {
                    App.proxy.SetMessageDeleted((int)item.Id, true);
                }
            }
        }

        void OnRecoverMessageButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RecoverMessages();
                UpdateWindow();
            }
            /// Сервис не отвечает
            catch (EndpointNotFoundException ex)
            {
                HandleException(ex, "()WPFClient.MainWindow.RecoverMessages()");
            }

            ///Креденшелы не подходят
            catch (System.ServiceModel.Security.MessageSecurityException ex)
            {
                HandleException(ex, "()WPFClient.MainWindow.RecoverMessages()");
            }

            /// Ошибка в сервисе
            /// (маловероятна, при таком варианте скорее сработает ошибка креденшелов,
            /// т.к. проверка паролей происходит на каждом запросе к сервису и ей необходима БД)
            catch (FaultException<System.ServiceModel.ExceptionDetail> ex)
            {
                HandleException(ex, "()WPFClient.MainWindow.RecoverMessages()");
            }

            /// Остальные исключения, в т.ч. ArgumentException, ArgumentNullException
            catch (Exception ex)
            {
                HandleException(ex, "()WPFClient.MainWindow.RecoverMessages()");
                throw;
            }
        }

        private void RecoverMessages()
        {

            foreach (MessageListItemModel item in MessageList.SelectedItems)
            {
                if (item.Type == MessageParentType.Inbox)
                {
                    App.proxy.SetRecipientDeleted((int)item.Id, false);
                }
                else
                {
                    App.proxy.SetMessageDeleted((int)item.Id, false);
                }
            }
        }
           
        void CreateNewMessage()
        {
             Message message = new Message()
            {
                Content = string.Empty,
                Date = new DateTime(),
                Deleted = false,
                SenderUsername = App.factory.Credentials.UserName.UserName,
                Title = string.Empty,
                Sender = this.AllEmployeesModel.Employees.FirstOrDefault(row => string.Compare(App.factory.Credentials.UserName.UserName, row.Username) == 0),
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
                SenderUsername = App.factory.Credentials.UserName.UserName,
                Title = newTitle,
                Recipients = recipients,
                Sender = this.AllEmployeesModel.Employees.FirstOrDefault(row => string.Compare(App.factory.Credentials.UserName.UserName, row.Username) == 0)
            };
            CreateMessageCreatorWindow(message);
        }

        void CreateMessageCreatorWindow(Message message)
        {
            MessageCreator messageCreator = new MessageCreator();
            messageCreator.DataContext = new MessageCreatorModel()
            {
                AllEmployees = this.AllEmployeesModel.Employees,
                Message = message
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
        
        #region StatusBar click

        void OnStatusBarMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowConnectionErrorDetailsWindow();
        }

        void ShowConnectionErrorDetailsWindow()
        {
            StatusBarModel StatusBarModel = (StatusBarModel)StatusBar.DataContext;
            if (StatusBarModel.Exception != null)
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
                   RecreateProxy();                                                  
                }
            }
        }

        void RecreateProxy()
        {
            if (App.proxy != null)
            {
                ((System.ServiceModel.Channels.IChannel)App.proxy).Close();
            }
            App.proxy = App.factory.CreateChannel();    
        }

        #endregion

        #region Updating inner data

        void OnserviceWatcherTimerTick(object sender, EventArgs e)
        {
            serviceWatcherTimer.Stop();
            try
            {                
                UpdateWindow();
            }
            catch (EndpointNotFoundException ex)
            {
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.MainWindow.UpdateWindow()");
                DisplayExceptionDetailInStatusBar(ex);
            }

            ///Креденшелы не проходят проверку
            catch (System.ServiceModel.Security.MessageSecurityException ex)
            {
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.MainWindow.UpdateWindow()");
                DisplayExceptionDetailInStatusBar(ex);
            }

            /// Ошибка в сервисе
            /// (маловероятна, при таком варианте скорее сработает ошибка креденшелов,
            /// т.к. проверка паролей происходит на каждом запросе к сервису и ей необходима БД)
            catch (FaultException<System.ServiceModel.ExceptionDetail> ex)
            {
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.MainWindow.UpdateWindow()");
                DisplayExceptionDetailInStatusBar(ex);
            }

            /// Остальные исключения
            catch (Exception ex)
            {            
                ClientSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "()WPFClient.MainWindow.UpdateWindow()");
                DisplayExceptionDetailInStatusBar(ex);              
                throw; ///Неизвестное исключение пробасывается дальше
            }
            serviceWatcherTimer.Start();
        }

        public void UpdateWindow()
        {
            AllEmployeesModel.RefreshEmployees();
            List<SidebarFolder> folders = (List<SidebarFolder>)Sidebar.ItemsSource;
            foreach (SidebarFolder item in folders)
            {
                item.RefreshFolderContent();
            }
            UpdateInboxFolderDisplay();
            UploadToMessageList();
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
