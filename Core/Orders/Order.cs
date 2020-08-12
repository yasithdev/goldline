using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Orders;

namespace Core.Orders
{
    public class Order
    {
        private static readonly OrderDb OrderDb = new OrderDb();

        public Order(int orderId, int userId, List<OrderEntry> orderEntries, string note,
            DateTime dateTime)
        {
            OrderId = orderId;
            UserId = userId;
            OrderEntries = orderEntries;
            Note = note;
            DateTime = dateTime;
        }

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public List<OrderEntry> OrderEntries { get; set; }
        public string Note { get; set; }
        public DateTime DateTime { get; set; }

        public decimal Total
        {
            get { return OrderEntries.Sum(entry => entry.NetPrice); }
        }

        public void AddEntry(OrderEntry entry)
        {
            OrderEntries.Add(entry);
        }

        public void RemoveEntry(OrderEntry entry)
        {
            OrderEntries.Remove(entry);
        }

        public static int GetNextOrderEntryId()
        {
            return OrderDb.GetMaxOrderId() + 1;
        }

        public static IEnumerable<Order> GetOrders(string orderId = "%", string dateTime = "",
            string note = "")
        {
            var reader = OrderDb.GetOrders(orderId, dateTime, note).CreateDataReader();

            while (reader.Read())
            {
                yield return new Order(
                    int.Parse(reader["Order_ID"].ToString()),
                    int.Parse(reader["User_ID"].ToString()),
                    OrderEntry.GetOrderEntries(reader["Order_ID"].ToString()).ToList()
                    /*GetOrderEntries(orderId).ToList()*/ /*meth is below*/,
                    reader["Note"].ToString(),
                    DateTime.Parse(reader["DateTime"].ToString()));
            }
        }

        public static IEnumerable<OrderEntry> GetOrderEntries(string orderId)
        {
            var entriesList = OrderEntry.GetOrderEntries(orderId);
            return entriesList;
        }

        public static void AddOrder(Order order)
        {
            Console.WriteLine(order.Total);
            OrderDb.AddOrder(order.OrderId, order.Total, order.Note,
                order.DateTime, order.UserId);
            foreach (var entry in order.OrderEntries)
            {
                OrderEntry.AddOrderEntry(entry);
            }
        }
    }
}