using System;

namespace Core.Employees.Users
{
    public class Cashier : User
    {
        public Cashier(int id, string name, string contactInfo, DateTime joinDateTime, bool active, string username,
            string password) : base(id, name, contactInfo, joinDateTime, active, username, password)
        {
            TypeName = "Cashier";
        }

        //public Order RequestOrderDetails(string orderId)
        //{
        //    return null; // invoices generation 
        //}


        //public string ViewCatalog()
        //{
        //    return null;
        //}

        //public string PayCredit(string customerId, decimal amount)
        //{
        //    return null; // settle credit customer payments
        //}

        //public string UpdateCustomerInfo()
        //{
        //    return null; // update credit customer info
        //}

        //public Item GetItemDetails(string itemId)
        //{
        //    return null; //when searching for items
        //}

        //public void AddOrderItem(string itemId, int quantity)
        //{
        //    //when adding items to the order
        //}

        //public string ProceedOrder(string customerId, string customerName, string contactNo, double amount)
        //{
        //    return null; //when finalizing the order
        //}

        //public void PrintInvoice()
        //{
        //}

        //public string SearchDueCredit(string customerId)
        //{
        //    return null; //to view the due credit amount of a credit ccustomer
        //}
    }
}