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
using System.ComponentModel;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for RecipientsControl.xaml
    /// </summary>
    public partial class RecipientsControl : UserControl
    {
        public RecipientsControl()
        {
            InitializeComponent();   
            Binding bind = new Binding();
            bind.Path = new PropertyPath("IsValid");
            SetBinding(IsValidProperty, bind);
        }

        RecipientsControlModel RecipientsControlModel
        {
            get
            {
                if (this.DataContext == null)
                    this.DataContext = new RecipientsControlModel();
                
                return (RecipientsControlModel)this.DataContext;
            }
        }

        /// <summary>
        /// Два состояния отображения контрола
        /// </summary>
        public enum state { IsReadOnly, IsEditable }

        /// <summary>
        /// Определяет вариант отображения контрола
        /// </summary>
        state controlState;

        /// <summary>
        /// Определяет вариант отображения контрола
        /// </summary>
        public state ControlState
        {
            get
            {
                return controlState;
            }
            set
            {
                controlState = value;
                PrepareControl();
            }
        }

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool),
            typeof(RecipientsControl), new UIPropertyMetadata(false),
            null);     
        
        /// <summary>
        /// Подготавливает контрол для различных вариантов использования
        /// </summary>
        void PrepareControl()
        {
            if (controlState == state.IsReadOnly)
            {
                AddButton.Visibility = System.Windows.Visibility.Collapsed;
                RecipientsTextBox.IsReadOnly = true;  
            }
            else
            {
                AddButton.Visibility = System.Windows.Visibility.Visible;
                RecipientsTextBox.IsReadOnly = false;
            }
        }

        /// <summary>
        /// Обработчик кнопки открытия диалога выбора сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            CreateRecipientsEditorWindow();
        }

        void CreateRecipientsEditorWindow()
        {
            RecipientsEditor recipientsEditor = new RecipientsEditor();
            recipientsEditor.DataContext = new RecipientsEditorModel()
            {
                AllEmployees = RecipientsControlModel.AllResidueEmployees,
                RecipientsEmployees = RecipientsControlModel.RecipientsEmployees
            };
            if(recipientsEditor.ShowDialog() == true)
            {
                RecipientsEditorModel rem = (RecipientsEditorModel)recipientsEditor.DataContext;
                RecipientsControlModel.RecipientsEmployees = rem.RecipientsEmployees;
                RecipientsControlModel.UpdateByRecipientsEmployees();
            }
        }

        /// <summary>
        /// При потери фокуса RecipientsTextBox-м наполнение RecipientsString данными о распознанных пользователях
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRecipientsTextBoxLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            RecipientsControlModel.UpdateRecipientsDefenitionInRecipientsString();
        }    
    }
}


