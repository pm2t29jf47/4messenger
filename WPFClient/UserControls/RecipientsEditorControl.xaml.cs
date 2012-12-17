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
using System.Collections.ObjectModel;
using WPFClient.Models;
using System.Windows.Controls.Primitives;

namespace WPFClient.UserControls
{
    delegate Point GetPositionDelegate(IInputElement element);

    public partial class RecipientsEditorControl : UserControl
    {
        public RecipientsEditorControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Возвращает модель из DataContex-а
        /// </summary>
        RecipientsEditorControlModel RecipientsEditorControlModel
        {
            get
            {
                if (this.DataContext == null)
                    this.DataContext = new RecipientsEditorControlModel();

                return (RecipientsEditorControlModel)this.DataContext;
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки переноса сотрудника в коллекцию выбраных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAddToSelectedButtonClick(object sender, RoutedEventArgs e)
        {           
            Employee[] selectedItems = new Employee[AllEmployeesListBox.SelectedItems.Count];
            Employee firstSelectedItem = (Employee)AllEmployeesListBox.SelectedItem;
            int nextSelection = AllEmployeesListBox.Items.IndexOf(firstSelectedItem);
            AllEmployeesListBox.SelectedItems.CopyTo(selectedItems, 0);            
            foreach (Employee item in selectedItems)
            {
                RecipientsEditorControlModel.SelectedEmployees.Add(item);
                RecipientsEditorControlModel.AllEmployees.Remove(item);
            }
            AllEmployeesListBox.SelectedIndex = nextSelection;
            if (nextSelection < AllEmployeesListBox.Items.Count)          
                AllEmployeesListBox.Focus();          
            else           
                AddToSelectedButton.IsEnabled = false;           
        }

        /// <summary>
        /// Обработчик события нажатия кнопки переноса сотрудника в коллекцию всех возможных для выбора сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRemoveFromSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            Employee[] selectedItems = new Employee[SelectedEmployeesListBox.SelectedItems.Count];
            Employee firstSelectedItem = (Employee)SelectedEmployeesListBox.SelectedItem;
            int nextSelection = SelectedEmployeesListBox.Items.IndexOf(firstSelectedItem);
            SelectedEmployeesListBox.SelectedItems.CopyTo(selectedItems, 0);
            foreach (Employee item in selectedItems)
            {               
                RecipientsEditorControlModel.AllEmployees.Add(item);
                RecipientsEditorControlModel.SelectedEmployees.Remove(item);
            }
            SelectedEmployeesListBox.SelectedIndex = nextSelection;
            if (nextSelection < SelectedEmployeesListBox.Items.Count)
                SelectedEmployeesListBox.Focus();            
            else
                RemoveFromSelectedButton.IsEnabled = false;
        }

        /// <summary>
        /// Смена состояния кнопок AddToSelectedButton и RemoveFromSelectedButton (IsEditable) при переводе фокуса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAllEmployeesListBoxIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AddToSelectedButton.IsEnabled = true;
            RemoveFromSelectedButton.IsEnabled = false;
        }

        /// <summary>
        /// Смена состояния кнопок AddToSelectedButton и RemoveFromSelectedButton (IsEditable) при переводе фокуса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSelectedEmployeesListBoxIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AddToSelectedButton.IsEnabled = false;
            RemoveFromSelectedButton.IsEnabled = true;
        }

        /// <summary>
        /// Отмечает всех сотрудников в списке AllEmployeesListBox по комбинации ctrl + a
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAllEmployeesSelectAllExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AllEmployeesListBox.SelectAll();     
        }

        /// <summary>
        /// Отмечает всех сотрудников в списке SelectedEmployeesListBox по комбинации ctrl + a
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSelectedEmployeesSelectAllExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectedEmployeesListBox.SelectAll();
        }
      
        void OnAllEmployeesListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DoDrag(sender, e);     
        }

        ListBox DraggedListBox;

        void DoDrag(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            if (parent.SelectedItems.Count != 0)
            {               
                int indexUnderMouse = GetCurrentIndex(parent, e.GetPosition);
                if (indexUnderMouse >= 0)
                {
                    Employee draggedItem = (Employee)parent.Items[indexUnderMouse];
                    if (parent.SelectedItems.Contains(draggedItem))
                    {
                        List<Employee> dataToSend = GetSelectedItems(parent);
                        DraggedListBox = parent;
                        DragDrop.DoDragDrop(parent, dataToSend, DragDropEffects.Move);                      
                    }
                }
            }
        }

        List<Employee> GetSelectedItems(ListBox source)
        {
            List<Employee> result = new List<Employee>();
            foreach (Employee item in source.SelectedItems)
            {
                result.Add(item);
            }
            return result;
        }

        void OnSelectedEmployeesListBoxDrop(object sender, DragEventArgs e)
        {          
           DoDrop(sender, e);  
        }

        void DoDrop(object sender, DragEventArgs e)
        {
            ListBox target = (ListBox)sender;
            ListBox source = DraggedListBox;
            DraggedListBox = null;
            int indexUnderMouse = this.GetCurrentIndex(target, e.GetPosition);
            if(indexUnderMouse < 0)
            {
                indexUnderMouse = target.Items.Count == 0 ? 0 : target.Items.Count - 1;
            }           
            List<Employee> externalSelectedEmployees = (List<Employee>)e.Data.GetData(typeof(List<Employee>));
            ObservableCollection<Employee> sourceListBoxCollection = (ObservableCollection<Employee>)source.ItemsSource;
            ObservableCollection<Employee> targetListBoxCollection = (ObservableCollection<Employee>)target.ItemsSource;
            RemoveFromCollection(sourceListBoxCollection, externalSelectedEmployees);
            InsertCollection(externalSelectedEmployees, targetListBoxCollection, indexUnderMouse);
            ProcessSelectionHighlighting(source, target, externalSelectedEmployees);                
        }

        private void ProcessSelectionHighlighting(ListBox source, ListBox target, List<Employee> highlightCollection)
        {
            source.SelectedItems.Clear();
            target.SelectedItems.Clear();           
            target.Focus();
            foreach (Employee item in highlightCollection)
                target.SelectedItems.Add(item);
        }


        void RemoveFromCollection(ObservableCollection<Employee> source, List<Employee> removable)
        {
            foreach (Employee item in removable)
            {
                source.Remove(item);
            }
        }

        void InsertCollection(List<Employee> source, ObservableCollection<Employee> target, int index)
        {
            if (target.Count <= index)
            {
                index =  target.Count == 0 ? 0 : target.Count - 1;
            }
            foreach (Employee item in source)
            {
                target.Insert(index++, item);
            }
        }

        ListBoxItem GetListViewItem(ListBox source, int index)
        {
            if (source.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;

            return source.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;
        }

        int GetCurrentIndex(ListBox source, GetPositionDelegate getPosition)
        {
            int index = -1;
            for (int i = 0; i < source.Items.Count; ++i)
            {
                ListBoxItem item = GetListViewItem(source, i);
                if (this.IsMouseOverTarget(item, getPosition))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        bool IsMouseOverTarget(Visual target, GetPositionDelegate getPosition)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            Point mousePos = getPosition((IInputElement)target);
            return bounds.Contains(mousePos);
        }

        private void OnAllEmployeesListBoxDrop(object sender, DragEventArgs e)
        {
            DoDrop(sender, e);  
        }

        private void OnSelectedEmployeesListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DoDrag(sender, e);
        }
    }
}