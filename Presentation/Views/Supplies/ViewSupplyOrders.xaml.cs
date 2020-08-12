using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Core.Orders;

namespace Presentation.Views.Supplies
{
    /// <summary>
    ///     Interaction logic for ViewSupplyOrders.xaml
    /// </summary>
    public partial class ViewSupplyOrders : Window
    {
        private readonly string _defaultText = "Enter Supplier Name..";
        private IEnumerable<SupplyOrder> _supplyOrderSource;
        private bool _isLimited;
        private SupplyOrder _selectedSupplyOrder;

        public ViewSupplyOrders()
        {
            InitializeComponent();
            _isLimited = true;
            RefreshDataGrid();
        }

        public void RefreshDataGrid(string text = "%")
        {
            _supplyOrderSource = SupplyOrder.GetSupplyOrders(_isLimited, supplierName: text);
            SupplyOrdersDataGrid.ItemsSource = _supplyOrderSource;
        }

        #region Action Listeners

        private void SupplyOrdersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedSupplyOrder = ((DataGrid)sender).SelectedItem as SupplyOrder;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
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

        #region Button_Click

        private void ReverseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSupplyOrder == null) return;
            MessageBoxResult result = MessageBox.Show("Confirm Supply Order", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;
            if (SupplyOrder.ReverseSupplyOrder(_selectedSupplyOrder, App.CurrentUser.Id))
            {
                MessageBox.Show("Successfully Updated", "Successfull", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshDataGrid();
            }
            else
            {
                MessageBox.Show("Not Added", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            SupplyOrderWindow newWindow = new SupplyOrderWindow();
            newWindow.AddObserver(this);
            newWindow.Show();
        }

        #endregion

        //public Point GetMousePositionWindowsForms()
        //{
        //    System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
        //    return new Point(point.X, point.Y);
        //}

        private void ViewSupplyOrders_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (SupplyOrdersDataGrid.SelectedIndex < SupplyOrdersDataGrid.Items.Count - 1)
                        SupplyOrdersDataGrid.SelectedIndex++;
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (SupplyOrdersDataGrid.SelectedIndex > 0) SupplyOrdersDataGrid.SelectedIndex--;
                    e.Handled = true;
                    break;
            }
        }

        private void CheckBox_OnCheckChanged(object sender, RoutedEventArgs e)
        {
            _isLimited = CheckBox.IsChecked == false;
            RefreshDataGrid();
        }

        private void SupplyOrderEntriesDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            ((DataGrid)sender).ItemsSource = _selectedSupplyOrder.OrderEntries.ToArray();
        }

        #region Expander Behaviour
        private void Expander_OnAction(object sender, RoutedEventArgs e)
        {
            var expander = (Expander)sender;

            for (var visual = (Visual)sender; visual != null; visual = (Visual)VisualTreeHelper.GetParent(visual))
            {
                var gridRow = visual as DataGridRow;
                if (gridRow == null) continue;

                _selectedSupplyOrder = (SupplyOrder)gridRow.Item;
                gridRow.IsSelected = true;
                gridRow.DetailsVisibility = expander.IsExpanded ? Visibility.Visible : Visibility.Collapsed;
                e.Handled = true;
                break;
            }
        }
        #endregion
    }
}