using System.Collections.Generic;
using DataLayer.Inventory;

namespace Core.Inventory
{
    public class Tyre : Item
    {
        private static readonly InventoryDb InventoryDb = new InventoryDb();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Tyre(int id, string name, string brand, string dimension, string country, decimal price, int stock)
            : base(id, name, stock, price)
        {
            Brand = brand;
            Dimension = dimension;
            Country = country;
        }

        public string Brand { get; set; }
        public string Dimension { get; set; }
        public string Country { get; set; }

        public static string GenerateName(string dimension, string brand, string country)
        {
            return dimension + " " + " " + brand + " " + country + " TYRE";
        }

        public static IEnumerable<Tyre> GetTyres(string id = "%", string name = "%")
        {
            var reader = InventoryDb.GetTyres(id, name).CreateDataReader();
            while (reader.Read())
            {
                // create tyre from retrieved data
                var pId = int.Parse(reader["Item_ID"].ToString());
                var pName = reader["ProductName"].ToString();
                var pStock = int.Parse(reader["Stock"].ToString());
                var pPrice = decimal.Parse(reader["Price"].ToString());
                var pBrand = reader["Brand"].ToString();
                var pDimension = reader["Dimension"].ToString();
                var pCountry = reader["Country"].ToString();

                yield return new Tyre(pId, pName, pBrand, pDimension, pCountry, pPrice, pStock);
            }
            reader.Close();
        }

        public static bool UpdateTyres(Tyre tyre)
        {
            if (!InventoryDb.UpdateTyreInfo(tyre.Id, tyre.Name, tyre.Stock, tyre.Price, tyre.Brand, tyre.Dimension,
                tyre.Country))
            {
                log.Debug(tyre);
                return false;
            }
            return true;
        }

        public static bool AddNewTyre(Tyre tyre)
        {
            if (!InventoryDb.AddNewTyreInfo(tyre.Id, tyre.Name, tyre.Stock,
                tyre.Price, tyre.Brand, tyre.Dimension, tyre.Country))
            {
                log.Debug(tyre);
                return false;
            }
            return true;
        }

    }
}