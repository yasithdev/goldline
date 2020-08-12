using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Inventory;
using Core.Orders;

namespace Presentation.Views.Orders
{
    /// <summary>
    ///     Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private const string DefaultSearchText = "Enter your text here";
        private readonly List<OrderEntry> _orderEntriesList;
        private bool _creditOrder;
        private int _invoiceNo; 
        private IEnumerable<Item> _itemSource;
        private decimal _netPrice;
        private string _product; //string value to know which type of entry is being processed
        private OrderEntry _selectedEntry;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public OrderWindow()
        {
            try
            {
                InitializeComponent();
                _orderEntriesList = new List<OrderEntry>();
                _itemSource = Tyre.GetTyres();
                ComboBox.ItemsSource = Item.GetItemTypes();
                _product = ComboBox.SelectedItem.ToString();
                SetInvoiceNumber();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show("Exception : " + ex.Message);
            }
        }

        public void SetInvoiceNumber()
        {
            _invoiceNo = Order.GetNextOrderEntryId();
            InvoiceNoLabel.Content = _invoiceNo;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _product = ComboBox.SelectedItem.ToString(); //assign the name of the item type 

            if (_product.Equals("Alloy Wheel"))
            {
                _itemSource = AlloyWheel.GetAlloyWheels();
            }

            else if (_product.Equals("Battery"))
            {
                _itemSource = Battery.GetBatteries();
            }

            else if (_product.Equals("Tyre"))
            {
                _itemSource = Tyre.GetTyres();
            }

            RefreshSearchDataGrid();
        }

        private void RefreshSearchDataGrid()
        {
            var selectionId = DataGridSearch.SelectedIndex;
            DataGridSearch.ItemsSource = _itemSource;
            DataGridSearch.SelectedIndex = DataGridSearch.Items.Count > selectionId
                ? selectionId
                : DataGridSearch.Items.Count - 1;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region Exception Handling

                if (QuantityTextBox.Text == "" || DiscountTextBox.Text == "")
                {
                    MessageBox.Show("Some inputs are empty");
                    log.Debug("Quantity TextBox or Discount TextBox is Empty");
                    return;
                }
                else if (int.Parse(QuantityTextBox.Text) <= 0)
                {
                    log.Debug("Qauntity is less than or equal to zero");
                    MessageBox.Show("Quantity is not valid");
                    return;
                }
                else if (int.Parse(DiscountTextBox.Text) < 0)
                {
                    log.Debug("Discount is negative ");
                    MessageBox.Show("Discount is negative");
                    return;
                }
                else if (int.Parse(DiscountTextBox.Text) >= 100)
                {
                    log.Debug("Discount greater than 100");
                    MessageBox.Show("Discount is more than 100%");
                    return;
                }

                #endregion

                var quantity = int.Parse(QuantityTextBox.Text);
                var discount = int.Parse(DiscountTextBox.Text);

                if (quantity != 0 || DataGridSearch.SelectedItem != null)
                {
                    var createdItem = DataGridSearch.SelectedItem as Item;
                    if (quantity > createdItem.Stock)
                    {
                        MessageBox.Show("Not enough items in stock to fullfil your requirement", "Not Enough Stock");
                        log.Debug("Entered quantity is greater than available items");
                        return;
                    }
                    if (!IsAlreadyEntered(createdItem.Id))
                    {
                        var orderEntry = new OrderEntry(_invoiceNo, createdItem.Id, createdItem.Name, quantity,
                            createdItem.Price, discount);
                        _orderEntriesList.Add(orderEntry); // add items to the order entries list
                        _netPrice += orderEntry.NetPrice;
                        NetPriceValueLabel.Content = _netPrice.ToString();
                    }
                    else
                    {
                        log.Debug("Attempted to enter same item twice");
                        MessageBox.Show("This entry is already available");
                    }
                }
                else
                {
                    log.Debug("Add button clicked without selecting an item");
                    MessageBox.Show("Please select an item!", "Item not selected");
                }

                RefreshOrderEntriesDataGrid();
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
                MessageBox.Show("Invalid input", "Invalid Input");
            }
            finally
            {
                DataGridSearch.SelectedItem = null;
                QuantityTextBox.Text = "";
                DiscountTextBox.Text = "";
            }
        }

        public bool IsAlreadyEntered(int id)
        {
            if (_orderEntriesList.Count == 0) return false;

            foreach (var ord in _orderEntriesList)
            {
                if (ord.ProductId == id)
                {
                    return true;
                }
            }
            return false;

        }
        public void RefreshOrderEntriesDataGrid()
        {
            OrderEntriesDataGrid.ItemsSource = _orderEntriesList.ToArray();
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            Order currentOrder;
            //update stocks accordingly
            try
            {
                Console.WriteLine(_orderEntriesList.Count);
                if (_orderEntriesList.Count > 0)
                {
                    #region Updating stocks
                    //foreach (var odrEntry in _orderEntriesList)
                    //{
                    //    if (Tyre.GetTyres(odrEntry.ProductId.ToString()).Count() == 1)
                    //    {
                    //        Item.UpdateStockDetails(odrEntry.ProductId,
                    //            Tyre.GetTyres(odrEntry.ProductId.ToString()).ElementAt(0).Stock - odrEntry.Quantity);
                    //    }
                    //    else if (Battery.GetBatteries(odrEntry.ProductId.ToString()).Count() == 1)
                    //    {
                    //        Item.UpdateStockDetails(odrEntry.ProductId,
                    //            Battery.GetBatteries(odrEntry.ProductId.ToString()).ElementAt(0).Stock -
                    //            odrEntry.Quantity);
                    //    }
                    //    else if (AlloyWheel.GetAlloyWheels(odrEntry.ProductId.ToString()).Count() == 1)
                    //    {
                    //        Item.UpdateStockDetails(odrEntry.ProductId,
                    //            AlloyWheel.GetAlloyWheels(odrEntry.ProductId.ToString()).ElementAt(0).Stock -
                    //            odrEntry.Quantity);
                    //    }
                    //} 
                    #endregion

                    currentOrder = new Order(_invoiceNo, /*MainWindow.CurrentUser.Id */1, _orderEntriesList,
                        CustomerNameTextBox.Text, DateTime.Now);

                    if (_creditOrder)
                    {
                        if (MessageBox.Show(this, "Do you want to proceed with Credit Order?", "Confirmation",
                            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            try
                            {
                                var verificWindow = new Customers.CreditCustomerVerificationWindow(CustomerNameTextBox.Text);
                                verificWindow.ShowDialog();

                                if (verificWindow.DialogResult == true)
                                {
                                    // MessageBox.Show(verificWindow.CustomerId.ToString());
                                    // MessageBox.Show(verificWindow.CustomerName

                                    Order.AddOrder(currentOrder); //add order to the DB & order Entries to DB
                                    CreditOrder.AddCreditOrder(_invoiceNo, verificWindow.CustomerId);

                                    // Customer.UpdateDues(verificWindow.CustomerId, _currentOrder.Total);

                                    MessageBox.Show(
                                        "Successfully Created The Order. Customer Name :" + verificWindow.CustomerName,
                                        "Credit Order Status");
                                    Close();
                                }
                                else
                                {
                                    CreditCheckBox.IsChecked = false;
                                    _creditOrder = false;
                                    RefreshOrderEntriesDataGrid();
                                }


                            }
                            catch (Exception ex)
                            {
                                log.Debug(ex.Message);
                                MessageBox.Show("Customer Verification Failed");
                            }
                        }

                    }
                    else if  (MessageBox.Show(this, "Do you want to proceed with Cash Order?", "Confirmation",
                             MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        
                    {   
                        Order.AddOrder(currentOrder);
                        MessageBox.Show("Successfully added the cash order.","Order Status");
                        Close();
                    }
                }
                else
                {
                    log.Debug("Proceed button click without selecting an item");
                    MessageBox.Show("Select Items to proceed!", "Empty order");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("exception in proceed button click"+ex.Message);
                log.Debug(ex.Message);
            }
            
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Text != DefaultSearchText)
            {
                if (_product.Equals("Tyre"))
                {
                    _itemSource = Tyre.GetTyres(name: textBox?.Text);
                    DataGridSearch.ItemsSource = _itemSource;
                }
                else if (_product.Equals("Alloy Wheel"))
                {
                    _itemSource = AlloyWheel.GetAlloyWheels(name: textBox?.Text);
                    DataGridSearch.ItemsSource = _itemSource;
                }
                else if (_product.Equals("Battery"))
                {
                    _itemSource = Battery.GetBatteries(name: textBox?.Text);
                    DataGridSearch.ItemsSource = _itemSource;
                }
            }
        }

        private void SearchTextBox_OnFocusChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
                textBox.Text =
                    textBox.Text == DefaultSearchText
                        ? ""
                        : textBox.Text == "" ? DefaultSearchText : textBox.Text;
        }

        private void CreditCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _creditOrder = true;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEntry != null)
            {
                _orderEntriesList.Remove(_selectedEntry);
                _netPrice = _netPrice - _selectedEntry.NetPrice;
                NetPriceValueLabel.Content = _netPrice.ToString();
                RefreshOrderEntriesDataGrid();
            }
            else
            {
                log.Debug("items not selected to remove");
                MessageBox.Show("Please select an item to remove");
            }
        }

        private void OrderEntriesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedEntry = OrderEntriesDataGrid.SelectedItem as OrderEntry;
        }

        private void ServiceButton_Click_1(object sender, RoutedEventArgs e)
        {
            var serviceOrder = new ServiceOrder();
            serviceOrder.ShowDialog();
            if (serviceOrder.DialogResult != null && serviceOrder.DialogResult.Value)
            {
                var service = serviceOrder.SelectedService;
                var servicePrice = serviceOrder.SpecifiedPrice;
                
                //get items from service order window as order entries
                if (!IsAlreadyEntered(service.Id))
                {
                    _orderEntriesList.Add(new OrderEntry(_invoiceNo, service.Id, service.Name, 1, servicePrice, 0));
                    _netPrice += servicePrice;
                    NetPriceValueLabel.Content = _netPrice.ToString();
                    RefreshOrderEntriesDataGrid();
                }
                else
                {
                    log.Debug("Service attempted to add twice");
                    MessageBox.Show("Duplicate Entry in the order");
                }
            }

        }

        #region Window Keydown Handling

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (DataGridSearch.SelectedIndex < DataGridSearch.Items.Count - 1)
                        DataGridSearch.SelectedIndex++;
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (DataGridSearch.SelectedIndex > 0) DataGridSearch.SelectedIndex--;
                    e.Handled = true;
                    break;
            }
        }

        #endregion
    }
}