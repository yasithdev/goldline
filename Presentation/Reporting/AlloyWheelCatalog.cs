using System;
using System.Windows.Forms;

namespace Presentation.Reporting
{
    public partial class AlloyWheelCatalog : Form
    {
        public AlloyWheelCatalog()
        {
            InitializeComponent();
        }

        private void AlloyWheelCatalog_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'CatalogDataSet.alloywheelsview' table. You can move, or remove it, as needed.
            alloywheelsviewTableAdapter.Fill(CatalogDataSet.alloywheelsview);

            reportViewer1.RefreshReport();
        }
    }
}