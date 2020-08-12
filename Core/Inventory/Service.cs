using System.Collections.Generic;
using DataLayer.Inventory;

namespace Core.Inventory
{
    public class Service : Product
    {
        private static readonly InventoryDb InventoryDb = new InventoryDb();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Service(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<Service> GetServices(string serviceName = "%")
        {
            var reader = InventoryDb.GetServices(name:serviceName).CreateDataReader();
            while (reader.Read())
            {
                var sId = int.Parse(reader["Service_ID"].ToString());
                var sName = reader["ProductName"].ToString();

                yield return new Service(sId, sName);
            }
            reader.Close();
        }

        public static IEnumerable<string> GetServicesNames()
        {
            foreach (var service in GetServices())
            {
                yield return service.Name;
            }
        }

        public static int GetLastServiceId()
        {
            return InventoryDb.GetMaxId() + 1;
        }

        public static bool AddService(Service service)
        {
            if (!InventoryDb.AddNewServiceInfo(service.Id, service.Name))
            {
                log.Debug(service);
                return false;
            }
            return true;
        }

        public static bool UpdateService(Service service)
        {
            if (!InventoryDb.UpdateServiceInfo(service.Id, service.Name))
            {
                log.Debug(service);
                return false;
            }
            return true;
        }

    }
}