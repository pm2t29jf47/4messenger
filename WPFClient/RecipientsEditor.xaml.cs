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
using WPFClient.Models;

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
            this.Closing +=new CancelEventHandler(OnRecipientsEditorClosing);
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

        void OnRecipientsEditorClosing(object sender, CancelEventArgs e)
        {            
            RecipientsEditorControlModel recm = (RecipientsEditorControlModel)this.RecipientsEditorControl.DataContext;         
            RecipientsEditorModel.RecipientsEmployees = new List<Employee>();
            foreach (var item in recm.SelectedEmployees)
                RecipientsEditorModel.RecipientsEmployees.Add(item);            
        }

        void OnRecipientsEditorDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RecipientsEditorControlModel recm = new RecipientsEditorControlModel();           
            foreach (var item in RecipientsEditorModel.AllEmployees)
                recm.AllEmployees.Add(item);
          
            RecipientsEditorControl.DataContext = recm;
        }
            }
}
