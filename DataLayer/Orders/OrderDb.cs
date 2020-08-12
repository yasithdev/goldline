using System;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace DataLayer.Orders
{
    public class OrderDb
    {
        private readonly MySqlConnection _connection;

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public OrderDb()
        {
            try
            {
                _connection = new MySqlConnection(ConnectionStrings.LocalConnString);
                _connection.Open();
            }
            catch (Exception ex)
            {
                log.Error("Could not open the connection :"+ ex.Message);
                Debug.WriteLine("Could not open the connection");
            }
        }

        ~OrderDb()
        {
            _connection.Close();
        }

        #region Get

        public int GetMaxOrderId() //method to get last order_Id
        {
            const string sqlStatement = "SELECT MAX(Order_ID) FROM Orders";
            var command = new MySqlCommand(sqlStatement, _connection);
            string maxOrderId = command.ExecuteScalar().ToString();

            if (maxOrderId != "")
            {
                return int.Parse(maxOrderId);
            }
            else
            {
                return 0;
            }
        }

        //method to read orders from database
        public DataTable GetOrders(string orderId = "%", string dateTime = "", string note = "")
        {
            const string sqlStatement =
                "SELECT * FROM  Orders " +
                "WHERE (Order_ID LIKE @id AND DateTime LIKE @datetime AND Note LIKE @note)";

            var command = new MySqlCommand(sqlStatement, _connection);

            command.Parameters.AddWithValue("@id", orderId);
            command.Parameters.AddWithValue("@datetime", dateTime + "%");
            command.Parameters.AddWithValue("@note", "%" + note + "%");

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public DataTable GetCreditOrders(string orderId = "%", string customerId = "%", string dateTime = "",
            string note = "") //method to read credit orders from database
        {
            const string sqlStatement =
                "SELECT * FROM  CreditOrdersView " +
                "WHERE (Order_ID LIKE @id AND Customer_ID LIKE @customerId AND DateTime LIKE @datetime AND Note LIKE @note)";

            var command = new MySqlCommand(sqlStatement, _connection);

            command.Parameters.AddWithValue("@id", orderId);
            command.Parameters.AddWithValue("@customerId", customerId);
            command.Parameters.AddWithValue("@datetime", dateTime + "%");
            command.Parameters.AddWithValue("@note", "%" + note + "%");

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public DataTable GetOrderEntries(string orderId) //method to read entries in an order
        {
            const string sqlStatement = "SELECT * FROM OrderEntries JOIN Products " +
                                        "ON (OrderEntries.Product_ID = Products.Product_ID) " +
                                        "WHERE (OrderEntries.Order_ID = @orderId)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@orderId", orderId);

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public DataTable GetOrdersByDateRange(DateTime lowerDate, DateTime higherDate)
            //method to get all orders between two dates
        {
            const string sqlStatement =
                "SELECT orders.Total, orders.DateTime " +
                "FROM orders " +
                "WHERE " +
                "orders.DateTime >= @lowerDate AND orders.DateTime <= @higherDate";
            var command = new MySqlCommand(sqlStatement, _connection);

            command.Parameters.AddWithValue("@lowerDate", lowerDate);
            command.Parameters.AddWithValue("@higherDate", higherDate);

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        #endregion

        #region Add

        public bool AddOrder(int orderId, decimal total, string note, DateTime dateTime, int userId)
            //create a new order.
        {
            const string sqlStatement = "INSERT INTO Orders (orders.Order_ID, orders.Total, orders.Note, " +
                                        "orders.DateTime, orders.User_ID) " +
                                        "VALUES (@order_ID, @total, @note, @dateTime, @user_ID)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@order_ID", orderId);
            command.Parameters.AddWithValue("@total", total);
            command.Parameters.AddWithValue("@note", note);
            command.Parameters.AddWithValue("@dateTime", dateTime);
            command.Parameters.AddWithValue("@user_ID", userId);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }

        public bool AddOrderEntry(int orderId, int productId, decimal unitPrice, int quantity, decimal discountPercent,
            decimal netPrice) //create order entries
        {
            const string sqlStatement = "INSERT INTO OrderEntries " +
                                        "(Order_ID, Product_ID, UnitPrice, Quantity, DiscountPercent, NetPrice) " +
                                        "VALUES (@order_ID, @product_ID, @unitPrice, @quantity, @discount_percentage, @net_price)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@order_ID", orderId);
            command.Parameters.AddWithValue("@product_ID", productId);
            command.Parameters.AddWithValue("@unitPrice", unitPrice);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@discount_percentage", discountPercent);
            command.Parameters.AddWithValue("@net_price", netPrice);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException exception)
            {
                log.Error(exception.Message);
                if (exception.Number == 1062)
                {
                    log.Debug("duplicate entry in order: "+orderId+" productId: "+productId);
                    return false; //Duplicate primary key
                }
                throw;

            }
        }

        public bool AddCreditOrder(int orderId, int customerId)
        {
            //if the order is a credit order, have to enter the credit customer's id
            // at this point we should know the customer id
            const string sqlStatement = "INSERT INTO CreditOrders VALUES (@Order_ID, @Customer_ID)";

            var commmand = new MySqlCommand(sqlStatement, _connection);
            commmand.Parameters.AddWithValue("@Order_ID", orderId);
            commmand.Parameters.AddWithValue("@Customer_ID", customerId);
            try
            {
                commmand.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                log.Debug("couldn't add credit order"+ex.Message);
                return false;
            }
        }

        #endregion
    }
}