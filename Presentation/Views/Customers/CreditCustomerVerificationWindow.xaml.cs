using System.Windows;
using System.Windows.Controls;
using Core.Customers;

namespace Presentation.Views.Customers
{
    /// <summary>
    ///     Interaction logic for CreditCustomerVerificationWindow.xaml
    /// </summary>
    public partial class CreditCustomerVerificationWindow : Window
    {
       
        private Customer _selectedCustomer;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string CustomerName { get; set; }

      
        public int CustomerId { get; set; } 

        public CreditCustomerVerificationWindow(string customername)
        {
            InitializeComponent();
            CustomerName = customername;
            CustomerSearchTextBox.Text = CustomerName;  
        }

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Text != "")
            {
                CustomerDataGrid.ItemsSource = Customer.GetCustomers(name: textBox?.Text);
            }
        }

        private void CustomerSearchTextBox_OnFocusChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
                textBox.Text =
                    textBox.Text == CustomerName
                        ? ""
                        : textBox.Text == "" ? CustomerName : textBox.Text;
        }

        private void CustomerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCustomer = CustomerDataGrid.SelectedItem as Customer;
            if (_selectedCustomer == null) return;

                CustomerId = _selectedCustomer.Id;
                CustomerName = _selectedCustomer.Name;
                CustomerIdTextBox.Text = CustomerId.ToString();
                NameTextBox.Text = CustomerName;
                ContactInfoTextBox.Text = _selectedCustomer.ContactInfo;
                NicTextBox.Text = _selectedCustomer.Nic;
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCustomer == null)
            {
                MessageBox.Show("Please select the relevant customer");
                DialogResult = false;
            }
            else
            {
                MessageBox.Show("Credit customer verified");
                DialogResult = true;
                Close();
            }

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}