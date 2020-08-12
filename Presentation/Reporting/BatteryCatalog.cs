using System;
using System.Windows.Forms;

namespace Presentation.Reporting
{
    public partial class BatteryCatalog : Form
    {
        public BatteryCatalog()
        {
            InitializeComponent();
        }

        private void BatteryCatalog_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'CatalogDataSet.batteriesview' table. You can move, or remove it, as needed.
            batteriesviewTableAdapter.Fill(CatalogDataSet.batteriesview);

            reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}