using System;
using System.Windows.Forms;

namespace Presentation.Reporting
{
    public partial class EmployeePaymentReport : Form
    {
        private readonly DateTime _begin;
        private readonly DateTime _end;

        public EmployeePaymentReport(DateTime start, DateTime end)
        {
            InitializeComponent();
            _end = end;
            _begin = start;
        }

        private void EmployeePaymentReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'EmployeePaymentDataSet.employeepaymentsview' table. You can move, or remove it, as needed.
            employeepaymentsviewTableAdapter.Fill(EmployeePaymentDataSet.employeepaymentsview, _begin, _end);

            reportViewer1.RefreshReport();
        }
    }
}