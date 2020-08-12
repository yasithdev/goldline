using System;
using System.Windows.Forms;

namespace Presentation.Reporting
{
    public partial class CashOrderReport : Form
    {
        private readonly DateTime _begin;
        private readonly DateTime _end;

        public CashOrderReport(DateTime start, DateTime end)
        {
            InitializeComponent();
            _end = end;
            _begin = start;
        }

        private void CashOrderReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'EmployeePaymentDataSet.cashordersview' table. You can move, or remove it, as needed.
            cashordersviewTableAdapter.Fill(EmployeePaymentDataSet.cashordersview, _begin, _end);

            reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
        }
    }
}