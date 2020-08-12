using System;
using System.Windows.Forms;

namespace Presentation.Reporting
{
    public partial class TyreCatalog : Form
    {
        public TyreCatalog()
        {
            InitializeComponent();
        }

        private void TyreCatalog_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'CatalogDataSet.tyresview' table. You can move, or remove it, as needed.
            tyresviewTableAdapter.Fill(CatalogDataSet.tyresview);

            reportViewer1.RefreshReport();
        }
    }
}