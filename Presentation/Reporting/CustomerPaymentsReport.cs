using System;
using System.Windows.Forms;

namespace Presentation.Reporting
{
    public partial class CustomerPaymentsReport : Form
    {
        private readonly DateTime _end;
        private readonly DateTime _start;

        public CustomerPaymentsReport(DateTime start, DateTime end)
        {
            InitializeComponent();
            _start = start;
            _end = end;
        }

        private void CustomerPaymentsReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'EmployeePaymentDataSet.CustomerPaymentsview' table. You can move, or remove it, as needed.
            CustomerPaymentsviewTableAdapter.Fill(EmployeePaymentDataSet.CustomerPaymentsview, _start, _end);

            reportViewer1.RefreshReport();
        }
    }
}