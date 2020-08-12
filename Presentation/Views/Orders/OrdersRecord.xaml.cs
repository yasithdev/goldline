using System;
using System.Windows;
using System.Windows.Controls;
using Core.Orders;

namespace Presentation.Views.Orders
{
    /// <summary>
    /// Interaction logic for OrdersRecord.xaml
    /// </summary>
    public partial class OrdersRecord : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public OrdersRecord()
        {
            try
            {
                InitializeComponent();
                OrdersDatagrid.ItemsSource = Order.GetOrders();
            }
            catch (Exception ex)
            {
                log.Error("could not open reverse order window :" + ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OrdersDatagrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var _currentOrder = OrdersDatagrid.SelectedItem as Order;
            if (_currentOrder == null) return;
            OrderEntriesDataGrid.ItemsSource = _currentOrder.OrderEntries.ToArray();
       

    }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            new ReverseOrder().ShowDialog();
            OrdersDatagrid.ItemsSource = Order.GetOrders();

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Text != "Enter order date here")
            {
               OrdersDatagrid.ItemsSource = Order.GetOrders(dateTime: textBox?.Text);

            }
        }
    }
}
