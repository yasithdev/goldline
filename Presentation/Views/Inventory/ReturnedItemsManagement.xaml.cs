using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Inventory.Returns;

namespace Presentation.Views.Inventory
{
    /// <summary>
    ///     Interaction logic for ReturnedItemsManagement.xaml
    /// </summary>
    public partial class ReturnedItemsManagement : Window
    {
        private readonly string _defaultText = "Enter your text here..";
        private readonly string[] comboBoxOptions = {"View All", "Pending", "Accepted", "Rejected", "Completed"};
        private IEnumerable<ReturnedItem> _returnedItemSource;
        private ReturnedItem _selectedReturnedItem;
        private string condition;

        public ReturnedItemsManagement()
        {
            InitializeComponent();
            comboBox.ItemsSource = comboBoxOptions;
            comboBox.SelectedIndex = 1;
        }

        public void RefreshDataGrid(string text = "%", string condition = "%")
        {
            _returnedItemSource = condition == "All" 
                ? ReturnedItem.GetreturnedItems(itemName: text) 
                : ReturnedItem.GetreturnedItems(itemName: text, condition: condition);

            InventoryDataGrid.ItemsSource = _returnedItemSource;
            _selectedReturnedItem = null;
        }

        #region Action Listeners

        private void InventoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedReturnedItem = ((DataGrid) sender).SelectedItem as ReturnedItem;
        }

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Text != _defaultText)
            {
                RefreshDataGrid(SearchTextBox.Text, condition);
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

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            condition = comboBox.SelectedValue.ToString();
            RefreshDataGrid(condition: condition);
            SearchTextBox.Text = _defaultText;
        }

        #endregion

        #region Button_Click

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewReturnedItem newWindow = new AddNewReturnedItem();
            newWindow.AddObserver(this);
            newWindow.Show();
        }

        private void RejectedButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedReturnedItem == null) return;
            _selectedReturnedItem.Condition = "Rejected";
            ReturnedItem.UpdateReturnedItem(_selectedReturnedItem);
            RefreshDataGrid();
        }

        private void AcceptedButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedReturnedItem == null) return;
            _selectedReturnedItem.Condition = "Accepted";
            ReturnedItem.UpdateReturnedItem(_selectedReturnedItem);
            RefreshDataGrid();
        }

        private void CompletedButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedReturnedItem == null) return;
            _selectedReturnedItem.Condition = "Completed";
            ReturnedItem.UpdateReturnedItem(_selectedReturnedItem);
            RefreshDataGrid();
        }

        #endregion

        private void ReturnedItemsManagement_OnPreviewKeyDown(object sender, KeyEventArgs e)
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
    }
}