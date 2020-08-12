using System.Linq;
using Core.Customers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Customers
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void Test_GetCustomer()
        {
            Assert.IsTrue(Customer.GetCustomers().Any());
        }

        public void Test_AddCustomer()
        {
            Assert.IsNull(Customer.AddCustomer(new Customer(1, "Test", "1234567890", "0987654321V", 0)));
        }

        public void Test_UpdateCustomer()
        {
            Assert.IsNull(Customer.UpdateCustomer( new Customer(1,"Mendis","0772990261","123456789v",0)));
        }

        public void Test_GetCustomerById()
        {
           Assert.IsNull(Customer.GetCustomerById(1)); 
        }

        public void Test_DeleteCustomer()
        {
           Assert.IsNull(Customer.DeleteCustomer(0));;
        }
        public void Test_GetNextCustomerId()
        {
            Assert.IsNotNull(Customer.GetNextCustomerId());
        }
        public void Test_GetDueAmount()
        {
            Assert.IsNotNull(Customer.GetDueAmount(0));
        }
        public void Test_UpdateDues()
        {
            Assert.IsNotNull(Customer.UpdateDues(0,0));
        }
    }
}