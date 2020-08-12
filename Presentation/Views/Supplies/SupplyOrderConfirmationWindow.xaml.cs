using System.Windows;
using System.Windows.Input;
using Core.Orders;
using Core.Suppliers;

namespace Presentation.Views.Supplies
{
    /// <summary>
    /// Interaction logic for SupplyOrderConfirmationWindow.xaml
    /// </summary>
    public partial class SupplyOrderConfirmationWindow : Window
    {
        private SupplyOrderWindow _observer;

        public SupplyOrderConfirmationWindow(SupplyOrder newOrder, Supplier supplier, SupplyOrderWindow window)
        {
            InitializeComponent();
            OrderEntriesListBox.ItemsSource = newOrder.OrderEntries;
            IdLabel.Content = "SupplyOrder: " + newOrder.Id;
            SupplierNameLabel.Content = supplier.Name;
            ContactLabel.Content = supplier.ContactInfo;
            TotalLabel.Content = newOrder.Total;
            Cash_CreditLabel.Content = newOrder.Paid ? "Cash" : "Credit";
            _observer = window;
            VerifyButton.Focus();
        }

        private void VerifyButton_OnClick(object sender, RoutedEventArgs e)
        {
            _observer.Show();
            Close();
            _observer.SaveOrder();
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Dragging window from the titlebar
            DragMove();
        }
    }
}
