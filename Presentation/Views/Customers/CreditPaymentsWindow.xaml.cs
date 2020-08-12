using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Customers;
using log4net;

namespace Presentation.Views.Customers
{
    /// <summary>
    ///     Interaction logic for CreditPaymentsWindow.xaml
    /// </summary>
    public partial class CreditPaymentsWindow : Window
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private CustomerPayment _selectedCustomerPayment;
        private Customer _selectedcuCustomer;

        public CreditPaymentsWindow()
        {
            InitializeComponent();
            CustomerPaymentSource = new ObservableCollection<CustomerPayment>(CustomerPayment.GetCustomerPayments());
            Console.WriteLine(CustomerPaymentSource.Count);
            dataGrid.ItemsSource = CustomerPaymentSource;
            CustomerComboBox.ItemsSource = Customer.GetCustomers();

            
        }
        public ObservableCollection<CustomerPayment> CustomerPaymentSource { get; set; }
        private void InitializeNewCustomer()
        {
            
            CustomerComboBox.ItemsSource = Customer.GetCustomers();
            
            
            
            ReloadDataGrid();
        }

        public CustomerPayment SelectedCustomerPayment
        {
            get { return _selectedCustomerPayment; }
            set
            {
                _selectedCustomerPayment = value;
                refreshTextBoxes();
            }
        }

        #region Button Operations










        #endregion

        #region UI Code Behind

        #region TextBox Updates

        private void refreshTextBoxes()
        {
            DueOutput.Content = _selectedcuCustomer.Dues.ToString();

        }

        #endregion

        #region EmployeeDataGrid Updates

        private void ReloadDataGrid()
        {
           // string text; /*= SearchTextBox.Text == DefaultText ? "" : SearchTextBox.Text;*/
           // var selectionId = dataGrid.SelectedIndex;
            CustomerPaymentSource.Clear();
            foreach (var CustomerPayment in CustomerPayment.GetCustomerPayments(customerId:_selectedcuCustomer.Id.ToString()))
            {
                CustomerPaymentSource.Add(CustomerPayment);
            }
           
        }

        #endregion

        #region Window Keydown Handling

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (dataGrid.SelectedIndex < dataGrid.Items.Count - 1)
                        dataGrid.SelectedIndex++;
                    break;
                case Key.Up:
                    if (dataGrid.SelectedIndex > 0) dataGrid.SelectedIndex--;
                    break;
            }
        }

        #endregion

        #region SearchTextBox Behaviour

     
      

        #endregion

        #endregion

        private void CustomerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedcuCustomer=CustomerComboBox.SelectedItem as Customer;
            ReloadDataGrid();
            refreshTextBoxes();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}