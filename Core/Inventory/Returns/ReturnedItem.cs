using System.Collections.Generic;
using DataLayer.Inventory.Returns;
using System;

namespace Core.Inventory.Returns
{
    public class ReturnedItem
    {
        private static readonly ReturnedItemsDb ReturnedItemsDb = new ReturnedItemsDb();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ReturnedItem(int returnedId, int userId, string customerId, int itemId, string userName,
            string customerName,
            string itemName, int quantity, DateTime date, string note, string condition)
        {
            ReturnedId = returnedId;
            UserId = userId;
            CustomerId = customerId;
            ItemId = itemId;
            UserName = userName;
            CustomerName = customerName;
            ItemName = itemName;
            Quantity = quantity;
            Date = date;
            Note = note;
            Condition = condition;
        }

        public int ReturnedId { get; set; }
        public int UserId { get; set; }
        public string CustomerId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string UserName { get; set; }
        public string CustomerName { get; set; }
        public string ItemName { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public string Condition { get; set; }

        public static IEnumerable<ReturnedItem> GetreturnedItems(string id = "%", string userName = "%",
            string customerName = "%", string itemName = "%", string condition = "%")
        {
            var reader = ReturnedItemsDb.GetReturnedItems(id, userName, customerName, itemName, condition).
                CreateDataReader();

            while (reader.Read())
            {
                // create tyre from retrieved data
                var rId = int.Parse(reader["Return_ID"].ToString());
                var rUserId = int.Parse(reader["User_ID"].ToString());
                var rCustomerId = reader["Customer_ID"].ToString();
                var ritemId = int.Parse(reader["Item_ID"].ToString());
                var rUserName = reader["userName"].ToString();
                var rCustomerName = reader["Name"].ToString();
                var rDate = DateTime.Parse(reader["DateTime"].ToString());
                var rItemName = reader["ProductName"].ToString();
                var rQuantity = int.Parse(reader["Quantity"].ToString());
                var rNote = reader["Notes"].ToString();
                var rCondition = reader["ReturnCondition"].ToString();

                yield return new ReturnedItem(rId, rUserId, rCustomerId, ritemId, rUserName, rCustomerName, rItemName,
                    rQuantity, rDate, rNote, rCondition);
            }
        }

        public static bool UpdateReturnedItem(ReturnedItem updatedItem)
        {
            if (!ReturnedItemsDb.UpdateReturnedItemInfo(updatedItem.ReturnedId.ToString(), updatedItem.Condition))
            {
                log.Debug(updatedItem);
                return false;
            }
            return true;
        }

        public static bool AddReturnedItem(ReturnedItem updatedItem)
        {
            if (!ReturnedItemsDb.AddReturnedItemInfo(updatedItem.ReturnedId.ToString(), updatedItem.UserId.ToString(),
                updatedItem.CustomerId, updatedItem.ItemId.ToString(), updatedItem.Quantity.ToString(),
                updatedItem.Date, updatedItem.Note))
            {
                log.Debug(updatedItem);
                return false;
            }
            return true;
        }

        public static int GetNextReturnsId()
        {
            return ReturnedItemsDb.GetMaxReturnId() + 1;
        }
    }
}