using System.Collections.Generic;
using System.Diagnostics;
using DataLayer.Customers;

namespace Core.Customers
{
    public class Customer : Entity
    {
        private static readonly CustomerDb CustomerDb = new CustomerDb();

        public Customer(int id, string name, string contactInfo, string nic, decimal dues)
            : base(id, name, contactInfo)
        {
            Nic = nic;
            Dues = dues;
        }

        public string Nic { get; set; }
        public decimal Dues { get; set; }

        public static IEnumerable<Customer> GetCustomers(string id = "%", string name = "", string contactInfo = "",
            string nic = "", string dues = "")
        {
            var dataReader = CustomerDb.GetCustomers(id, name, nic, contactInfo, dues).CreateDataReader();
            while (dataReader.Read())
            {
                yield return new Customer(
                    int.Parse(dataReader["Customer_ID"].ToString()),
                    dataReader["Name"].ToString(),
                    dataReader["ContactInfo"].ToString(),
                    dataReader["Nic"].ToString(),
                    decimal.Parse(dataReader["Dues"].ToString())
                    );
            }
        }

        public static Customer GetCustomerById(int customerId)
        {
            var dataReader = CustomerDb.GetCustomers(id: customerId.ToString()).CreateDataReader();
            if (dataReader.Read())
            {
                return new Customer(
                    int.Parse(dataReader["Customer_ID"].ToString()),
                    dataReader["Name"].ToString(),
                    dataReader["ContactInfo"].ToString(),
                    dataReader["Nic"].ToString(),
                    decimal.Parse(dataReader["Dues"].ToString())
                    );
            }
            return null;
        }

        //Added a method to get due amount for a particular customer
        public static decimal GetDueAmount(int customerId)
        {
            return CustomerDb.GetCustomerDues(customerId);
        }

        public static bool UpdateDues(int customerId, decimal total)
        {
            var due = GetDueAmount(customerId);
            Debug.WriteLine("current Due = " + due);
            due = due + total;
            Debug.WriteLine("newDue = " + due);
            return CustomerDb.UpdateDueAmount(customerId, due);
        }

        public static bool AddCustomer(Customer customer)
        {
            return CustomerDb.AddCustomer(customer.Id, customer.Name, customer.Nic, customer.ContactInfo, customer.Dues);
        }

        public static bool UpdateCustomer(Customer customer)
        {
            return CustomerDb.UpdateCustomer(customer.Id, customer.Name, customer.Nic, customer.ContactInfo,
                customer.Dues);
        }

        public static bool DeleteCustomer(int id)
        {
            return CustomerDb.DeleteCustomer(id);
        }

        public static int GetNextCustomerId()
        {
            return CustomerDb.GetMaxCustomerId() + 1;
        }
       
    }
}