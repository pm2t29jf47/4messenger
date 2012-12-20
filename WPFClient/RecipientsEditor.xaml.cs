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
using System.Collections.ObjectModel;
using System.ComponentModel;
using WPFClient.ControlsModels;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for RecipientsEditor.xaml
    /// </summary>
    public partial class RecipientsEditor : Window
    {
        public RecipientsEditor()
        {
            InitializeComponent();
            this.Title = Properties.Resources.RecipientsList;
            DataContextChanged += new DependencyPropertyChangedEventHandler(OnRecipientsEditorDataContextChanged);
        }

        RecipientsEditorModel RecipientsEditorModel
        {
            get
            {
                if (this.DataContext == null)
                    this.DataContext = new RecipientsEditorModel();

                return (RecipientsEditorModel)this.DataContext;
            }
        }

        void OnRecipientsEditorDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SendDataToRecipientsEditorControl();
        }

        void SendDataToRecipientsEditorControl()
        {
            RecipientsEditorControlModel recm = new RecipientsEditorControlModel();
            foreach (var item in RecipientsEditorModel.AllEmployees)
                recm.AllEmployees.Add(item);

            foreach (var item in RecipientsEditorModel.RecipientsEmployees)
                recm.SelectedEmployees.Add(item);

            RecipientsEditorControl.DataContext = recm;
        }

        void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            SendDataToInnerModel();            
        }

        void SendDataToInnerModel()
        {
            RecipientsEditorControlModel recm = (RecipientsEditorControlModel)this.RecipientsEditorControl.DataContext;
            RecipientsEditorModel.RecipientsEmployees.Clear();
            foreach (var item in recm.SelectedEmployees)
                RecipientsEditorModel.RecipientsEmployees.Add(item);

            DialogResult = true;
            this.Close();   
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
