using System.Windows;
using Core.Customers;

namespace Presentation.Views.Customers
{
    /// <summary>
    ///     Interaction logic for AddCustomerControl.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        public AddCustomerWindow()
        {
            InitializeComponent();
            CustomerIdTextBox.Text = Customer.GetNextCustomerId().ToString();
            
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == "" || ContactInfoTextBox.Text == "" || !IsNicValid())
            {
                MessageBox.Show("Please make sure your inputs are valid");
                return;
            }
            MessageBox.Show(Customer.AddCustomer(new Customer(
                int.Parse(CustomerIdTextBox.Text),
                NameTextBox.Text,
                ContactInfoTextBox.Text,
                NicTextBox.Text,
                decimal.Parse(DueTextBox.Text)))
                ? "Customer Added Successfully"
                : "Could not complete the operation");
            Close();
        }

        private bool IsNicValid()
        {
            var text = NicTextBox.Text;
            int n;
            int maxIndex = text.Length - 1;
            return( text.Length == 10 && int.TryParse(text.Substring(1,maxIndex-1),out n) && (text.Substring(maxIndex).ToUpper() == "V" || text.Substring(maxIndex).ToUpper() ==     "X"));
        }

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = "";
            ContactInfoTextBox.Text = "";
            NicTextBox.Text = "";
            DueTextBox.Text="";
            
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}