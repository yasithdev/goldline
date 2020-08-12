using System;
using System.Collections.Generic;
using Core.Orders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Orders
{
    [TestClass()]
    public class OrderTest
    {
        [TestCleanup]
        public void After_Method()
        {
            Console.WriteLine("Method Executed!");
        }

        [ClassCleanup]
        public static void After_Class()
        {
            Console.WriteLine("OrderTest Class Executed");
        }
        [TestMethod()]
        public void AddOrderTest()
        {
            List<OrderEntry> oList = new List<OrderEntry>
            {
                new OrderEntry(100,3,"AMARON 12V",1,4500,10)
            };
            Order.AddOrder(new Order(100, 1, oList, "Test", System.DateTime.Now));
        }

        [TestMethod()]
        public void GetOrdersTest()
        {
            Assert.IsNotNull(Order.GetOrders());
        }


        [TestMethod()]
        public void GetCreditOrdersTest()
        {
            Assert.IsNotNull(CreditOrder.GetCreditOrders());
        }

        [TestMethod()]
        public void GetOrderEntriesTest()
        {
            Assert.IsNotNull(Order.GetOrderEntries("99"));
        }

        [TestMethod()]
        public void AddCreditOrderTest()
        {
            CreditOrder.AddCreditOrder(99, 1);
        }

        [TestMethod()]
        public void AddEntryTest()
        {
            List<OrderEntry> oList = new List<OrderEntry>
            {
                new OrderEntry(99,3,"AMARON 12V",1,4500,10)
            };
            Order o = new Order(99, 1, oList, "Test", System.DateTime.Now);
            o.AddEntry(new OrderEntry(99, 4, "ENKEI", 1, 1500, 10));
        }

        [TestMethod()]
        public void RemoveEntryTest()
        {
            List<OrderEntry> oList = new List<OrderEntry>
            {
                new OrderEntry(99,3,"AMARON 12V",1,4500,10),
                new OrderEntry(99, 4, "ENKEI", 1, 1500, 10)
            };
            Order o = new Order(99, 1, oList, "Test", System.DateTime.Now);
            o.RemoveEntry(oList[1]);
        }

        [TestMethod()]
        public void GetNextOrderEntryIdTest()
        {
            Assert.IsNotNull(Order.GetNextOrderEntryId());
        }

        
    }
}

