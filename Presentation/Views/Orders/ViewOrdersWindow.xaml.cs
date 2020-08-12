using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core.Orders;

namespace Presentation.Views.Orders
{
    /// <summary>
    /// Interaction logic for ReverseOrder.xaml
    /// </summary>
    public partial class ViewOrdersWindow : Window
    {
        private const string DefaultSearchText = "Enter order date here";
        private Order _currentOrder;
        private Order _reverseOrder;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ViewOrdersWindow()
        {
            try {
                InitializeComponent();
                OrdersDatagrid.ItemsSource = Order.GetOrders();
            }
            catch (Exception ex)
            {
                log.Error("could not open reverse order window :"+ex.Message);
            }
        }

        private void OrdersDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentOrder = OrdersDatagrid.SelectedItem as Order;
            if (_currentOrder == null) return;
            OrderEntriesDataGrid.ItemsSource = _currentOrder.OrderEntries.ToArray();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Text != DefaultSearchText)
            {
                //check
                OrdersDatagrid.ItemsSource=Order.GetOrders(dateTime: textBox?.Text);
                 
             }
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            int orderId = Order.GetNextOrderEntryId();
            if (OrdersDatagrid.SelectedItem == null)
            {
                MessageBox.Show("Please Select an order.");
                return;
            }
            string preId = _currentOrder.OrderId.ToString();
            string newNote = "reversed order :" + preId;
            if (CheckReversability(newNote))
            {
                Console.WriteLine("reversed already :"+IsAlreadyReversed());
                if (!IsAlreadyReversed())
                {
                    foreach (OrderEntry entry in _currentOrder.OrderEntries)
                    {
                        entry.Quantity = -entry.Quantity;
                        entry.OrderId = orderId;
                    }
                    _reverseOrder = new Order(orderId, 2/*App.CurrentUser.Id*/, _currentOrder.OrderEntries, newNote, System.DateTime.Now);
                } else
                {
                    MessageBox.Show("Already Reversed Order Selected");
                    OrdersDatagrid.SelectedItem = null;
                    _reverseOrder = null;
                    return;
                }
            }
            else
            {
                OrdersDatagrid.SelectedItem = null;
                MessageBox.Show("Attempting to reverse an order which has alredy reversed");
                return;
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDatagrid.SelectedItem!=null)
            {
                if (_reverseOrder == null)
                {
                    MessageBox.Show("Please Verify the order");
                    return;
                }
                Console.WriteLine(_reverseOrder.OrderId+" tot "+_reverseOrder.Total);
                Order.AddOrder(_reverseOrder);
                MessageBox.Show("Successfully Reversed the order :"+_currentOrder.OrderId);
                OrdersDatagrid.ItemsSource = Order.GetOrders();
            } else
            {
                MessageBox.Show("Please select a valid order to proceed with..");
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public bool CheckReversability(string note)
        {
            if (Order.GetOrders().ToArray().Length == 0) return false;
            foreach (Order od in Order.GetOrders().ToArray())
            {
                if (od.Note.Equals(note))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsAlreadyReversed()
        {
            if (_currentOrder.Note.StartsWith("reversed"))
            {
                return true;
            }
            return false;
        }
    }
}
