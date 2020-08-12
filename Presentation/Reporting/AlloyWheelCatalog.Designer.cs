namespace Presentation.Reporting
{
    partial class AlloyWheelCatalog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.alloywheelsviewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CatalogDataSet = new Presentation.Reporting.CatalogDataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.alloywheelsviewTableAdapter = new Presentation.Reporting.CatalogDataSetTableAdapters.alloywheelsviewTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.alloywheelsviewBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CatalogDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // alloywheelsviewBindingSource
            // 
            this.alloywheelsviewBindingSource.DataMember = "alloywheelsview";
            this.alloywheelsviewBindingSource.DataSource = this.CatalogDataSet;
            // 
            // CatalogDataSet
            // 
            this.CatalogDataSet.DataSetName = "CatalogDataSet";
            this.CatalogDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer1.AutoSize = true;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.alloywheelsviewBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Presentation.Reporting.Report_AlloyWheel.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(644, 615);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
            // 
            // alloywheelsviewTableAdapter
            // 
            this.alloywheelsviewTableAdapter.ClearBeforeFill = true;
            // 
            // AlloyWheelCatalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 615);
            this.Controls.Add(this.reportViewer1);
            this.MinimumSize = new System.Drawing.Size(660, 650);
            this.Name = "AlloyWheelCatalog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AlloyWheelCatalog";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.AlloyWheelCatalog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.alloywheelsviewBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CatalogDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource alloywheelsviewBindingSource;
        private CatalogDataSet CatalogDataSet;
        private CatalogDataSetTableAdapters.alloywheelsviewTableAdapter alloywheelsviewTableAdapter;
    }
}