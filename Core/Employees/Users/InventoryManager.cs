using System;

namespace Core.Employees.Users
{
    public class InventoryManager : User
    {
        public InventoryManager(int id, string name, string contactInfo, DateTime joinDateTime, bool active,
            string username, string password) : base(id, name, contactInfo, joinDateTime, active, username, password)
        {
            TypeName = "Inventory Manager";
        }

        //public string CheckLowStocks()
        //{
        //    return null; //have to return something
        //}

        //private string UpdateStocks()
        //{
        //    return null; //have to update 
        //}

        //public string AddDefectiveItem(string serialNumber, string brandName)
        //{
        //    return null; //have to store returns of items to the originated company 
        //}

        //public void IssueReplacement()
        //{
        //    //issue replacement
        //}

        //public void InitSupplyOrder()
        //{
        //    //when applying supply order
        //}

        //public void AddSupplyItem(string itemId, int quantity)
        //{
        //    //add items to supply order
        //}

        //public string ProceedSupplyOrder(double amount, string message)
        //{
        //    return null; //finalize supply order
        //}
    }
}