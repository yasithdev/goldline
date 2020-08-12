using System.Collections.Generic;
using DataLayer.Inventory;

namespace Core.Inventory
{
    public class AlloyWheel : Item
    {
        private static readonly InventoryDb InventoryDb = new InventoryDb();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AlloyWheel(int id, string name, int stock, decimal price, string brand, string dimension)
            : base(id, name, stock, price)
        {
            Brand = brand;
            Dimension = dimension;
        }

        public string Brand { get; set; }
        public string Dimension { get; set; }

        public static string GenerateName(string dimension, string brand)
        {
            return dimension + " " + brand + " ALLOYWHEEL ";
        }

        public static IEnumerable<AlloyWheel> GetAlloyWheels(string id = "%", string name = "%")
        {
            var reader = InventoryDb.GetAlloyWheels(id, name).CreateDataReader();
            while (reader.Read())
            {
                // create tyre from retrieved data
                var pId = int.Parse(reader["Item_ID"].ToString());
                var pName = reader["ProductName"].ToString();
                var pStock = int.Parse(reader["Stock"].ToString());
                var pPrice = decimal.Parse(reader["Price"].ToString());
                var pBrand = reader["Brand"].ToString();
                var pDimension = reader["Dimension"].ToString();

                yield return new AlloyWheel(pId, pName, pStock, pPrice, pBrand, pDimension);
            }
            reader.Close();
        }

        public static bool UpdateAlloyWheel(AlloyWheel alloywheel)
        {
            if (!InventoryDb.UpdateAlloyWheelInfo(alloywheel.Id, alloywheel.Name, alloywheel.Stock, alloywheel.Price,
                alloywheel.Brand, alloywheel.Dimension))
            {
                log.Debug(alloywheel);
                return false;
            }
            return true;
        }

        public static bool AddNewAlloyWheel(AlloyWheel alloyWheel)
        {
            if (!InventoryDb.AddNewAlloyWheelInfo(alloyWheel.Id, alloyWheel.Name, alloyWheel.Stock,
                alloyWheel.Price, alloyWheel.Brand, alloyWheel.Dimension))
            {
                log.Debug(alloyWheel);
                return false;
            }
            return true;
        }

    }
}