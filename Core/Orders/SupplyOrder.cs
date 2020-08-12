using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Orders;

namespace Core.Orders
{
    public class SupplyOrder
    {
        private static readonly SupplyOrderDb SupplyOrderDb = new SupplyOrderDb();
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SupplyOrder(int id, int userId)
        {
            Id = id;
            UserId = userId;
            OrderEntries = new List<SupplyOrderEntry>();
        }

        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public List<SupplyOrderEntry> OrderEntries { get; set; }
        public DateTime DateTime { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }
        public bool Paid { get; set; }
        public decimal Total
        {
            get { return OrderEntries.Sum(entry => entry.Price); }
        }

        public bool AddEntry(SupplyOrderEntry entry)
        {
            foreach (var testEntry in OrderEntries)
            {
                if (entry.ItemName == testEntry.ItemName)
                {
                    return false;
                }
            }
            OrderEntries.Add(entry);
            return true;
        }

        public void RemoveEntry(SupplyOrderEntry entry)
        {
            OrderEntries.Remove(entry);
        }

        public static int GetNextOrderId()
        {
            return SupplyOrderDb.GetMaxSupplyOrderId() + 1;
        }

        public static IEnumerable<SupplyOrder> GetSupplyOrders(bool islimited, string orderId = "%",string supplierId = "%", 
            string supplierName = "%", string itemName = "%",string date = "%", string paid = "%")
        {
            int limit = 50000;
            if (islimited) limit = 100;
            var reader = SupplyOrderDb.GetSupplyOrders(orderId, supplierId, supplierName, itemName, date, paid,limit).CreateDataReader();
            while (reader.Read())
            {
                var id = int.Parse(reader["SupplyOrder_ID"].ToString());
                var sId = int.Parse(reader["Supplier_ID"].ToString());
                var sName = reader["Name"].ToString();
                var entries = GetSupplyOrderEntries(id);
                var dateTime = DateTime.Parse(reader["DateTime"].ToString());
                var note = reader["Note"].ToString();
                var uId = int.Parse(reader["User_ID"].ToString());
                var sPaid = bool.Parse(reader["Paid"].ToString());

                var newOrder = new SupplyOrder(id, uId)
                {
                    SupplierId = sId,
                    SupplierName = sName,
                    OrderEntries = entries.ToList(),
                    Note = note,
                    DateTime = dateTime,
                    Paid = sPaid
                };

                yield return newOrder;
            }
            reader.Close();
        }

        public static IEnumerable<SupplyOrder> GetDueSupplyOrders(int supplierId)
        {
            var reader = SupplyOrderDb.GetDueSupplyOrders(supplierId).CreateDataReader();
            while (reader.Read())
            {
                var id = int.Parse(reader["SupplyOrder_ID"].ToString());
                var sId = int.Parse(reader["Supplier_ID"].ToString());
                var sName = reader["Name"].ToString();
                var entries = GetSupplyOrderEntries(id);
                var dateTime = DateTime.Parse(reader["DateTime"].ToString());
                var note = reader["Note"].ToString();
                var uId = int.Parse(reader["User_ID"].ToString());
                var sPaid = bool.Parse(reader["Paid"].ToString());

                var newOrder = new SupplyOrder(id, uId)
                {
                    SupplierId = sId,
                    SupplierName = sName,
                    OrderEntries = entries.ToList(),
                    Note = note,
                    DateTime = dateTime,
                    Paid = sPaid
                };

                yield return newOrder;
            }
            reader.Close();
        }

        public static IEnumerable<SupplyOrderEntry> GetSupplyOrderEntries(int orderId)
        {
            var reader = SupplyOrderDb.GetSupplyOrderEntries(orderId).CreateDataReader();

            while (reader.Read())
            {
                var id = int.Parse(reader["SupplyOrder_ID"].ToString());
                var itemId = int.Parse(reader["Item_ID"].ToString());
                var itemName = reader["ProductName"].ToString();
                var quantity = int.Parse(reader["Quantity"].ToString());
                var price = decimal.Parse(reader["Amount"].ToString());

                yield return new SupplyOrderEntry(id, itemId, itemName, quantity, price);
            }
            reader.Close();
        }

        public static bool AddSupplyOrder(SupplyOrder supplyOrder)
        {
            Log.Info("Adding new supply order");
            if (!SupplyOrderDb.BeginUpdateOrders()) return false;

            if (!SupplyOrderDb.AddSupplyOrder(supplyOrder.Id, supplyOrder.SupplierId,
                supplyOrder.Paid, supplyOrder.DateTime, supplyOrder.Note, supplyOrder.UserId,
                supplyOrder.Total))
            {
                Log.Debug(supplyOrder);
                return false;
            }

            foreach (var entry in supplyOrder.OrderEntries)
            {
                if (!SupplyOrderDb.AddSupplyOrderEntry(entry.OrderId, entry.ItemId, entry.Quantity,
                    entry.Price))
                {
                    Log.Debug(supplyOrder);
                    return false;
                }
            }

            return SupplyOrderDb.FinalizeOrder();
        }

        public static bool ReverseSupplyOrder(SupplyOrder supplyOrder, int userId)
        {
            Log.Info("Reverse supply orders");
            if (!SupplyOrderDb.BeginUpdateOrders()) return false;

            if (!SupplyOrderDb.UpdateSupplyOrder(supplyOrder.Id, true))
            {
                Log.Debug(supplyOrder);
                return false;
            }

            supplyOrder.Note = "--ReversedOrder (ReferenceOrderId: " + supplyOrder.Id + ")" ;
            supplyOrder.Id = GetNextOrderId();
            supplyOrder.Paid = true;

            if (!SupplyOrderDb.ReverseSupplyOrder(supplyOrder.Id, supplyOrder.SupplierId,
                supplyOrder.Paid, DateTime.Now, supplyOrder.Note, userId, supplyOrder.Total))
            {
                Log.Debug(supplyOrder);
                return false;
            }

            foreach (var entry in supplyOrder.OrderEntries)
            {
                if (!SupplyOrderDb.ReverseSupplyOrderEntry(supplyOrder.Id, entry.ItemId, entry.Quantity,
                    entry.Price))
                {
                    Log.Debug(entry);
                    return false;
                }
            }

            return SupplyOrderDb.FinalizeOrder();
        }

        public static bool PaySupplyOrders(List<SupplyOrder> orderList)
        {
            Log.Info("Update supply orders");
            if (!SupplyOrderDb.BeginUpdateOrders()) return false;

            foreach (var order in orderList)
            {
                if (!SupplyOrderDb.UpdateSupplyOrder(order.Id, true))
                {
                    Log.Debug(order);
                    return false;
                }
            }
            return SupplyOrderDb.FinalizeOrder();
        }
    }
}