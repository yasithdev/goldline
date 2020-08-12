using System.Data;
using MySql.Data.MySqlClient;
using System;

namespace DataLayer.Inventory.Returns
{
    public class ReturnedItemsDb
    {
        protected readonly MySqlConnection Connection;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ReturnedItemsDb()
        {
            Connection = new MySqlConnection(ConnectionStrings.LocalConnString);
            try
            {
                Connection.Open();
            }
            catch (MySqlException e)
            {
                log.Error("Connection open error:",e);
            }
        }

        ~ReturnedItemsDb()
        {
            try
            {
                Connection.Close();
            }
            catch(MySqlException e)
            {
                log.Error("Connection close error:",e);
            }
        }

        #region Access DataBase

        public DataTable GetReturnedItems(string id = "%", string userName = "%", string customerName = "",
            string itemName = "", string condition = "%")
        {
            const string sqlStatement =
                "SELECT * FROM ReturnedItemsView " +
                "WHERE Return_ID LIKE @id  AND ReturnCondition LIKE @condition AND " +
                "UserName LIKE @userName AND " +
                "Name LIKE @customerName AND ProductName LIKE @itemName";

            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@condition", condition);
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@customerName", "%" + customerName + "%");
            command.Parameters.AddWithValue("@itemName", "%" + itemName + "%");

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public bool UpdateReturnedItemInfo(string returnId, string condition)
        {
            const string sqlStatement =
                "UPDATE Returns SET ReturnCondition = @condition WHERE Return_ID LIKE @returnid";
            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@condition", condition);
            command.Parameters.AddWithValue("@returnId", returnId);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                log.Info("Update Returned item:", e);
                return false;
            }
        }

        public bool AddReturnedItemInfo(string returnId, string userId, string customerId, string itemId,
            string quantity, DateTime date, string note)
        {
            const string sqlStatement =
                "INSERT INTO Returns (Return_ID, User_ID, Customer_ID, Item_ID, Quantity, DateTime, Notes) " +
                "VALUES (@returnId, @userId, @customerId, @itemId, @quantity, @date, @note)";

            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@returnId", returnId);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@customerId", customerId);
            command.Parameters.AddWithValue("@itemId", itemId);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@note", note);
            try { 
            command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                log.Info("Add returned item:", e);
                return false;
            }
        }

        public void DeleteReturnedItemInfo(int returnId)
        {
            const string sqlStatement =
                "DELETE FROM Returns WHERE Return_ID LIKE @id";

            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@id", returnId);

            command.ExecuteNonQuery();
        }

        public int GetMaxReturnId()
        {
            const string sqlStatement =
                "SELECT MAX(Return_ID) FROM Returns";

            var command = new MySqlCommand(sqlStatement, Connection);
            var maxId =  command.ExecuteScalar().ToString();
            return maxId == "" ? 0 : int.Parse(maxId);
        }

        #endregion
    }
}