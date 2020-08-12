using System;
using System.Linq;
using Core.Customers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Customers
{
    [TestClass]
    public class CustomerPaymentTest
    {
        [TestMethod]
        public void Test_GetCustomerPayment()
        {
            Assert.IsTrue(CustomerPayment.GetCustomerPayments().Any());
        }

        public void Test_AddCustomerPayment()
        {
            Assert.IsNull(CustomerPayment.AddCustomerPayment(new CustomerPayment(1,1,0,new DateTime(),1)));
        }

        public void Test_UpdateCustomerPayment()
        {
            
            Assert.IsNull(CustomerPayment.UpdateCustomerPayment(new CustomerPayment(1,1,0,new DateTime(),1)));
        }

        public void Test_DeleteCustomerPayment()
        {
            Assert.IsNull(CustomerPayment.DeleteCustomerPayment(1));
        }

        public void Test_GetNextCustomerPaymentId()
        {
            Assert.IsNull(CustomerPayment.DeleteCustomerPayment(1));
        }
    }
}
