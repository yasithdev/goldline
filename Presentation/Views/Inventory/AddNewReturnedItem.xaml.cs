using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Customers;
using Core.Inventory;
using Core.Inventory.Returns;

namespace Presentation.Views.Inventory
{
    /// <summary>
    ///     Interaction logic for AddNewReturnedItem.xaml
    /// </summary>
    public partial class AddNewReturnedItem : Window
    {
        private readonly string _defaultCustomerSearch = "Search Customer...";
        private readonly string _defaultItemSearch = "Search Item...";

        private IEnumerable<Customer> _customerSource;
        private Item _selectedItem;

        private IEnumerable<Item> _itemsSource;
        private Customer _selectedCustomer;

        private bool _itemFocused = true;
        private ReturnedItemsManagement _observer;

        public AddNewReturnedItem()
        {
            InitializeComponent();
            _itemsSource = Item.GetItems();
            _customerSource = Customer.GetCustomers();
            RefreshInventoryDataGrid();
            RefreshCustomerDataGrid();
        }

        private bool IsDataInCorrectForm()
        {
            if (IdTextBox.Text == "") return false;
            if (ItemNameTextBox.Text == "") return false;
            if (QuantityTextBox.Text == "") return false;
            if (QuantityTextBox.Text == "0") return false;
            return true;
        }

        private ReturnedItem CreateReturnedItemFromData()
        {
            try
            {
                return new ReturnedItem(int.Parse(IdTextBox.Text), App.CurrentUser.Id,
                    _selectedCustomer.Id.ToString(), _selectedItem.Id,
                    App.CurrentUser.Name, CustomerNameTextBox.Text, _selectedItem.Name,
                    int.Parse(QuantityTextBox.Text), DateTime.Now, NotesTextBox.Text,
                    "Pending");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Data);
                return null;
            }

            //create a method to get currently logged in user
        }

        #region NotifyThereturnedItemsManagement
        public void AddObserver(ReturnedItemsManagement window)
        {
            _observer = window;
        }

        private void NotifyObservers()
        {
            if (_observer == null)
            {
                new ReturnedItemsManagement().Show();
            }
            else
            {

                _observer.RefreshDataGrid();
            }
            Close();
        }
        #endregion

        #region RefreshTexBoxData
        private void ClearPropertyBoxes()
        {
            IdTextBox.Text = "";
            ItemNameTextBox.Text = "";
            QuantityTextBox.Text = "";
            NotesTextBox.Text = "";
            CustomerNameTextBox.Text = "";
            CustomerInfoTextBox.Text = "";
            _selectedCustomer = null;
            _selectedItem = null;
        }

        private void RefreshItemName()
        {
            if (_selectedItem == null) return;

            ItemNameTextBox.Text = _selectedItem.Name;
        }

        private void RefreshCustomerInfo()
        {
            if (_selectedCustomer == null) return;

            CustomerNameTextBox.Text = _selectedCustomer.Name;
            CustomerInfoTextBox.Text = _selectedCustomer.ContactInfo;
            CustomerInfoTextBox.IsEnabled = true;
        }
        #endregion

        #region ButtonClick
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            IdTextBox.Text = ReturnedItem.GetNextReturnsId().ToString();
            if (IsDataInCorrectForm())
            {
                ReturnedItem.AddReturnedItem(CreateReturnedItemFromData());
                if (_selectedCustomer == null) return;
                _selectedCustomer.ContactInfo = CustomerInfoTextBox.Text;
                Customer.UpdateCustomer(_selectedCustomer);
                MessageBox.Show("Successfully Added!!", "Information", MessageBoxButton.OK, 
                    MessageBoxImage.Information);
                NotifyObservers();
            }
            else
            {
                MessageBox.Show("Incomplete Data!!", "Information", MessageBoxButton.OK,
                   MessageBoxImage.Information);
            }

        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPropertyBoxes();
            RefreshCustomerDataGrid();
        }

        private void CustomerNameClearButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerNameTextBox.Text = "";
            CustomerInfoTextBox.Text = "";
            CustomerInfoTextBox.IsEnabled = false;
            _selectedCustomer = null;
            RefreshCustomerDataGrid();
        }

        private void CustomerInfoDiscardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCustomer == null) return;
            CustomerInfoTextBox.Text = _selectedCustomer.ContactInfo;
        }

        private void ReturnedItemsManagement_OnClick(object sender, RoutedEventArgs e)
        {
            NotifyObservers();
        }
        #endregion

        #region Search (TextBox & ComboBox & KeyPress)

        #region TextBoxFocus
        private void SearchCustomerTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text =
                textBox.Text == _defaultCustomerSearch
                    ? ""
                    : textBox.Text == "" ? _defaultCustomerSearch : textBox.Text;
            _itemFocused = false;
        }

        private void SearchCustomerTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text =
                textBox.Text == _defaultCustomerSearch
                    ? ""
                    : textBox.Text == "" ? _defaultCustomerSearch : textBox.Text;
        }

        private void SearchItemTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text =
                textBox.Text == _defaultItemSearch
                    ? ""
                    : textBox.Text == "" ? _defaultItemSearch : textBox.Text;
            _itemFocused = true;
        }

        private void SearchItemTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text =
                textBox.Text == _defaultItemSearch
                    ? ""
                    : textBox.Text == "" ? _defaultItemSearch : textBox.Text;
        }
        #endregion

        private void SearchItemTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Text != _defaultItemSearch)
            {
                RefreshInventoryDataGrid(name: textBox?.Text);
            }
        }


        private void SearchCustomerTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Text != _defaultCustomerSearch)
            {
                RefreshCustomerDataGrid(name: textBox?.Text);
            }
        }

        #region KeyScroling
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (_itemFocused)
                    {
                        if (InventoryDataGrid.SelectedIndex < InventoryDataGrid.Items.Count - 1)
                            InventoryDataGrid.SelectedIndex++;
                    }
                    else
                    {
                        if (CustomerDataGrid.SelectedIndex < CustomerDataGrid.Items.Count - 1)
                            CustomerDataGrid.SelectedIndex++;
                    }
                    e.Handled = true;
                    break;

                case Key.Up:
                    if (_itemFocused)
                    {
                        if (InventoryDataGrid.SelectedIndex > 0) InventoryDataGrid.SelectedIndex--;
                    }
                    else
                    {
                        if (CustomerDataGrid.SelectedIndex > 0) CustomerDataGrid.SelectedIndex--;
                    }
                    e.Handled = true;
                    break;
            }
        }
        #endregion

        #endregion

        #region DataGrid

        private void LoadItemSource(int dataGridNo, string id = "%", string name = "%")
        {
            if (dataGridNo == 1)
            {
                _itemsSource = Item.GetItems(id, name);
            }
            else
            {
                _customerSource = Customer.GetCustomers(id, name);
            }
        }

        private void RefreshInventoryDataGrid(string id = "%", string name = "%")
        {
            LoadItemSource(1, id, name);
            InventoryDataGrid.ItemsSource = _itemsSource;
        }

        private void RefreshCustomerDataGrid(string id = "%", string name = "%")
        {
            LoadItemSource(2, id, name);
            CustomerDataGrid.ItemsSource = _customerSource;
        }

        private void InventoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedItem = ((DataGrid) sender).SelectedItem as Item;
            RefreshItemName();
        }

        private void CustomerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCustomer = ((DataGrid)sender).SelectedItem as Customer;
            RefreshCustomerInfo();
        }

        #endregion

    }
}