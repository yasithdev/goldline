using System.Collections.Generic;
using DataLayer.Orders;

namespace Core.Orders
{
    public class OrderEntry
    {
        private static readonly OrderDb OrderDb = new OrderDb();

        public OrderEntry(int orderId, int productId, string itemName, int quantity, decimal unitPrice,
            decimal discountPercent)
        {
            OrderId = orderId;
            ProductId = productId;
            ItemName = itemName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            DiscountPercent = discountPercent;
        }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal NetPrice => UnitPrice*Quantity*(100 - DiscountPercent)/100;

        //Get order entries matching a given order id
        public static IEnumerable<OrderEntry> GetOrderEntries(string orderId)
        {
            var reader = OrderDb.GetOrderEntries(orderId).CreateDataReader();

            while (reader.Read())
            {
                yield return new OrderEntry(
                    int.Parse(reader["Order_ID"].ToString()),
                    int.Parse(reader["Product_ID"].ToString()),
                    reader["ProductName"].ToString(),
                    int.Parse(reader["Quantity"].ToString()),
                    decimal.Parse(reader["UnitPrice"].ToString()),
                    decimal.Parse(reader["DiscountPercent"].ToString()));
            }
        }

        public static void AddOrderEntry(OrderEntry entry)
        {
            OrderDb.AddOrderEntry(entry.OrderId, entry.ProductId, entry.UnitPrice, entry.Quantity,
                entry.DiscountPercent, entry.NetPrice);
        }
    }
}