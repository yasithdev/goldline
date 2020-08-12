using System.Collections.Generic;
using DataLayer.Inventory;

namespace Core.Inventory
{
    public class Battery : Item
    {
        private static readonly InventoryDb InventoryDb = new InventoryDb();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Battery(int id, string name, int stock, decimal price, string brand) : base(id, name, stock, price)
        {
            Brand = brand;
        }

        public string Brand { get; set; }

        public static string GenerateName(string brand, string other="")
        {
            return brand + " " + other + " BATTERY";
        }

        public static IEnumerable<Battery> GetBatteries(string id = "%", string name = "%")
        {
            var reader = InventoryDb.GetBatteries(id, name).CreateDataReader();
            while (reader.Read())
            {
                // create tyre from retrieved data
                var pId = int.Parse(reader["Item_ID"].ToString());
                var pName = reader["ProductName"].ToString();
                var pStock = int.Parse(reader["Stock"].ToString());
                var pPrice = decimal.Parse(reader["Price"].ToString());
                var pBrand = reader["Brand"].ToString();

                yield return new Battery(pId, pName, pStock, pPrice, pBrand);
            }
            reader.Close();
        }

        public static bool UpdateBattery(Battery battery)
        {
            if (!InventoryDb.UpdateBatteryInfo(battery.Id, battery.Name, battery.Stock, battery.Price, battery.Brand))
            {
                log.Debug(battery);
                return false;
            }
            return true;
        }

        public static bool AddNewBattery(Battery battery)
        {
            if (!InventoryDb.AddNewBatteryInfo(battery.Id, battery.Name, battery.Stock,
                battery.Price, battery.Brand))
            {
                log.Debug(battery);
                return false;
            }
            return true;
        }

    }
}