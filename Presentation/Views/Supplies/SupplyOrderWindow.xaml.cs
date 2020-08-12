using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Inventory;
using Core.Orders;
using Core.Suppliers;

namespace Presentation.Views.Supplies
{
    /// <summary>
    ///     Interaction logic for SupplyOrderWindow.xaml
    /// </summary>
    public partial class SupplyOrderWindow : Window
    {
        private readonly string _defaultText = "Enter Item Name here..";
        private int _orderId;
        private Item _selectedItem;
        private SupplyOrderEntry _selectedOrderEntry;
        private Supplier _selectedSupplier;
        private SupplyOrder _newOrder;
        private ViewSupplyOrders _observer;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SupplyOrderWindow()
        {
            InitializeComponent();
            InitializeNewOrder();
            log.Info("SupplyOrderWindow Started............");
        }

        private void InitializeNewOrder()
        {
            _orderId = SupplyOrder.GetNextOrderId();
            SearchComboBox.ItemsSource = Supplier.GetSuppliers();
            InvoiceIdTextBox.Text = _orderId.ToString();
            TotalAmountTextBox.Text = "";
            CheckBox.IsChecked = false;
            RefreshDataGrid();
            _newOrder = new SupplyOrder(_orderId, 1) {Paid = false};
            RefreshSupplyDataGrid();
        }

        #region NotifyThereturnedItemsManagement
        public void AddObserver(ViewSupplyOrders window)
        {
            _observer = window;
        }

        private void NotifyObservers()
        {
            _observer?.RefreshDataGrid();
        }
        #endregion

        private void AddOrderEntryRefresh()
        {
            NameTextBox.Text = _selectedItem.Name;
            QuantityTextBox.Text = "";
            PriceTextBox.Text = "";
        }

        private void FinalInfoRefresh()
        {
            ContactInfoTextBox.Text = _selectedSupplier.ContactInfo;
        }

        private void ClearOrderEntryTextBox()
        {
            NameTextBox.Text = "";
            QuantityTextBox.Text = "";
            PriceTextBox.Text = "";
        }


        #region DataGrid
        private void RefreshDataGrid(string text = "%")
        {
            if (_selectedSupplier == null)
            {
                InventoryDataGrid.ItemsSource = null;
                return;
            }
            InventoryDataGrid.ItemsSource = Supplier.GetSuppliedItems(_selectedSupplier.Id, text);
        }

        private void InventoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedItem = ((DataGrid)sender).SelectedItem as Item;
            if (_selectedItem == null)
            {
                ClearOrderEntryTextBox();
                return;
            }
            AddOrderEntryRefresh();
        }

        private void RefreshSupplyDataGrid()
        {
            SupplyOrderDataGrid.ItemsSource = _newOrder.OrderEntries.ToArray();
            _selectedOrderEntry = null;
        }

        private void SupplyOrderDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedOrderEntry = ((DataGrid)sender).SelectedItem as SupplyOrderEntry;
        }
        #endregion

        #region SearchText

        private void SearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedSupplier = ((ComboBox)sender).SelectedItem as Supplier;
            SearchTextBox.Text = _defaultText;
            RefreshDataGrid();
        }

        private void TextBox_OnFocusChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text =
                textBox.Text == _defaultText
                    ? ""
                    : textBox.Text == "" ? _defaultText : textBox.Text;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Text != _defaultText)
            {
                RefreshDataGrid(textBox?.Text);
            }
        }

        private void SupplyOrderWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
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
            }
        }
        #endregion

        #region ChechOrderItemIsEligile
        private bool IsOrderEligible()
        {
            if (_selectedSupplier == null) return false;
            if (_newOrder.OrderEntries.Count == 0) return false;
            _newOrder.SupplierId = _selectedSupplier.Id;
            _newOrder.SupplierName = _selectedSupplier.Name;
            return true;
        }

        private bool IsAddOrderItemEligible()
        {
            if (_selectedItem == null)
            {
                MessageBox.Show("Please Select a item", "Error", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                return false;
            }
            if (QuantityTextBox.Text == "" || PriceTextBox.Text == "")
            {
                MessageBox.Show("Please Enter Quntity and Price", "Error", MessageBoxButton.OK,
                   MessageBoxImage.Information);
                return false;
            }else if (QuantityTextBox.Text == "0" || PriceTextBox.Text == "0")
            {
                MessageBox.Show("Quntity and price must be greater than 0", "Information", MessageBoxButton.OK,
                   MessageBoxImage.Information);
                return false;
            }
            return true;
        }

        public void SaveOrder()
        {
            if (SupplyOrder.AddSupplyOrder(_newOrder))
            {
                MessageBox.Show("Successfully Added", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                InitializeNewOrder();
                SearchComboBox.Focus();
                NotifyObservers();
            }
            else
            {
                MessageBox.Show("Not Added", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        #endregion

        #region Button__Click

        private void AddToOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsAddOrderItemEligible())
            {
                _selectedOrderEntry = new SupplyOrderEntry(_orderId, _selectedItem.Id, _selectedItem.Name,
                    int.Parse(QuantityTextBox.Text), decimal.Parse(PriceTextBox.Text));
                if (_newOrder.AddEntry(_selectedOrderEntry))
                {
                    RefreshSupplyDataGrid();
                    TotalAmountTextBox.Text = _newOrder.Total.ToString();
                    FinalInfoRefresh();
                }
                else
                {
                    MessageBox.Show("Item Is Already in the Order", "Information", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }

            QuantityTextBox.Focus();
        }

        private void CancelOrderButton_Click(object sender, RoutedEventArgs e)
        {
            _newOrder = new SupplyOrder(_orderId, 1);
            RefreshDataGrid();
            RefreshSupplyDataGrid();
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            //error in checkout button
            _newOrder.DateTime = DateTime.Now;
            _newOrder.Note = NoteTextBox.Text;
            if (IsOrderEligible())
            {
                new SupplyOrderConfirmationWindow(_newOrder, _selectedSupplier, this).ShowDialog();
            }else
            {
                MessageBox.Show("Check Order And Retry!!", "Information", MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
            }
        }

        private void RemoveEntryButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrderEntry == null) return;
            _newOrder.RemoveEntry(_selectedOrderEntry);
            RefreshSupplyDataGrid();
        }

        private void CheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            _newOrder.Paid = CheckBox.IsChecked == true;
        }

        #endregion

        private void ExpressCheckoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            _newOrder.DateTime = DateTime.Now;
            _newOrder.Note = NoteTextBox.Text;
            if (IsOrderEligible())
            {
                SaveOrder();
            }
            else
            {
                MessageBox.Show("Check Order And Retry!!", "Information", MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
            }
        }
    }
}