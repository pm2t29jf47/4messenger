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
            DataContextChanged += new DependencyPropertyChangedEventHandler(OnRecipientsEditorDataContextChanged);
            this.Closing +=new CancelEventHandler(OnRecipientsEditorClosing);
        }

        RecipientsEditorModel RecipientsEditorModel
        {
            get
            {
                return (RecipientsEditorModel)this.DataContext;
            }
        }

        void OnRecipientsEditorClosing(object sender, CancelEventArgs e)
        {            
            RecipientsEditorControlModel recm = (RecipientsEditorControlModel)this.RecipientsEditorControl.DataContext;
            if (recm.SelectedEmployees != null)
            {
                RecipientsEditorModel.RecipientsEmployees = new List<Employee>();
                foreach (var item in recm.SelectedEmployees)
                    RecipientsEditorModel.RecipientsEmployees.Add(item);
            }
        }

        void OnRecipientsEditorDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RecipientsEditorControlModel recm = new RecipientsEditorControlModel();
            if (RecipientsEditorModel.AllEmployees != null)
            {
                foreach (var item in RecipientsEditorModel.AllEmployees)
                    recm.AllEmployees.Add(item);
            }
            RecipientsEditorControl.DataContext = recm;
        }

        ///// <summary>
        ///// Коллекция содержащая всех возможных для выбора сотрудников
        ///// </summary>
        //public List<Employee> AllEmployees
        //{
        //    set                
        //    {              
        //        if (value == null) 
        //            return;

        //        foreach (var item in value)
        //            this.RecipientsEditorControl.AllEmployees.Add(item);                    
        //    }
        //}

        ///// <summary>
        ///// Коллекция содержащая выбраных пользователем сотрудников
        ///// </summary>
        //public List<Employee> RecipientsEmployees
        //{
        //    get
        //    {
        //        //List<Employee> result = new List<Employee>();
        //        //foreach (var item in this.RecipientsEditorControl.SelectedEmployees)
        //        //    result.Add(item);
        //        //return result;
        //        List<Employee> result = new List<Employee>();
        //        RecipientEditorControlModel recm = (RecipientEditorControlModel)RecipientsEditorControl.DataContext;
        //        foreach (var item in recm.SelectedEmployees)
        //            result.Add(item);
        //        return result;
        //    }
        //}
    }
}
