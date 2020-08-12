using System;
using System.Collections.Generic;

namespace Core.Orders
{
    public class SupplyOrderEntry
    {
        public SupplyOrderEntry(int orderId, int productId, string itemName, int quantity,
            decimal price)
        {
            OrderId = orderId;
            ItemId = productId;
            ItemName = itemName;
            Quantity = quantity;
            Price = price;
        }

        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}