using System;
using System.Windows.Forms;

namespace Presentation.Reporting
{
    public partial class CreditOrderReport : Form
    {
        private readonly DateTime _beginDate;
        private readonly DateTime _endDate;

        public CreditOrderReport(DateTime startDate, DateTime endDate)
        {
            InitializeComponent();
            _beginDate = startDate;
            _endDate = endDate;
        }

        private void CreditOrderReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'EmployeePaymentDataSet.creditordersview' table. You can move, or remove it, as needed.
            creditordersviewTableAdapter.Fill(EmployeePaymentDataSet.creditordersview, _beginDate, _endDate);

            reportViewer1.RefreshReport();
        }
    }
}