using DataLayer.Inventory;

namespace Core.Inventory
{
    public abstract class Product
    {
        private static readonly InventoryDb InventoryDb = new InventoryDb();

        protected Product(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }

        public static int GetNextAvailableId()
        {
            return InventoryDb.GetMaxId() + 1;
        }

        public static int GetProductId(string name)
        {
            return int.Parse(InventoryDb.GetProductIdByName(name));
        }
    }
}