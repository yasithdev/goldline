using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Employees.Users;

namespace Presentation.Views.Employees
{
    /// <summary>
    ///     Interaction logic for AuthenticationWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow()
        {
            InitializeComponent();
        }

        private void UsernameBox_OnFocusChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text =
                textBox.Text == "username"
                    ? ""
                    : textBox.Text == "" ? "username" : textBox.Text;
        }

        private void PasswordBox_OnFocusChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            passwordBox.Password =
                passwordBox.Password == "password"
                    ? ""
                    : passwordBox.Password == "" ? "password" : passwordBox.Password;
        }

        private void Grid_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            // Dragging window from the titlebar
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void AuthenticateButton_Click(object sender, RoutedEventArgs e)
        {
            var user = User.TryLogin(UsernameTextBox.Text, PasswordBox.Password);
            if (user != null)
            {
                if (user is Manager) DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Not Authorized", "Error", MessageBoxButton.OK);
            }
        }
    }
}