using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Core.Employees;

namespace Presentation.Views.Employees
{
    /// <summary>
    ///     Interaction logic for EmployeeManagementWindow.xaml
    /// </summary>
    public partial class EmployeeManagementWindow
    {
        private const string DefaultText = "Enter your text here..";
        private Employee _selectedEmployee;

        public EmployeeManagementWindow()
        {
            EmployeeSource = new ObservableCollection<Employee>(Employee.GetEmployees());
            InitializeComponent();
        }

        public ObservableCollection<Employee> EmployeeSource { get; set; }

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                refreshTextBoxes();
            }
        }

        #region Button Operations

        private void DiscardButton_Click(object sender, RoutedEventArgs e) // revert to original employee information
        {
            if (SelectedEmployee != null)
            {
                var employeeId = SelectedEmployee.Id;
                SelectedEmployee = Employee.GetEmployeeById(employeeId);
                ReloadDataGrid();
            }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContactInfoTextBox.Text == "")
            {
                MessageBox.Show("You cannot have empty contact information");
                return;
            }
            bool wereAllUpdated = true;
            foreach (Employee employee in EmployeeDataGrid.Items)
            {
                if (!Employee.UpdateEmployee(employee))
                {
                    wereAllUpdated = false;
                }
            }
            MessageBox.Show(wereAllUpdated ? "All Employees updated successfully" : "Not all updates were successful");
            ReloadDataGrid();
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new AddEmployeeWindow().ShowDialog();
            ReloadDataGrid();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(this, "Are You Sure You Want To Remove?", "Confirmation", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            MessageBox.Show(Employee.DeleteEmployee(SelectedEmployee) // If delete was successful
                ? "Successfully Removed"
                : "Could not complete the action");
            ReloadDataGrid();
        }

        private void PaymentsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new EmployeePaymentWindow().ShowDialog();
        }

        #endregion

        #region UI Code Behind

        #region TextBox Updates

        private void refreshTextBoxes()
        {
            EmployeeIdTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            NameTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            ContactInfoTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            JoinedOnTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            ActiveCheckBox.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateTarget();
        }

        #endregion

        #region EmployeeDataGrid Updates

        private void ReloadDataGrid()
        {
            var text = SearchTextBox.Text == DefaultText ? "" : SearchTextBox.Text;
            var selectionId = EmployeeDataGrid.SelectedIndex;
            EmployeeSource.Clear();
            foreach (var employee in Employee.GetEmployees(name: text))
            {
                EmployeeSource.Add(employee);
            }
            EmployeeDataGrid.SelectedIndex = EmployeeDataGrid.Items.Count > selectionId
                ? selectionId
                : EmployeeDataGrid.Items.Count - 1;
        }

        #endregion

        #region Window Keydown Handling

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (EmployeeDataGrid.SelectedIndex < EmployeeDataGrid.Items.Count - 1)
                        EmployeeDataGrid.SelectedIndex++;
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (EmployeeDataGrid.SelectedIndex > 0) EmployeeDataGrid.SelectedIndex--;
                    e.Handled = true;
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
        }

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ReloadDataGrid();
        }

        #endregion

        #endregion

        private void ManageUserAccessButton_OnClick(object sender, RoutedEventArgs e)
        {
            var employee = EmployeeDataGrid.SelectedItem as Employee;
            if (employee == null) return;
            new EditUserAccess(employee).ShowDialog();
            ReloadDataGrid();
        }
    }
}