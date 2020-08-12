using System;
using System.Collections.Generic;
using DataLayer.Customers;

namespace Core.Customers
{
    public class Vehicle
    {
        private static readonly CustomerDb CustomerDb = new CustomerDb();

        public Vehicle(string vehicleNo, int customerId, DateTime lastVisit)
        {
            VehicleNo = vehicleNo;
            CustomerId = customerId;
            LastVisit = lastVisit;
        }

        public string VehicleNo { get; set; }
        public int CustomerId { get; set; }
        public DateTime LastVisit { get; set; }

        public static IEnumerable<Vehicle> GetCustomerVehicles(string vehicleNo = "", string customerId = "",
            string lastVisit = "")
        {
            var reader = CustomerDb.GetCustomerPayments(vehicleNo, customerId, lastVisit).CreateDataReader();

            while (reader.Read())
            {
                yield return new Vehicle(
                    reader["VehicleNo"].ToString(),
                    int.Parse(reader["Customer_ID"].ToString()),
                    DateTime.Parse(reader["LastVisit"].ToString())
                    );
            }
        }

        public static bool AddCustomerVehicle(Vehicle vehicle)
        {
            return CustomerDb.AddCustomerVehicle(vehicle.VehicleNo, vehicle.CustomerId, vehicle.LastVisit);
        }

        public static bool UpdateCustomerVehicle(Vehicle vehicle)
        {
            return CustomerDb.UpdateCustomerVehicle(vehicle.VehicleNo, vehicle.CustomerId, vehicle.LastVisit);
        }

        public static bool DeleteCustomerVehicle(string vehicleNo)
        {
            return CustomerDb.DeleteCustomerVehicle(vehicleNo);
        }
    }
}