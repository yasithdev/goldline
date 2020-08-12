using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core.Orders;
using Core.Suppliers;

namespace Presentation.Views.Supplies
{
    /// <summary>
    /// Interaction logic for SettleCreditSupplyOrder.xaml
    /// </summary>
    public partial class SettleCreditSupplyOrder : Window
    {
        private Supplier _selectedSupplier;
        private List<SupplyOrder> _selectedOrders;
        public SettleCreditSupplyOrder()
        {
            InitializeComponent();
            _selectedOrders = new List<SupplyOrder>();
        }

        public Supplier SelectedSupplier
        {
            get { return _selectedSupplier; }
            set
            {
                _selectedSupplier = value;
                RefreshDataGrid();
                RefreshTextBox();
            }
        }

        #region Button Operations

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrders.Any())
            {
                var result = MessageBox.Show("Confirm payment", "Confirm", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Cancel) return;
                if (SupplyOrder.PaySupplyOrders(_selectedOrders))
                {
                    MessageBox.Show("Successfully Updated!!", "Information", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    RefreshDataGrid();
                }
                else
                {
                    MessageBox.Show("Not Updated", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please Select supply orders", "Nothing Selected", MessageBoxButton.OK,
                        MessageBoxImage.Information);
            }
        }

        #endregion

        #region UI Code Behind

        #region TextBox Updates

        private void RefreshTextBox()
        {
            if (_selectedSupplier == null) return;
            SupplierIdTextBox.Text = _selectedSupplier.Id.ToString();
            NameTextBox.Text = _selectedSupplier.Name;
        }

        #endregion

        #region SupplierDataGrid Updates

        private void RefreshDataGrid()
        {
            if (_selectedSupplier == null) return;
            SupplyOrdersDataGrid.ItemsSource = SupplyOrder.GetDueSupplyOrders(_selectedSupplier.Id);
        }

        #endregion

        #endregion

        private void SupplierDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            decimal total = 0;
            foreach (SupplyOrder order in ((DataGrid) sender).SelectedItems)
            {
                _selectedOrders.Add(order);
                total += order.Total;
            }
            AmountTextBox.Text = total.ToString();
        }
    }
}
