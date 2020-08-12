using System;
using System.Windows.Forms;

namespace Presentation.Reporting
{
    public partial class SupplyOrdersReport : Form
    {
        private readonly DateTime _end;
        private readonly DateTime _start;

        public SupplyOrdersReport(DateTime start, DateTime end)
        {
            InitializeComponent();
            _start = start;
            _end = end;
        }

        private void SupplyOrdersReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'EmployeePaymentDataSet.supplyorders' table. You can move, or remove it, as needed.
            supplyordersviewTableAdapter.Fill(EmployeePaymentDataSet.supplyordersview, _start, _end);

            reportViewer1.RefreshReport();
        }
    }
}