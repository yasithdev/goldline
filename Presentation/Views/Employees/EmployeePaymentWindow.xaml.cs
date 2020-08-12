using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Core.Employees;

namespace Presentation.Views.Employees
{
    /// <summary>
    ///     Interaction logic for EmployeePaymentWindow.xaml
    /// </summary>
    public partial class EmployeePaymentWindow
    {
        private const string DefaultText = "Enter your text here..";
        private Employee _selectedEmployee;

        public EmployeePaymentWindow()
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
                RefreshTextBoxes();
            }
        }

        #region Button Operations

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(EmpPayment.AddEmpPayment(new EmpPayment(EmpPayment.GetNextEmployeePaymentId(),
                int.Parse(EmployeeIdTextBox.Text),decimal.Parse(AmountTextBox.Text),
                DateTime.Now.Date,ReasonTextBox.Text)) 
                ? "Payment was added successfully!" 
                : "Something happened and we could not complete the action");
            ReloadDataGrid();
        }

        private void ReversePaymentButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadDataGrid();
            throw new NotImplementedException("Should undo the last made payment with authorization");
        }

        private void SummaryButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = EmployeeDataGrid.SelectedItem as Employee;
            new Reports.TransactionReports().Show();
        }

        #endregion

        #region UI Code Behind

        #region TextBox Updates

        private void RefreshTextBoxes()
        {
            EmployeeIdTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            NameTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            LastPaymentDateTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
        }

        #endregion

        #region EmployeeDataGrid Updates

        private void ReloadDataGrid()
        {
            var selectedIndex = EmployeeDataGrid.SelectedIndex;
            EmployeeSource.Clear();
            foreach (var employee in Employee.GetEmployees())
            {
                EmployeeSource.Add(employee);
            }
            EmployeeDataGrid.SelectedIndex = selectedIndex;
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

        #endregion

        private void EmployeePaymentEntriesDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            ((DataGrid)sender).ItemsSource = EmpPayment.GetEmpPayments(employeeId: _selectedEmployee.Id.ToString());
        }

        #region Expander Behaviour
        private void Expander_OnAction(object sender, RoutedEventArgs e)
        {
            var expander = (Expander)sender;

            for (var visual = (Visual)sender; visual != null; visual = (Visual)VisualTreeHelper.GetParent(visual))
            {
                var gridRow = visual as DataGridRow;
                if (gridRow == null) continue;

                _selectedEmployee = (Employee)gridRow.Item;
                gridRow.IsSelected = true;
                gridRow.DetailsVisibility = expander.IsExpanded ? Visibility.Visible : Visibility.Collapsed;
                e.Handled = true;
                break;
            }
        }
        #endregion
    }
}