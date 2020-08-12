using System.Collections.Generic;
using Core.Inventory;
using DataLayer.Suppliers;

namespace Core.Suppliers
{
    public class Supplier : Entity
    {
        private static readonly SupplyDb SupplyDb = new SupplyDb();
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Supplier(int id, string name, string contactInfo, IEnumerable<Item> suppliedItems)
            : base(id, name, contactInfo)
        {
            SuppliedItems = suppliedItems;
        }

        public IEnumerable<Item> SuppliedItems { get; set; }

        public static IEnumerable<Supplier> GetSuppliers(string id = "%", string name = "%")
        {
            var reader = SupplyDb.GetSuppliers(id, name).CreateDataReader();
            while (reader.Read())
            {
                // create tyre from retrieved data
                var sId = int.Parse(reader["Supplier_ID"].ToString());
                var sName = reader["Name"].ToString();
                var sContactInfo = reader["ContactInfo"].ToString();
                var supItems = GetSuppliedItems(sId);

                yield return new Supplier(sId, sName, sContactInfo, supItems);
            }
        }

        public static bool UpdateSupplier(Supplier supplier)
        {
            if (!SupplyDb.UpdateSupplier(supplier.Id.ToString(), supplier.Name, supplier.ContactInfo))
            {
                Log.Debug(supplier);
                return false;
            }
            return true;
        }

        public static bool AddNewSupplier(Supplier supplier)
        {
            Log.Info("Add new supplier");
            if (!SupplyDb.AddNewSupplier(supplier.Id, supplier.Name,
                supplier.ContactInfo))
            {
                Log.Debug(supplier);
                return false;
            }
            return true;
        }

        public static int GetNextSupplierId()
        {
            return SupplyDb.GetMaxSupplierId() + 1;
        }

        public static bool RemoveSupplier(Supplier supplier)
        {
            return SupplyDb.RemoveSupplier(supplier.Id);
        }

        public static IEnumerable<Item> GetSuppliedItems(int supplierId, string itemName = "%")
        {
            return Item.GetSuppliedItems(supplierId.ToString(),
                itemName);
        }

        public static void RemoveSuppliedItem(int supplierId, Item suppliedItem)
        {
            SupplyDb.RemoveSuppliedItem(supplierId, suppliedItem.Id);
        }

        public static bool AddSuppliedItem(int supplierId, Item newSuppliedItem)
        {
            if (!SupplyDb.AddSuppliedItem(supplierId, newSuppliedItem.Id))
            {
                Log.Debug(newSuppliedItem);
                return false;
            }
            return true;
        }
    }
}