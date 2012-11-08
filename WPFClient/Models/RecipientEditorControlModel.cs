using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Collections.ObjectModel;

namespace WPFClient.Models
{
    class RecipientEditorControlModel
    {
        public RecipientEditorControlModel()
        {
            AllEmployees = new ObservableCollection<Employee>();
            SelectedEmployees = new ObservableCollection<Employee>();
        }

        /// <summary>
        /// Коллекция содержащая всех возможных для выбора сотрудников
        /// </summary>
        public ObservableCollection<Employee> AllEmployees { get; set; }
        //{
        //    get
        //    {
        //        if (this.AllEmployeesListBox.ItemsSource == null)
        //            this.AllEmployeesListBox.ItemsSource = new ObservableCollection<Employee>();

        //        return (ObservableCollection<Employee>)this.AllEmployeesListBox.ItemsSource;
        //    }
        //    set
        //    {
        //        this.AllEmployeesListBox.ItemsSource = value;
        //    }
        //}

        /// <summary>
        /// Коллекция содержащая выбраных пользователем сотрудников
        /// </summary>
        public ObservableCollection<Employee> SelectedEmployees { get; set; }
        //{
        //    get
        //    {
        //        if (this.SelectedEmployeesListBox.ItemsSource == null)
        //            this.SelectedEmployeesListBox.ItemsSource = new ObservableCollection<Employee>();

        //        return (ObservableCollection<Employee>)this.SelectedEmployeesListBox.ItemsSource;
        //    }
        //    set
        //    {
        //        this.SelectedEmployeesListBox.ItemsSource = value;
        //    }
        //}

    }
}
