using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Customers;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace Presentation.Views.Customers
{
    /// <summary>
    ///     Interaction logic for CustomerManagementWindow.xaml
    /// </summary>
    public partial class CreditCustomerManagement : Window
    {
        private const string DefaultText = "Enter your customer name here..";
        private Customer _selectedCustomer;

        public CreditCustomerManagement()
        {
            CustomerSource = new ObservableCollection<Customer>(Customer.GetCustomers());
            InitializeComponent();
            PayTextBox.Text = "0";
            RefreshButtons();
        }

        public ObservableCollection<Customer> CustomerSource { get; set; }

        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                RefreshTextBoxes();
                RefreshButtons();
            }
        }

        #region Button Operations

        private void DiscardButton_Click(object sender, RoutedEventArgs e) // revert to original Customer information
        {
            var customerId = SelectedCustomer.Id;
            SelectedCustomer = Customer.GetCustomerById(customerId);
            ReloadDataGrid();
        }


        private void PayButton_Click(object sender, RoutedEventArgs e)

        {
            if (PayTextBox.Text == "0" || PayTextBox.Text == "" )
            {
                MessageBox.Show("Please enter a value greater than 0", "GOLDLINE", MessageBoxButton.OK);
            }
            else if(CustomerIdLabel.Content.ToString() == "" || _selectedCustomer == null)
            {
                MessageBox.Show("Please select a customer", "GOLDLINE", MessageBoxButton.OK);
            }
            else
            {
                CustomerPayment cs = new CustomerPayment(CustomerPayment.GetNextCustomerPaymentId(),
                    _selectedCustomer.Id, decimal.Parse(PayTextBox.Text), DateTime.Now, App.CurrentUser.Id);

                MessageBox.Show(CustomerPayment.AddCustomerPayment(cs)
                    ? "Payment was recorded successfully"
                    : "Could not record payment");

                PayTextBox.Text = "";
                ReloadDataGrid();
            }
        }


        #endregion

        #region UI Code Behind

        #region TextBox Updates

        private void RefreshTextBoxes()
        {
            CustomerIdTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            NameTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            ContactInfoTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            NicTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
        }

        private void RefreshButtons()
        {
            PayButton.IsEnabled = _selectedCustomer != null && PayTextBox.Text != "0" && PayTextBox.Text != "";
            UpdateButton.IsEnabled = _selectedCustomer != null;
            DiscardButton.IsEnabled = _selectedCustomer != null;
        }

        #endregion

        #region CustomerDataGrid Updates

        private void ReloadDataGrid()
        {
            var text = SearchTextBox.Text == DefaultText ? "" : SearchTextBox.Text;
            CustomerSource.Clear();
            var selectionId = CustomerDataGrid.SelectedIndex;
            foreach (var customer in Customer.GetCustomers(name: text))
            {
                CustomerSource.Add(customer);
            }
            CustomerDataGrid.SelectedIndex = CustomerDataGrid.Items.Count > selectionId
               ? selectionId
               : CustomerDataGrid.Items.Count - 1;
        }

        #endregion

        #region Window Keydown Handling

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (CustomerDataGrid.SelectedIndex < CustomerDataGrid.Items.Count - 1)
                        CustomerDataGrid.SelectedIndex++;
                    break;
                case Key.Up:
                    if (CustomerDataGrid.SelectedIndex > 0) CustomerDataGrid.SelectedIndex--;
                    break;
            }
        }

        #endregion

        #region SearchTextBox Behaviour

        private void SearchTextBox_OnFocusChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
                textBox.Text =
                    textBox.Text == DefaultText
                        ? ""
                        : textBox.Text == "" ? DefaultText : textBox.Text;
            PayButton.IsEnabled = textBox?.Text != "0" && PayTextBox.Text != "";
        }

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text != DefaultText)
            {
                ReloadDataGrid();
            }
        }

        #endregion

        #endregion

        private void CustomerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCustomer = CustomerDataGrid.SelectedItem as Customer;
        }

        private void Add_customer_Click(object sender, RoutedEventArgs e)
        {
            new AddCustomerWindow().ShowDialog();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #region PayTextBox Behaviour

        private void PayTextBox_OnFocusChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
                textBox.Text =
                    textBox.Text == "0"
                        ? ""
                        : textBox.Text == "" 
                            ? "0" 
                            : textBox.Text;
        }

        private void PayTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text != "0")
            {
                PayButton.IsEnabled = true;
            }
        }

        #endregion

        private void UpdateButton_OnClick(object sender, RoutedEventArgs e)

        {
            if (_selectedCustomer == null)
            {
                MessageBox.Show("Please select a customer", "GOLDLINE", MessageBoxButton.OK);
            }
            else if (NameTextBox.Text == "" || ContactInfoTextBox.Text == "")
            {
                MessageBox.Show("Please enter valid inputs", "GOLDLINE", MessageBoxButton.OK);
            }
            else
            {
                Customer cs = new Customer(int.Parse(CustomerIdTextBox.Text), NameTextBox.Text,
                    ContactInfoTextBox.Text, NicTextBox.Text, _selectedCustomer.Dues);
                Customer.UpdateCustomer(cs);
                MessageBox.Show("Changes updated successfully", "GOLDLINE", MessageBoxButton.OK);

            }
        }
    }
}

