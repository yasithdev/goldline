using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Core.Employees.Users;

namespace Presentation.Views.Employees
{
    /// <summary>
    ///     Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        public bool IsFirstChange;
        private string _oldPassword;
        private string _newPassword;
        public ChangePasswordWindow(bool isFirstChange)
        {
            InitializeComponent();
            // Load default password automatically if password change is for the first time
            IsFirstChange = isFirstChange;

            if (!isFirstChange) return;

            _oldPassword = App.DefaultPassword;
            OldPasswordBox.Password = _oldPassword;
            OldPasswordBox.IsEnabled = false;
        }

        private void Grid_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            // Dragging window from the titlebar
            DragMove();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            // should create a textbox to type old password and set it to _oldPassword. For now use default App.CurrentUser.Password
            if(!IsFirstChange) _oldPassword = OldPasswordBox.Password;

            #region Validation

            if (_oldPassword != App.CurrentUser.Password)
            {
                MessageBox.Show("Please enter your old password correctly");
                return;

            }

            if (PasswordBox.Password == "" || RepeatPasswordBox.Password == "")
            {
                MessageBox.Show("Password fields cannot be empty");
                _newPassword = "";
                return;
            }

            if (PasswordBox.Password != RepeatPasswordBox.Password)
            {
                MessageBox.Show("Passwords do not match. Please try again.");
                _newPassword = "";
                return;
            }

            _newPassword = PasswordBox.Password;

            if (_newPassword == _oldPassword || _newPassword == App.DefaultPassword)
            {
                MessageBox.Show("You cannot use the same password again");
                return;
            }
            #endregion

            var originalUser = App.CurrentUser;

            try
            {
                App.CurrentUser.Password = _newPassword;
                if (!User.UpdateUser(App.CurrentUser))
                {
                    MessageBox.Show("Something happened and we could not change the password");
                    App.CurrentUser = originalUser;
                    DialogResult = false;
                    return;
                }
                MessageBox.Show("Password changed successfully");
                DialogResult = true;
                Close();
            }
            catch
            {
                MessageBox.Show("Something happened and we could not change the password");
                //revert to before changes
                App.CurrentUser = originalUser;
                DialogResult = false;
            }
        }

        private void ChangePasswordWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (!IsFirstChange || (IsFirstChange && DialogResult == true)) return;

            // Else stop closing of window
            MessageBox.Show("Password change is mandatory for security reasons");
            e.Cancel = true;
        }
    }
}