using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Inventory;
using Core.Suppliers;

namespace Presentation.Views.Supplies
{
    /// <summary>
    /// Interaction logic for SuppliedItemAdd.xaml
    /// </summary>
    public partial class SuppliedItemAdd : Window
    {
        private const string DefaultItemSearch = "Search Item...";
        private IEnumerable<Item> _itemsSource;
        private Item _selectedItem;
        private readonly int _supplierId;
        public SupplierManagement Observer;

        public SuppliedItemAdd(int supplierId, SupplierManagement observer)
        {
            InitializeComponent();
            Observer = observer;
            _supplierId = supplierId;
            RefreshInventoryDataGrid();
        }

        private void SearchItemTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.Text =
                textBox.Text == DefaultItemSearch
                    ? ""
                    : textBox.Text == "" ? DefaultItemSearch : textBox.Text;
        }

        private void SearchItemTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox.Text != DefaultItemSearch)
            {
                RefreshInventoryDataGrid(textBox.Text);
            }
        }

        private void RefreshInventoryDataGrid(string name = "%")
        {
            _itemsSource = Item.GetItems(name: name);
            InventoryDataGrid.ItemsSource = _itemsSource;
        }

        private void InventoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedItem = ((DataGrid)sender).SelectedItem as Item;
        }


        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void SuppliedItemAdd_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (InventoryDataGrid.SelectedIndex < InventoryDataGrid.Items.Count - 1)
                        InventoryDataGrid.SelectedIndex++;
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (InventoryDataGrid.SelectedIndex > 0) InventoryDataGrid.SelectedIndex--;
                    e.Handled = true;
                    break;
                case Key.Enter:
                    if (_selectedItem == null) return;
                    Supplier.AddSuppliedItem(_supplierId, _selectedItem);
                    Observer.NotifyToRefresh(_supplierId);
                    e.Handled = true;
                    Close();
                    break;
            }
        }
    }
}
