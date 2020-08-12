using System.Collections.Generic;
using DataLayer.Inventory;

namespace Core.Inventory
{
    public class Item : Product
    {
        private static readonly InventoryDb InventoryDb = new InventoryDb();

        public Item(int id, string name, int stock, decimal price) : base(id, name)
        {
            Stock = stock;
            Price = price;
        }

        public int Stock { get; set; }
        public decimal Price { get; set; }

        public static IEnumerable<Item> GetItems(string id = "%", string name = "%")
        {
            var reader = InventoryDb.GetItems(id, name).CreateDataReader();
            while (reader.Read())
            {
                var pId = int.Parse(reader["Item_ID"].ToString());
                var pName = reader["ProductName"].ToString();
                var pStock = int.Parse(reader["Stock"].ToString());
                var pPrice = decimal.Parse(reader["Price"].ToString());

                yield return new Item(pId, pName, pStock, pPrice);
            }
        }

        public static IEnumerable<Item> GetSuppliedItems(string supplierId = "%", 
            string itemName = "%")
        {
            var reader = InventoryDb.GetSupplierItems(supplierId, itemName).CreateDataReader();
            while (reader.Read())
            {
                var pId = int.Parse(reader["Item_ID"].ToString());
                var pName = reader["ProductName"].ToString();
                var pStock = int.Parse(reader["Stock"].ToString());
                var pPrice = decimal.Parse(reader["Price"].ToString());

                yield return new Item(pId, pName, pStock, pPrice);
            }
        }

        public static IEnumerable<string> GetItemTypes()
        {
            var reader = InventoryDb.GetItemTypes().CreateDataReader();
            while (reader.Read())
            {
                yield return reader["TypeName"].ToString();
            }
            reader.Close();
        }

        public static bool UpdateStockDetails(int itemId, int stock)
        {
            return InventoryDb.UpdateStockDetails(itemId, stock);
        }
    }
}