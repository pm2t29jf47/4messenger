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

        /// <summary>
        /// Отлов события закрытия окна RecipientsEditor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnrecipientsEditorClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RecipientsEditor recipientsEditor = (RecipientsEditor)sender;
            RecipientsEditorModel rem = (RecipientsEditorModel)recipientsEditor.DataContext;
            RecipientsControlModel.AddEmployeesToRecipientsString(rem.RecipientsEmployees);
            RecipientsControlModel.UpdateRecipients();         
        }       
        
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
        /// Обработчик кнопки добавления полуателей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAddButtonClick(object sender, RoutedEventArgs e)
        {           
            RecipientsEditor recipientsEditor = new RecipientsEditor();            
            recipientsEditor.DataContext = new RecipientsEditorModel()
            {
                AllEmployees = RecipientsControlModel.AllResidueEmployees
            };
            recipientsEditor.Show();
            recipientsEditor.Closing += new System.ComponentModel.CancelEventHandler(OnrecipientsEditorClosing);
        }
    }
}


