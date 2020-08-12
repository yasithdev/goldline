using System;
using System.Windows;
using System.Windows.Controls;
using Core.Employees;
using Core.Employees.Users;

namespace Presentation.Views.Employees
{
    /// <summary>
    ///     Interaction logic for AddEmployeeControl.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        private bool _isUser;
        private bool? _isUserNameUnique;

        public AddEmployeeWindow()
        {
            InitializeComponent();
            EmployeeIdTextBox.Text = Employee.GetNextEmployeeId().ToString();
            JoinedOnTextBox.Text = DateTime.Now.Date.ToShortDateString();
            UserTypeComboBox.ItemsSource = User.GetUserTypes();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (NameTextBox.Text == "" || ContactInfoTextBox.Text == "" || (_isUser && UserNameTextBox.Text == ""))
            {
                MessageBox.Show("One or more fields are empty");
                return;
            }

            if (_isUser && _isUserNameUnique == null) CheckIfUserNameUnique();

            if (_isUser && _isUserNameUnique == false)
            {
                MessageBox.Show("This username is already taken. Please choose another");
                return;
            }

            #endregion

            if (_isUser)
            {
                User newuser = null;

                switch (UserTypeComboBox.SelectedItem.ToString())
                {
                    case "Manager":
                        newuser = new Manager(int.Parse(EmployeeIdTextBox.Text), NameTextBox.Text,
                            ContactInfoTextBox.Text, DateTime.Parse(JoinedOnTextBox.Text).Date,
                            ActiveCheckBox.IsChecked != null && ActiveCheckBox.IsChecked.Value, UserNameTextBox.Text,
                            App.DefaultPassword);
                        break;
                    case "Inventory Manager":
                        newuser = new InventoryManager(int.Parse(EmployeeIdTextBox.Text), NameTextBox.Text,
                            ContactInfoTextBox.Text, DateTime.Parse(JoinedOnTextBox.Text).Date,
                            ActiveCheckBox.IsChecked != null && ActiveCheckBox.IsChecked.Value, UserNameTextBox.Text,
                            App.DefaultPassword);
                        break;
                    case "Cashier":
                        newuser = new Cashier(int.Parse(EmployeeIdTextBox.Text), NameTextBox.Text,
                            ContactInfoTextBox.Text, DateTime.Parse(JoinedOnTextBox.Text).Date,
                            ActiveCheckBox.IsChecked != null && ActiveCheckBox.IsChecked.Value, UserNameTextBox.Text,
                            App.DefaultPassword);
                        break;
                }
                MessageBox.Show(User.AddUser(newuser)
                    ? "Employee Added Successfully"
                    : "Could not complete the operation");
            }
            else
            {
                MessageBox.Show(Employee.AddEmployee(new Employee(
                int.Parse(EmployeeIdTextBox.Text),
                NameTextBox.Text,
                ContactInfoTextBox.Text,
                DateTime.Parse(JoinedOnTextBox.Text).Date,
                ActiveCheckBox.IsChecked != null && ActiveCheckBox.IsChecked.Value))
                ? "Employee Added Successfully"
                : "Could not complete the operation");
            }
            Close();
        }

        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = "";
            ContactInfoTextBox.Text = "";
            JoinedOnTextBox.Text = DateTime.Now.Date.ToShortDateString();
            _isUser = false;
            _isUserNameUnique = null;
            ProvideUserAccessExpander.IsExpanded = false;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void VerifyButton_OnClick(object sender, RoutedEventArgs e)
        {
            // Verify if username is available
            CheckIfUserNameUnique();
            if (_isUserNameUnique == false) MessageBox.Show("This UserName is already taken. Please choose another");
            if (_isUserNameUnique == true) MessageBox.Show("This username is available!");
        }

        private void CheckIfUserNameUnique()
        {
            _isUserNameUnique = User.IsUserNameAvailable(UserNameTextBox.Text);
        }

        #region ProvideUserAccessExpander Behaviour

        private void ProvideUserAccessExpander_Expanded(object sender, RoutedEventArgs e)
        {
            _isUser = true;
        }

        private void ProvideUserAccessExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            _isUser = false;
        }

        #endregion

        private void UserNameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _isUserNameUnique = null;
        }
    }
}