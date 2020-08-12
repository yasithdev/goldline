using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace Presentation.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NameLabel.Content = App.CurrentUser.Name;

            // Create timer object
            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(0.1)};
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Content = DateTime.Now.ToLongTimeString();
        }


        /*
         * Button Operations
         */
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            //Logout Handled on Closing Event
            Close();
        }

        private bool IsLogoutConfirmed()
        {
            return MessageBox.Show(this, "Are you sure you want to logout?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        private void ChangePasswordButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Employees.ChangePasswordWindow(false).ShowDialog();
        }

        private void InventoryManagementButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Inventory.InventoryManagement().Show();
        }

        private void ShowCatalogButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Reports.Catalogs().Show();
        }

        private void SupplierManagementButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Supplies.SupplierManagement().Show();
        }

        private void PlaceSupplyOrderButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Supplies.SupplyOrderWindow().Show();
        }

        private void ViewSupplyOrdersButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Supplies.ViewSupplyOrders().Show();
        }

        private void PlaceOrderButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Orders.OrderWindow().Show();
        }

        private void ViewReturnedItemsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Inventory.ReturnedItemsManagement().Show();
        }

        private void AddItemReturnsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Inventory.AddNewReturnedItem().Show();
        }

        private void ManageCustomersButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Customers.CreditCustomerManagement().Show();
        }

        private void CreditPaymentDetailsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Customers.CreditPaymentsWindow().Show();
        }

        private void SettleCreditBillButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Customers.CreditCustomerManagement().Show();
        }

        private void EmployeeManagementButton_Click(object sender, RoutedEventArgs e)
        {
            new Employees.EmployeeManagementWindow().Show();
        }

        private void EmployeePaymentsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Employees.EmployeePaymentWindow().Show();
        }

        private void TransactionReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Reports.TransactionReports().Show();
        }

        private void ActivityLogButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ViewOrdersButton_OnClick(object sender, RoutedEventArgs e)
        {
            new Orders.ViewOrdersWindow().Show();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (IsLogoutConfirmed()) new LoginWindow().Show();
            else e.Cancel = true;
        }
    }
}