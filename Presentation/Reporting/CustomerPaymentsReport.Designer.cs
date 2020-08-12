namespace Presentation.Reporting
{
    partial class CustomerPaymentsReport
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
            this.CustomerPaymentsviewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.EmployeePaymentDataSet = new Presentation.Reporting.EmployeePaymentDataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.CustomerPaymentsviewTableAdapter = new Presentation.Reporting.EmployeePaymentDataSetTableAdapters.CustomerPaymentsviewTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.CustomerPaymentsviewBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmployeePaymentDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // CustomerPaymentsviewBindingSource
            // 
            this.CustomerPaymentsviewBindingSource.DataMember = "CustomerPaymentsview";
            this.CustomerPaymentsviewBindingSource.DataSource = this.EmployeePaymentDataSet;
            // 
            // EmployeePaymentDataSet
            // 
            this.EmployeePaymentDataSet.DataSetName = "EmployeePaymentDataSet";
            this.EmployeePaymentDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer1.AutoSize = true;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.CustomerPaymentsviewBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Presentation.Reporting.Report_CustomerPayments.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(770, 691);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
            // 
            // CustomerPaymentsviewTableAdapter
            // 
            this.CustomerPaymentsviewTableAdapter.ClearBeforeFill = true;
            // 
            // CustomerPaymentsReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 691);
            this.Controls.Add(this.reportViewer1);
            this.MinimumSize = new System.Drawing.Size(619, 726);
            this.Name = "CustomerPaymentsReport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomerPaymentsReport";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.CustomerPaymentsReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CustomerPaymentsviewBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmployeePaymentDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource CustomerPaymentsviewBindingSource;
        private EmployeePaymentDataSet EmployeePaymentDataSet;
        private EmployeePaymentDataSetTableAdapters.CustomerPaymentsviewTableAdapter CustomerPaymentsviewTableAdapter;
    }
}