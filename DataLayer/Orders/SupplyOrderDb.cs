using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataLayer.Orders
{
    public class SupplyOrderDb
    {
        protected readonly MySqlConnection Connection;
        protected static MySqlTransaction Transaction;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SupplyOrderDb()
        {
            Connection = new MySqlConnection(ConnectionStrings.LocalConnString);
            try
            {
                Connection.Open();
            }
            catch (MySqlException e)
            {
                log.Error("Connection open Error:",e);
            }
        }

        ~SupplyOrderDb()
        {
            try
            {
                Connection.Close();
            }
            catch (MySqlException e)
            {
                log.Error("Connection close Error:",e);
            }
        }

        #region Get

        public int GetMaxSupplyOrderId()
        {
            const string sqlStatement = "SELECT MAX(SupplyOrder_ID) FROM SupplyOrders";
            var command = new MySqlCommand(sqlStatement, Connection);
            var maxId = command.ExecuteScalar().ToString();
            return maxId == "" ? 0 : int.Parse(maxId);
        }

        public DataTable GetSupplyOrders(string orderId = "%", string supplierId = "%", string supplierName = "",
            string itemName = "", string date = "", string paid = "%", int limit = 50000)
        {
            const string sqlStatement =
                "SELECT * FROM SupplyOrdersView " +
                "WHERE SupplyOrder_ID LIKE @orderId AND " +
                "Supplier_ID LIKE @supplierId AND " +
                "Name LIKE @supplierName AND " +
                "Paid LIKE @paid LIMIT @limit";

            var command = new MySqlCommand(sqlStatement, Connection);

            command.Parameters.AddWithValue("@orderId", orderId);
            command.Parameters.AddWithValue("@supplierId", supplierId);
            command.Parameters.AddWithValue("@supplierName", "%" + supplierName + "%");
            command.Parameters.AddWithValue("@itemName", "%" + itemName + "%");
            command.Parameters.AddWithValue("@paid", paid);
            command.Parameters.AddWithValue("@limit", limit);

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public DataTable GetDueSupplyOrders(int supplierId)
        {
            const string sqlStatement =
                "SELECT * FROM SupplyOrdersView " +
                "WHERE Supplier_ID LIKE @supplierId AND " +
                "Paid LIKE @paid";

            var command = new MySqlCommand(sqlStatement, Connection);

            command.Parameters.AddWithValue("@supplierId", supplierId);
            command.Parameters.AddWithValue("@paid", false);

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public DataTable GetSupplyOrderEntries(int orderId)
        {
            const string sqlStatement =
                "SELECT * FROM SupplyOrderEntriesView " +
                "WHERE SupplyOrder_ID LIKE @orderId";

            var command = new MySqlCommand(sqlStatement, Connection);

            command.Parameters.AddWithValue("@orderId", orderId);

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        #endregion

        #region AddOrder
        public bool AddSupplyOrder(int orderId, int supplierId, bool paid, DateTime dateTime,
            string note, int userId, decimal price)
        {
            const string sqlStatement =
                "INSERT INTO SupplyOrders " +
                "(SupplyOrder_ID, Supplier_ID, Paid, DateTime, Note, User_ID, Price) " +
                "VALUES (@orderId, @supplierId, @paid, @dateTime, @note, @userId, @price)";
            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@orderId", orderId);
            command.Parameters.AddWithValue("@supplierId", supplierId);
            command.Parameters.AddWithValue("@paid", paid);
            command.Parameters.AddWithValue("@dateTime", dateTime);
            command.Parameters.AddWithValue("@note", note);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@price", price);

            command.Transaction = Transaction;

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                Transaction.Rollback();
                log.Info("Adding Supply Order:",e);
                return false;
            }
        }

        public bool AddSupplyOrderEntry(int orderId, int itemId, int quantity, decimal amount)
        {
            const string sqlStatement =
                "INSERT INTO SupplyOrderEntries " +
                "(SupplyOrder_ID, Item_ID, Quantity, Amount) " +
                "VALUES (@orderId, @itemId, @quantity, @amount)";
            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@orderId", orderId);
            command.Parameters.AddWithValue("@itemId", itemId);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@amount", amount);

            command.Transaction = Transaction;

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                Transaction.Rollback();
                log.Info("Adding Supply Order Entry:", e);
                return false;
            }
        }

        public bool BeginUpdateOrders()
        {
            try
            {
                Transaction = Connection.BeginTransaction();
                return true;
            }
            catch (MySqlException e)
            {
                log.Info("Begin transaction mode:",e);
                return false;
            }
        }

        public bool FinalizeOrder()
        {
            try
            {
                Transaction.Commit();
                return true;
            }
            catch (MySqlException e)
            {
                Transaction.Rollback();
                log.Info("Finalyze Order:",e);
                return false;
            }
        }
        #endregion

        #region ReverseOrder
        public bool ReverseSupplyOrder(int orderId, int supplierId, bool paid, DateTime dateTime,
            string note, int userId, decimal price)
        {

            const string sqlStatement =
                "INSERT INTO SupplyOrders " +
                "(SupplyOrder_ID, Supplier_ID, Paid, DateTime, Note, User_ID, Price) " +
                "VALUES (@orderId, @supplierId, @paid, @dateTime, @note, @userId, @price)";
            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@orderId", orderId);
            command.Parameters.AddWithValue("@supplierId", supplierId);
            command.Parameters.AddWithValue("@paid", paid);
            command.Parameters.AddWithValue("@dateTime", dateTime);
            command.Parameters.AddWithValue("@note", note);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@price", -price);

            command.Transaction = Transaction;

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException)
            {
                Transaction.Rollback();
                return false;
            }
        }

        public bool ReverseSupplyOrderEntry(int orderId, int itemId, int quantity, decimal amount)
        {

            const string sqlStatement =
                "INSERT INTO SupplyOrderEntries " +
                "(SupplyOrder_ID, Item_ID, Quantity, Amount) " +
                "VALUES (@orderId, @itemId, @quantity, @amount)";
            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@orderId", orderId);
            command.Parameters.AddWithValue("@itemId", itemId);
            command.Parameters.AddWithValue("@quantity", -quantity);
            command.Parameters.AddWithValue("@amount", -amount);

            command.Transaction = Transaction;

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException)
            {
                Transaction.Rollback();
                return false;
            }
        }

        #endregion

        #region Update

        public bool UpdateSupplyOrder(int supplyOrderId, bool paid)
        {
            const string sqlStatement =
                "UPDATE SupplyOrders SET Paid = @paid " +
                "WHERE SupplyOrder_ID LIKE @supplyOrderId";
            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@supplyOrderId", supplyOrderId);
            command.Parameters.AddWithValue("@paid", paid);
            command.Transaction = Transaction;

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                Transaction.Rollback();
                log.Info("Update supply order:", e);
                return false;
            }
        }

        #endregion

    }
}