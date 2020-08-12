using System.Windows;
using Presentation.Reporting;

namespace Presentation.Views.Reports
{
    /// <summary>
    /// Interaction logic for Catalogs.xaml
    /// </summary>
    public partial class Catalogs : Window
    {
        public Catalogs()
        {
            InitializeComponent();
        }

        private void BatteryButton_Click(object sender, RoutedEventArgs e)
        {
            new BatteryCatalog().Show();
        }

        private void TyreButton_Click(object sender, RoutedEventArgs e)
        {
            new TyreCatalog().Show();
        }

        private void AlloyWheelButton_Click(object sender, RoutedEventArgs e)
        {
            new AlloyWheelCatalog().Show();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
