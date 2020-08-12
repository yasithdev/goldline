namespace Presentation.Reporting
{
    partial class TyreCatalog
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
            this.tyresviewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CatalogDataSet = new Presentation.Reporting.CatalogDataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.tyresviewTableAdapter = new Presentation.Reporting.CatalogDataSetTableAdapters.tyresviewTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.tyresviewBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CatalogDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // tyresviewBindingSource
            // 
            this.tyresviewBindingSource.DataMember = "tyresview";
            this.tyresviewBindingSource.DataSource = this.CatalogDataSet;
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
            reportDataSource1.Value = this.tyresviewBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Presentation.Reporting.Report_TyreCatalog.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(659, 691);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
            // 
            // tyresviewTableAdapter
            // 
            this.tyresviewTableAdapter.ClearBeforeFill = true;
            // 
            // TyreCatalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 691);
            this.Controls.Add(this.reportViewer1);
            this.MinimumSize = new System.Drawing.Size(675, 726);
            this.Name = "TyreCatalog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TyreCatalog";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.TyreCatalog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tyresviewBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CatalogDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource tyresviewBindingSource;
        private CatalogDataSet CatalogDataSet;
        private CatalogDataSetTableAdapters.tyresviewTableAdapter tyresviewTableAdapter;
    }
}