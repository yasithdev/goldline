using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Inventory;
using Core.Suppliers;
using DataGrid = System.Windows.Controls.DataGrid;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ListBox = System.Windows.Controls.ListBox;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace Presentation.Views.Supplies
{
    /// <summary>
    ///     Interaction logic for SupplierManagement.xaml
    /// </summary>
    public partial class SupplierManagement : Window
    {
        private readonly string _defaultText = "Enter your text here..";
        private Item _selectedSuppliedItem;
        private Supplier _selectedSupplier;
        private IEnumerable<Supplier> _supplierSource;


        public SupplierManagement()
        {
            InitializeComponent();
            RefreshDataGrid();
        }

        private void CreateNewItemFromData()
        {
            _selectedSupplier = new Supplier(int.Parse(IdTextBox.Text), NameTextBox.Text,
                ContactInfoTextBox.Text, null);
        }

        private void ClearPropertyBoxes()
        {
            IdTextBox.Text = "";
            NameTextBox.Text = "";
            ContactInfoTextBox.Text = "";
            listBox.ItemsSource = null;
        }

        private bool IsDataInCorrectForm()
        {
            if (NameTextBox.Text == "") return false;
            return true;
        }

        private void RefreshEditInfo()
        {
            if (_selectedSupplier == null)
            {
                ClearPropertyBoxes();
                return;
            }

            IdTextBox.Text = _selectedSupplier.Id.ToString();
            NameTextBox.Text = _selectedSupplier.Name;
            ContactInfoTextBox.Text = _selectedSupplier.ContactInfo;

            listBox.ItemsSource = _selectedSupplier.SuppliedItems;
            _selectedSuppliedItem = null;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedSuppliedItem = ((ListBox) sender).SelectedItem as Item;
        }

        public void NotifyToRefresh(int supplierId)
        {
            RefreshDataGrid();
            _selectedSupplier = Supplier.GetSuppliers(supplierId.ToString()).ElementAt(0);
            RefreshEditInfo();
        }

        #region SearchTextBox

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Text != _defaultText)
            {
                RefreshDataGrid(SearchTextBox.Text);
            }
        }

        private void TextBox_OnFocusChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text =
                textBox.Text == _defaultText
                    ? ""
                    : textBox.Text == "" ? _defaultText : textBox.Text;
        }

        #endregion

        #region DataGrid

        private void RefreshDataGrid(string text = "%")
        {
            _supplierSource = Supplier.GetSuppliers(name: text);
            SupplierDataGrid.ItemsSource = _supplierSource;
        }

        private void SupplierDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedSupplier = ((DataGrid) sender).SelectedItem as Supplier;
            RefreshEditInfo();
        }

        #endregion

        #region Buttons

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsDataInCorrectForm()) return;
            var result = MessageBox.Show("Tou are requesting to add new supplier", "Confirmation", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            IdTextBox.Text = Supplier.GetNextSupplierId().ToString();
            CreateNewItemFromData();
            if (!Supplier.AddNewSupplier(_selectedSupplier))
            {
                ClearPropertyBoxes();
                MessageBox.Show("Dupplicate entry", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Successfully added", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                RefreshDataGrid();
                listBox.ItemsSource = null;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsDataInCorrectForm())
            {
                CreateNewItemFromData();
                if (Supplier.UpdateSupplier(_selectedSupplier))
                {
                    MessageBox.Show("Successfully Updated!!", "Information", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    RefreshDataGrid();
                }
                else
                {
                    MessageBox.Show("Dupplicate Entry", "Error", MessageBoxButton.OK,
                       MessageBoxImage.Error);
                }
                
            }
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            if (IdTextBox.Text == "") return;
            _selectedSupplier = Supplier.GetSuppliers(IdTextBox.Text).ElementAt(0);
            RefreshEditInfo();
        }

        private void AddPayMent_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedSupplier == null)
            {
                MessageBox.Show("Please select a customer", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }
            SettleCreditSupplyOrder newWindow = new SettleCreditSupplyOrder {SelectedSupplier = _selectedSupplier};
            newWindow.Show();
        }

        private void ItemAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSupplier == null) return;
            new SuppliedItemAdd(_selectedSupplier.Id, this).ShowDialog();
        }

        private void ItemRomoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSuppliedItem == null) return;
            int id = _selectedSupplier.Id;
            Supplier.RemoveSuppliedItem(_selectedSupplier.Id, _selectedSuppliedItem);
            RefreshDataGrid();
            _selectedSupplier = Supplier.GetSuppliers(id.ToString()).ElementAt(0);
            RefreshEditInfo();
        }

        #endregion

        private void SupplierManagement_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (SupplierDataGrid.SelectedIndex < SupplierDataGrid.Items.Count - 1)
                        SupplierDataGrid.SelectedIndex++;
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (SupplierDataGrid.SelectedIndex > 0) SupplierDataGrid.SelectedIndex--;
                    e.Handled = true;
                    break;
            }
        }
    }
}