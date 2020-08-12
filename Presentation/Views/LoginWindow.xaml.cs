using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Employees.Users;

namespace Presentation.Views
{
    /// <summary>
    ///     Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
        }

        private void UsernameBox_OnFocusChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text =
                textBox.Text == "username"
                    ? ""
                    : textBox.Text == "" ? "username" : textBox.Text;
        }

        private void Grid_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            // Dragging window from the titlebar
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(this,"Are you sure you want to exit?","Confirm",MessageBoxButton.YesNo) == MessageBoxResult.Yes) Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //Try Both the specified password and default password
            var user = User.TryLogin(UsernameTextBox.Text, PasswordBox.Password) ??
                       User.TryLogin(UsernameTextBox.Text, App.DefaultPassword);

            if (user != null)
            {
                App.CurrentUser = user;
                if (user.Password == App.DefaultPassword)
                {
                    //Initial password change
                    new Employees.ChangePasswordWindow(true).ShowDialog();
                }
                new MainWindow().Show();
                Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Error", MessageBoxButton.OK);
                PasswordBox.Password = "";
            }
        }
    }
}