using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataLayer.Customers
{
    public class CustomerDb
    {
        private readonly MySqlConnection _connection;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CustomerDb()
        {
            _connection = new MySqlConnection(ConnectionStrings.LocalConnString);
            _connection.Open();
        }

        ~CustomerDb()
        {
            _connection.Close();
        }

        public int GetMaxCustomerId()
        {
            const string sqlStatement =
                "SELECT MAX(Customer_ID) FROM CreditCustomers";
            var command = new MySqlCommand(sqlStatement, _connection);
            var maxId = command.ExecuteScalar().ToString();
            return int.Parse(maxId == "" ? "0" : maxId);
        }

        public int GetMaxCustomerPaymentId()
        {
            const string sqlStatement =
                "SELECT MAX(Payment_ID) FROM CustomerPayments";
            var command = new MySqlCommand(sqlStatement, _connection);
            var maxId = command.ExecuteScalar().ToString();
            return int.Parse(maxId == "" ? "0" : maxId);
        }

        #region Customers

        public DataTable GetCustomers(string id = "%", string name = "", string contactInfo = "", string nic = "",
            string dues = "")
        {
            // get customer from database according to given search parameters
            const string sqlStatement =
                "SELECT creditcustomers.Customer_ID, creditcustomers.Name , creditcustomers.Nic ," +
                "creditcustomers.ContactInfo, creditcustomers.Dues FROM creditcustomers " +
                "WHERE (Customer_ID LIKE @id AND Name LIKE @name AND Nic LIKE @nic AND ContactInfo " +
                "LIKE @contactInfo AND Dues LIKE @dues)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", "%" + name + "%");
            command.Parameters.AddWithValue("@nic", nic + "%");
            command.Parameters.AddWithValue("@contactInfo", contactInfo + "%");
            command.Parameters.AddWithValue("@dues", dues + "%");

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        //Added a method to get due amount for a particular customer
        public decimal GetCustomerDues(int id)
        {
            const string sqlStatement =
                "SELECT creditcustomers.Dues FROM creditcustomers " +
                "WHERE (Customer_ID = @id)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@id", id);

            return decimal.Parse(command.ExecuteScalar().ToString());
        }


        public bool AddCustomer(int id, string name, string nic, string contactInfo, decimal dues)
        {
            // add customer to database
            const string sqlStatement =
                "INSERT INTO CreditCustomers (Customer_ID, Name, Nic, ContactInfo, Dues) " +
                "VALUES (@id, @name, @nic, @contactInfo, @dues)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@nic", nic);
            command.Parameters.AddWithValue("@contactInfo", contactInfo);
            command.Parameters.AddWithValue("@dues", dues);

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                log.Debug(ex.Message +"Addcustomer method"+ "customer id= " + id);
                return false;
                
            }
        }

        public bool UpdateCustomer(int id, string name, string nic, string contactInfo, decimal dues)
        {
            // update customer in database
            const string sqlStatement =
                "UPDATE CreditCustomers " +
                "SET Name = @name, Nic = @nic, ContactInfo = @contactInfo, Dues = @dues " +
                "WHERE Customer_ID LIKE @id";
            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@nic", nic);
            command.Parameters.AddWithValue("@contactInfo", contactInfo);
            command.Parameters.AddWithValue("@dues", dues);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                log.Debug(ex.Message +"UpdateCustomer method"+ "customer id= " + id);
                return false;
            }
        }

        //method to get update Due amount for a particular customer
        public bool UpdateDueAmount(int id, decimal dues)
        {
            const string sqlStatement = "UPDATE CreditCustomers SET Dues = @dues WHERE Customer_ID = @id";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@dues", dues);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                log.Debug(ex.Message +"UpdateDueAmount method"+ "customer id= " + id);
                return false;
            }
        }

        #endregion

        #region Customer Payments

        public DataTable GetCustomerPayments(string id = "%", string customerId = "%", string amount = "",
            string dateTime = "", string userId = "%")
        {
            // search for customer payments according to given search parameters
            const string sqlStatement =
                "SELECT * FROM CustomerPaymentsView " +
                "WHERE (Payment_ID LIKE @pid AND Customer_ID LIKE @cid AND Amount LIKE @amount AND DateTime LIKE @dateTime AND User_ID LIKE @uid)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@pid", id);
            command.Parameters.AddWithValue("@cid", customerId);
            command.Parameters.AddWithValue("@amount", amount + "%");
            command.Parameters.AddWithValue("@dateTime", dateTime + "%");
            command.Parameters.AddWithValue("@uid", userId);

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public DataTable GetCustomerPaymentsByDateRange(DateTime lowerDate, DateTime higherDate)
        {
            //method to get CustomerPayments by credit customers by DATE RANGE 
            //date should be sent in "2016-05-12" manner
            const string sqlStatement =
                "SELECT Amount, DateTime FROM CustomerPayments " +
                "WHERE DateTime " +
                "DateTime >= @lowerDate AND DateTime <= @higherDate";
            var command = new MySqlCommand(sqlStatement, _connection);

            command.Parameters.AddWithValue("@lowerDate", lowerDate);
            command.Parameters.AddWithValue("@higherDate", higherDate);

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public bool AddCustomerPayment(int id, int customerId, decimal amount, DateTime dateTime, int userId)
        {
            // add customer payment to database
            const string sqlStatement =
                "INSERT INTO CustomerPayments (Payment_ID, Customer_ID, Amount, DateTime, User_ID) " +
                "VALUES (@pid, @cid, @amount, @dateTime, @uid)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@pid", id);
            command.Parameters.AddWithValue("@cid", customerId);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@dateTime", dateTime);
            command.Parameters.AddWithValue("@uid", userId);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                log.Debug(ex.Message +"AddCustomerPayment method"+" Payment id"+id+" customer id= "+customerId);
                return false;
            }
        }

        public bool UpdateCustomerPayment(int id, int customerId, decimal amount, DateTime dateTime, int userId)
        {
            // update customer payment in database
            const string sqlStatement =
                "UPDATE CustomerPayments " +
                "SET (Customer_ID = @cid, Amount = @amount, DateTime = @dateTime, User_ID = @uid) " +
                "WHERE (Payment_ID = @pid)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@cid", customerId);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@dateTime", dateTime);
            command.Parameters.AddWithValue("@uid", userId);

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)

            {
                log.Debug(ex.Message +"UpdateCustomerPayment method"+"customer payment id="+id+ "customer id= " + customerId);
                return false;
            }
        }

        #endregion

        #region Customer Vehicles

        public DataTable GetCustomerVehicles(string vehicleNo = "%", string customerId = "%", string lastVisit = "")
        {
            // search for vehicles according to given parameters
            const string sqlStatement =
                "SELECT * FROM Vehicles " +
                "WHERE (VehicleNo LIKE @vno AND Customer_ID LIKE @cid AND LastVisit LIKE @lastVisit)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@vno", vehicleNo);
            command.Parameters.AddWithValue("@cid", customerId);
            command.Parameters.AddWithValue("@lastVisit", lastVisit + "%");

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public bool AddCustomerVehicle(string vehicleNo, int customerId, DateTime lastVisit)
        {
            // add vehicle to database
            const string sqlStatement =
                "INSERT INTO Vehicles (VehicleNo, Customer_ID, LastVisit) " +
                "VALUES (@vno, @cid, @lastvisit)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@vno", vehicleNo);
            command.Parameters.AddWithValue("@cid", customerId);
            command.Parameters.AddWithValue("@lastvisit", lastVisit);

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                log.Debug(ex.Message +"AddCustomerVehicle method "+ "customer id= " + customerId);
                return false;
            }
        }

        public bool UpdateCustomerVehicle(string vehicleNo, int customerId, DateTime lastVisit)
        {
            // update vehicle in database
            const string sqlStatement =
                "UPDATE Vehicles " +
                "SET (Customer_ID = @cid, LastVisit = @lastvisit) " +
                "WHERE (VehicleNo = @vno)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@vno", vehicleNo);
            command.Parameters.AddWithValue("@cid", customerId);
            command.Parameters.AddWithValue("@lastVisit", lastVisit);

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                log.Debug(ex.Message + "UpdateCustomerVehicle method"+"customer id= " + customerId);
                return false;
            }
        }

        #endregion

        #region Delete

        public bool DeleteCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCustomerPayment(int id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCustomerVehicle(string vehicleNo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}