using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using  Core.Orders;

namespace UnitTest.Orders
{
    [TestClass]
    public class SupplyOrderTest
    {
        [TestMethod]
        public void AddSupplyOrder()
        {
            int tempId = SupplyOrder.GetNextOrderId();
            SupplyOrderEntry tempEntry1 = new SupplyOrderEntry(tempId, 1, "tempName1", 1, 1000);
            SupplyOrderEntry tempEntry2 = new SupplyOrderEntry(tempId, 2, "tempName2", 1, 1000);
            SupplyOrder newOrder = new SupplyOrder(tempId,1);
            newOrder.AddEntry(tempEntry1);
            newOrder.AddEntry(tempEntry2);
            newOrder.SupplierId = 1;
            newOrder.SupplierName = "Randil";
            Assert.IsTrue(SupplyOrder.AddSupplyOrder(newOrder));
        }

        [TestMethod]
        public void GetSupplieOrder()
        {
            Assert.IsTrue(SupplyOrder.GetSupplyOrders(false,supplierId: 1.ToString()).Any());
        }

        [TestMethod]
        public void CheckTheTransactionMode()
        {
            int tempId = SupplyOrder.GetNextOrderId();
            SupplyOrderEntry tempEntry1 = new SupplyOrderEntry(tempId, 1, "tempName1", 1, 1000);
            //add a item with same itemid, but when teh items are adding using gui this was prevented
            SupplyOrderEntry tempEntry2 = new SupplyOrderEntry(tempId, 1, "tempName2", 1, 1000);
            SupplyOrder newOrder = new SupplyOrder(tempId, 1);
            newOrder.AddEntry(tempEntry1);
            newOrder.AddEntry(tempEntry2);
            newOrder.SupplierId = 1;
            newOrder.SupplierName = "Randil";
            Assert.IsFalse(SupplyOrder.AddSupplyOrder(newOrder));
            Assert.IsFalse(SupplyOrder.GetSupplyOrders(false, orderId:tempId.ToString()).Any());
        }
    }
}
