using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Orders;

namespace Core.Orders
{
    public class CreditOrder : Order
    {
        private static readonly OrderDb OrderDb = new OrderDb();

        public CreditOrder(int orderId, int userId, List<OrderEntry> orderEntries, string note,
            DateTime dateTime, int customerId)
            : base(orderId, userId, orderEntries, note, dateTime)
        {
            CustomerId = customerId;
        }

        public int CustomerId { get; set; }

        public static IEnumerable<CreditOrder> GetCreditOrders(string orderId = "", string customerId = "",
            string dateTime = "", string note = "")
        {
            var reader = OrderDb.GetCreditOrders(orderId, customerId, dateTime, note).CreateDataReader();

            while (reader.Read())
            {
                yield return new CreditOrder(
                    int.Parse(reader["Order_ID"].ToString()),
                    int.Parse(reader["User_ID"].ToString()),
                    OrderEntry.GetOrderEntries(reader["Order_ID"].ToString()).ToList(),
                    reader["Note"].ToString(),
                    DateTime.Parse(reader["DateTime"].ToString()),
                    int.Parse(reader["Customer_ID"].ToString()));
            }
        }

        public static void AddCreditOrder(int orderId,int customerId)
        {
            OrderDb.AddCreditOrder(orderId,customerId);
        }
    }
}