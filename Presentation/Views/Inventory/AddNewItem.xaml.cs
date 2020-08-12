using System.Windows;
using System.Windows.Controls;
using Core.Inventory;

namespace Presentation.Views.Inventory
{
    /// <summary>
    /// Interaction logic for AddNewItem.xaml
    /// </summary>
    public partial class AddNewItem : Window
    {
        private readonly string _selectedItemType;
        private Item _selectedItem;
        private InventoryManagement _observer;
        private int _itemId;

        public AddNewItem(string itemType,int itemId,InventoryManagement newWindow)
        {
            InitializeComponent();
            _selectedItemType = itemType;
            _itemId = itemId;
            _observer = newWindow;
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
            IdTextBox.Text = _itemId.ToString();
        }

        private void CreateNewItemFromData()
        {
            switch (_selectedItemType)
            {
                case "Tyre":
                    _selectedItem = new Tyre(
                        _itemId,
                        NameTextBox.Text,
                        BrandTextBox.Text,
                        DimensionTextBox.Text,
                        CountryTextBox.Text,
                        decimal.Parse(PriceTextBox.Text),
                        int.Parse(StockTextBox.Text));
                    break;
                case "Alloy Wheel":
                    _selectedItem = new AlloyWheel(
                        _itemId,
                        NameTextBox.Text,
                        int.Parse(StockTextBox.Text),
                        decimal.Parse(PriceTextBox.Text),
                        BrandTextBox.Text,
                        DimensionTextBox.Text);
                    break;
                case "Battery":
                    _selectedItem = new Battery(
                        _itemId,
                        NameTextBox.Text,
                        int.Parse(StockTextBox.Text),
                        decimal.Parse(PriceTextBox.Text),
                        BrandTextBox.Text);
                    break;
            }
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
                    NameTextBox.Text = Battery.GenerateName(BrandTextBox.Text, DimensionTextBox.Text);
                    break;
            }
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsDataInCorrectForm())
            {
                CreateNewItemFromData();
                _observer.AddNewItem(_selectedItem);
            }
            else
            {
                MessageBox.Show("Please Check The Entered Data!!", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
            Close();   
        }

        private void DiscardButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
