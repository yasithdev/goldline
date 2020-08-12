using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Core.Inventory;
using MessageBox = System.Windows.MessageBox;

namespace Presentation.Views.Orders
{
    /// <summary>
    ///     Interaction logic for ServiceOrder.xaml
    /// </summary>
    public partial class ServiceOrder : Window
    {
        public ServiceOrder()
        {
            InitializeComponent();
            ServiceNameComboBox.ItemsSource = Service.GetServicesNames();
        }

        public Service SelectedService { get; set; }
        public decimal SpecifiedPrice { get; set; }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ServiceChargeTextBox.Text == "" || ServiceNameComboBox.Text == "")
                {
                    MessageBox.Show("Empty Inputs.", "Invalid Inputs");
                    return;
                }
                try
                {
                    decimal.Parse(ServiceChargeTextBox.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Please enter valid service charge");
                    ServiceChargeTextBox.Text = "";
                    return;
                }
                if (decimal.Parse(ServiceChargeTextBox.Text) <= 0)
                {
                    MessageBox.Show("Please Enter Valid Service Charge");
                    ServiceChargeTextBox.Text = "";
                    return;
                }
                var serviceName = ServiceNameComboBox.Text;
                Debug.WriteLine(serviceName);
                var searchResults = Service.GetServices(serviceName).ToArray();
                Debug.WriteLine(searchResults.Length);
                //if service IS there in DB,
                if (searchResults.Length == 1)
                {
                    SelectedService = searchResults.Single(); //get the only element which is selected
                }
                //result isnot there in DB
                else if (searchResults.Length == 0)
                {
                    //create new service
                    var nextAvailableId = Product.GetNextAvailableId();
                    var createdService = new Service(nextAvailableId, serviceName);

                    if (!Service.AddService(createdService))
                        return; //break if service was not added (add service returns a bool)

                    SelectedService = createdService;
                }
                else //there are many results
                {
                    MessageBox.Show("Please Be more specific");
                    return;
                }
                DialogResult = true;
                SpecifiedPrice = decimal.Parse(ServiceChargeTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
            
        }
    }

   
    }
