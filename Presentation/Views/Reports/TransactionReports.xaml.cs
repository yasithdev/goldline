using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Presentation.Reporting;

namespace Presentation.Views.Reports
{
    /// <summary>
    ///     Interaction logic for TransactionReports.xaml
    /// </summary>
    public partial class TransactionReports : Window
    {
        private readonly List<string> _itemSource =
            new List<string>(new[] {"Cash Orders", "Credit Orders", "Supply Orders", "Employee Payments", "CustomerPayments"});

        //private DateTime _end = DateTime.Today.AddDays(1);
        private DateTime _end = DateTime.Today;
        private DateTime _start = DateTime.MinValue;

        private DateTime _selectedStartDate;
        private DateTime _selectedEndDate;

       // private DateTime _start = DateTime.Today;
        private string _typeOfReport;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public TransactionReports()
        {
            try
            {
                InitializeComponent();
                ResetDatePickers();

                ComboBox.ItemsSource = _itemSource;
                _typeOfReport = ComboBox.SelectedItem.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open the window");
                log.Error("Could not open the Transaction window :"+ex.Message);
            }
            //Debug.WriteLine(_typeOfReport);
        }

        private void ResetDatePickers()
        {
            StartDatePicker.SelectedDate = _start;
            endDatePicker.SelectedDate = _end;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _typeOfReport = ComboBox.SelectedItem.ToString();
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
                _selectedStartDate = StartDatePicker.SelectedDate.Value;
                Debug.WriteLine(_selectedStartDate);
        }

        private void endDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedEndDate = endDatePicker.SelectedDate.Value;
            Debug.WriteLine(_selectedEndDate);
        }
        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedStartDate < _selectedEndDate || _selectedStartDate == _selectedEndDate)
            {
                try {
                    if (_typeOfReport.Equals("Credit Orders"))
                    {
                        var cr = new CreditOrderReport(_selectedStartDate, _selectedEndDate);
                        cr.ShowDialog();
                    }
                    else if (_typeOfReport.Equals("Cash Orders"))
                    {
                        var co = new CashOrderReport(_selectedStartDate, _selectedEndDate);
                        co.ShowDialog();
                    }
                    else if (_typeOfReport.Equals("Employee Payments"))
                    {
                        var ep = new EmployeePaymentReport(_selectedStartDate, _selectedEndDate);
                        ep.ShowDialog();
                    }
                    else if (_typeOfReport == "CustomerPayments")
                    {
                        var sr = new CustomerPaymentsReport(_selectedStartDate, _selectedEndDate);
                        sr.ShowDialog();
                    }
                    else if (_typeOfReport == "Supply Orders")
                    {
                        var so = new SupplyOrdersReport(_selectedStartDate, _selectedEndDate);
                        so.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
            else if (_selectedStartDate > _selectedEndDate)
            {
                MessageBox.Show("Selected StartDate is Greater Than the EndDate!");
                log.Debug("StartDate is Greater Than the EndDate");
                return;
            }
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
               
    }
}