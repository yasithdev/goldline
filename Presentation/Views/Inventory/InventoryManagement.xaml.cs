using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Inventory;

namespace Presentation.Views.Inventory
{
    /// <summary>
    ///     Interaction logic for InventoryManagement.xaml
    /// </summary>
    public partial class InventoryManagement : Window
    {
        private readonly string _defaultText = "Enter your text here..";
        private readonly string _defaultType = "Tyre";
        private IEnumerable<Item> _itemSource;
        private Item _selectedItem;
        private string _selectedItemType;

        public InventoryManagement()
        {
            InitializeComponent();
            ItemTypesComboBox.ItemsSource = Item.GetItemTypes();
            _selectedItemType = _defaultType;
            RefreshDataGrid();
        }

        private void CreateNewItemFromData()
        {
            switch (_selectedItemType)
            {
                case "Tyre":
                    _selectedItem = new Tyre(
                        int.Parse(IdTextBox.Text),
                        NameTextBox.Text,
                        BrandTextBox.Text,
                        DimensionTextBox.Text,
                        CountryTextBox.Text,
                        decimal.Parse(PriceTextBox.Text),
                        int.Parse(StockTextBox.Text));
                    break;
                case "Alloy Wheel":
                    _selectedItem = new AlloyWheel(
                        int.Parse(IdTextBox.Text),
                        NameTextBox.Text,
                        int.Parse(StockTextBox.Text),
                        decimal.Parse(PriceTextBox.Text),
                        BrandTextBox.Text,
                        DimensionTextBox.Text);
                    break;
                case "Battery":
                    _selectedItem = new Battery(
                        int.Parse(IdTextBox.Text),
                        NameTextBox.Text,
                        int.Parse(StockTextBox.Text),
                        decimal.Parse(PriceTextBox.Text),
                        BrandTextBox.Text);
                    break;
            }
        }

        private bool AddNewItemInfo()
        {
            switch (_selectedItemType)
            {
                case "Tyre":
                    return Tyre.AddNewTyre((Tyre)_selectedItem);

                case "Alloy Wheel":
                    return AlloyWheel.AddNewAlloyWheel((AlloyWheel)_selectedItem);

                case "Battery":
                    return Battery.AddNewBattery((Battery)_selectedItem);
            }

            return false;

        }

        public void AddNewItem(Item newItem)
        {
            _selectedItem = newItem;
            if (AddNewItemInfo())
            {
                MessageBox.Show("Successfully Updated", "Information", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Duplicate Entry", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            SearchTextBox.Text = _defaultText;
            RefreshDataGrid();
        }

        private bool UpdateItem()
        {
            if (IsDataInCorrectForm())
            {
                CreateNewItemFromData();
                switch (_selectedItemType)
                {
                    case "Tyre":
                        return Tyre.UpdateTyres((Tyre)_selectedItem);

                    case "Alloy Wheel":
                        return AlloyWheel.UpdateAlloyWheel((AlloyWheel)_selectedItem);

                    case "Battery":
                        return Battery.UpdateBattery((Battery)_selectedItem);
                }
            }
            return false;
        }

        #region TextBox Focus Properties

        private void TextBox_OnFocusChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text =
                textBox.Text == _defaultText
                    ? ""
                    : textBox.Text == "" ? _defaultText : textBox.Text;
        }

        #endregion

        #region SearchBar(TextBox and ComboBox and KeyPress)

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Text != _defaultText)
            {
                RefreshDataGrid(name: textBox?.Text);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void ItemTypesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearPropertyBoxes();
            _selectedItemType = ((ComboBox) sender).SelectedValue.ToString();
            switch (_selectedItemType)
            {
                case "Tyre":
                    DimensionLabel.Content = "Dimension:";
                    CountryTextBox.IsEnabled = true;
                    break;
                case "Alloy Wheel":
                    DimensionLabel.Content = "Dimension:";
                    CountryTextBox.IsEnabled = false;
                    CountryTextBox.Text = "";
                    break;
                case "Battery":
                    DimensionLabel.Content = "Other Details:";
                    CountryTextBox.IsEnabled = false;
                    DimensionTextBox.Text = "";
                    CountryTextBox.Text = "";
                    break;
            }

            SearchTextBox.Text = _defaultText;
            _selectedItem = null;
            RefreshDataGrid();
            RefreshEditInfo();
        }

        #endregion

        #region DataGrid

        private void LoadItemSource(string id = "%", string name = "%")
        {
            switch (_selectedItemType)
            {
                case "Tyre":
                    _itemSource = Tyre.GetTyres(id, name);
                    break;
                case "Alloy Wheel":
                    _itemSource = AlloyWheel.GetAlloyWheels(id, name);
                    break;
                case "Battery":
                    _itemSource = Battery.GetBatteries(id, name);
                    break;
            }
        }

        private void RefreshDataGrid(string id = "%", string name = "%")
        {
            LoadItemSource(id, name);
            InventoryDataGrid.ItemsSource = _itemSource;
        }

        private void InventoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedItem = ((DataGrid) sender).SelectedItem as Item;
            RefreshEditInfo();
        }

        #endregion

        #region ItemProperty TextBoxes Text

        private void ClearPropertyBoxes()
        {
            IdTextBox.Text = "";
            BrandTextBox.Text = "";
            StockTextBox.Text = "";
            PriceTextBox.Text = "";
            DimensionTextBox.Text = "";
            CountryTextBox.Text = "";
            NameTextBox.Text = "";
        }

        private bool IsDataInCorrectForm()
        {
            if (IdTextBox.Text == "") return false;
            if (BrandTextBox.Text == "") return false;
            if (PriceTextBox.Text == "") return false;
            if (StockTextBox.Text == "") return false;
            if (DimensionTextBox.Text == "" &&
                (_selectedItemType == "Tyre" || _selectedItemType == "Alloy Wheel")) return false;
            return true;
        }

        private void RefreshEditInfo()
        {
            if (_selectedItem == null) return;

            IdTextBox.Text = _selectedItem.Id.ToString();
            NameTextBox.Text = _selectedItem.Name;
            PriceTextBox.Text = _selectedItem.Price.ToString();
            StockTextBox.Text = _selectedItem.Stock.ToString();

            switch (_selectedItemType)
            {
                case "Tyre":
                    BrandTextBox.Text = ((Tyre) _selectedItem).Brand;
                    DimensionTextBox.Text = ((Tyre) _selectedItem).Dimension;
                    CountryTextBox.Text = ((Tyre) _selectedItem).Country;
                    break;
                case "Alloy Wheel":
                    BrandTextBox.Text = ((AlloyWheel) _selectedItem).Brand;
                    DimensionTextBox.Text = ((AlloyWheel) _selectedItem).Dimension;
                    break;
                case "Battery":
                    BrandTextBox.Text = ((Battery) _selectedItem).Brand;
                    DimensionTextBox.Text =
                        ((Battery) _selectedItem).Name.Replace(((Battery)_selectedItem).Brand,"");
                    break;
            }
        }

        private void PropertyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (_selectedItemType)
            {
                case "Tyre":
                    NameTextBox.Text = Tyre.GenerateName(DimensionTextBox.Text, BrandTextBox.Text, CountryTextBox.Text);
                    break;
                case "Alloy Wheel":
                    NameTextBox.Text = AlloyWheel.GenerateName(DimensionTextBox.Text, BrandTextBox.Text);
                    break;
                case "Battery":
                    NameTextBox.Text = Battery.GenerateName(BrandTextBox.Text,DimensionTextBox.Text);
                    break;
            }
        }

        #endregion

        #region Buttons Click

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            if (IdTextBox.Text == "") return;
            LoadItemSource(IdTextBox.Text);
            _selectedItem = _itemSource.ElementAt(0);
            RefreshEditInfo();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsDataInCorrectForm())
            {
                if (UpdateItem())
                {
                    MessageBox.Show("Successfully Updated", "Information", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Duplicate Entry", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please Check The Entered Data!!", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            SearchTextBox.Text = _defaultText;
            RefreshDataGrid();
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("You are requesting to add a item", "Confirmation",
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                AddNewItem newWindow = new AddNewItem(_selectedItemType,
                    Product.GetNextAvailableId(),this);
                newWindow.ShowDialog();
            }
        }
        #endregion

    }
}