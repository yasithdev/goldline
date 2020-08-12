using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core.Employees;
using Core.Employees.Users;

namespace Presentation.Views.Employees
{
    /// <summary>
    ///     Interaction logic for AddEmployeeControl.xaml
    /// </summary>
    public partial class EditUserAccess : Window
    {
        private readonly bool _isAlreadyUser;
        private readonly Employee _employee;
        private bool _isExpanded;
        private bool? _isUserNameUnique;

        public EditUserAccess(Employee employee)
        {
            InitializeComponent();
            #region Initializing variables and UI
            _employee = employee;
            EmployeeIdTextBox.Text = _employee.Id.ToString();
            NameTextBox.Text = _employee.Name;

            //If Employee is already a user, make user access expander expanded and disable editing username
            _isAlreadyUser = _employee.IsUser;
            ProvideUserAccessExpander.IsExpanded = _isAlreadyUser;
            UserNameTextBox.IsEnabled = !_isAlreadyUser;
            VerifyButton.IsEnabled = !_isAlreadyUser;
            UserTypeComboBox.ItemsSource = User.GetUserTypes(); 

            if (_isAlreadyUser)
            {
                // Assign the user object to employee
                _employee = User.GetUsers(_employee.Id.ToString()).Single();
                UserNameTextBox.Text = ((User)_employee).Username;
                UserTypeComboBox.SelectedItem = ((User)_employee).TypeName;
            }
            #endregion
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (_isExpanded && UserNameTextBox.Text == "")
            {
                MessageBox.Show("One or more fields are empty");
                return;
            }

            if (_isExpanded && !_isAlreadyUser && _isUserNameUnique == null) CheckIfUserNameUnique();

            if (_isExpanded && !_isAlreadyUser && _isUserNameUnique == false)
            {
                MessageBox.Show("This username is already taken. Please choose another");
                return;
            }

            if (!_isExpanded)
            {
                MessageBox.Show("No changes to be done");
                Close();
            }

            #endregion

            if (!_isAlreadyUser)
            {
                // should add new user to database
                User newuser = null;
                switch (UserTypeComboBox.SelectedItem.ToString())
                {
                    case "Manager":
                        newuser = new Manager(_employee.Id, _employee.Name, _employee.ContactInfo, _employee.JoinDateTime, _employee.Active, UserNameTextBox.Text, App.DefaultPassword);
                        break;
                    case "Inventory Manager":
                        newuser = new InventoryManager(_employee.Id, _employee.Name, _employee.ContactInfo, _employee.JoinDateTime, _employee.Active, UserNameTextBox.Text, App.DefaultPassword);
                        break;
                    case "Cashier":
                        newuser = new Cashier(_employee.Id, _employee.Name, _employee.ContactInfo, _employee.JoinDateTime, _employee.Active, UserNameTextBox.Text, App.DefaultPassword);
                        break;
                }
                MessageBox.Show(User.AddUser(newuser)
                    ? "User Access provided Successfully"
                    : "Could not complete the operation");
            }

            else if (_isAlreadyUser)
            {
                // should update existing employee if there are any changes
                var user = _employee as User;
                #region Validation
                if (user?.TypeName == UserTypeComboBox.SelectedItem.ToString())
                {
                    MessageBox.Show("Nothing to change");
                    return;
                }
                #endregion
                if (user != null) user.TypeName = UserTypeComboBox.SelectedItem.ToString();
                MessageBox.Show(User.UpdateUser(user)
                    ? "User access updated successfully"
                    : "Could not complete the operation");
            }
            Close();
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

        private void UserNameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _isUserNameUnique = null;
        }

        #region ProvideUserAccessExpander Behaviour
        private void ProvideUserAccessExpander_Expanded(object sender, RoutedEventArgs e)
        {
            _isExpanded = true;
        }
        private void ProvideUserAccessExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            if (_isAlreadyUser)
            {
                MessageBox.Show("You cannot remove user access for existing users!");
                ((Expander) sender).IsExpanded = true;
                e.Handled = true;
            }
            else
            {
                _isExpanded = false;
            }
        }
        #endregion
    }
}