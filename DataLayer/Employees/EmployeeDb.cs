using System;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace DataLayer.Employees
{
    public class EmployeeDb
    {
        private readonly MySqlConnection _connection;
        private static readonly log4net.ILog log =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public EmployeeDb()
        {
            try
            {
                _connection = new MySqlConnection(ConnectionStrings.LocalConnString);
                _connection.Open();
            }
            catch (Exception ex)
            {
                log.Error("connection could not be opened :"+ex.Message);
            }
        }

        ~EmployeeDb()
        {
            _connection.Close();
        }

        public int GetMaxEmployeeId()
        {
            const string sqlStatement =
                "SELECT MAX(Employee_ID) FROM Employees";
            var command = new MySqlCommand(sqlStatement, _connection);
            var maxId = command.ExecuteScalar().ToString();
            return int.Parse(maxId == "" ? "0" : maxId);
        }

        public int GetMaxEmployeePaymentId()
        {
            const string sqlStatement =
                "SELECT MAX(Payment_ID) FROM Payments";
            var command = new MySqlCommand(sqlStatement, _connection);
            var maxId = command.ExecuteScalar().ToString();
            return int.Parse(maxId == "" ? "0" : maxId);
        }

        #region Employees

        public DataTable GetEmployees(string id = "%", string name = "", string contactInfo = "",
            string joinDateTime = "", string active = "%")
        {
            // get employees from database according to given parameters
            const string sqlStatement =
                "SELECT * FROM  EmployeeView WHERE (" +
                "Employee_ID LIKE @id AND " +
                "Name LIKE @name AND " +
                "ContactInfo LIKE @contactInfo AND " +
                "JoinDateTime LIKE @joinDateTime AND " +
                "Active LIKE @active)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", "%" + name + "%");
            command.Parameters.AddWithValue("@contactInfo", contactInfo + "%");
            command.Parameters.AddWithValue("@joinDateTime", joinDateTime + "%");
            command.Parameters.AddWithValue("@active", active);

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public bool AddEmployee(int id, string name, string contactInfo, DateTime joinDateTime, bool active)
        {
            // add employee to database
            const string sqlStatement =
                "INSERT INTO Employees (Employee_ID, Name, ContactInfo, JoinDateTime, Active) " +
                "VALUES (@id, @name, @contactInfo, @joinDateTime, @active)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@contactInfo", contactInfo);
            command.Parameters.AddWithValue("@joinDateTime", joinDateTime);
            command.Parameters.AddWithValue("@active", active);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException exception)
            {
                log.Debug(exception.Message);
                if (exception.Number == 1062)
                {
                    log.Debug("Duplicate primary key employeeID= :"+id);
                    Debug.WriteLine(exception.Message);
                    return false; //Duplicate primary key
                }
                throw;
            }
        }

        public bool UpdateEmployee(int id, string name, string contactInfo, DateTime joinDateTime, bool active)
        {
            // update employee in database
            const string sqlStatement =
                "UPDATE Employees " +
                "SET Name = @name, ContactInfo = @contactInfo, JoinDateTime = @joinDateTime, Active = @active " +
                "WHERE Employee_ID = @id";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@contactInfo", contactInfo);
            command.Parameters.AddWithValue("@joinDateTime", joinDateTime);
            command.Parameters.AddWithValue("@active", active);

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                log.Debug(ex.Message);
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /* In case of accidental adding of employee. i.e. If he has no dependencies in database
         * like payments and billings, can delete employee. Else should return false
         */

        public bool DeleteEmployee(int id)
        {
            // remove employee from database if no foreign key constraints fail
            const string sqlStatement =
                "DELETE FROM Employees WHERE Employee_ID = @id";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@id", id);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException exception)
            {
                if (exception.Number == 1451)
                {
                    log.Error("foreign key constraint fail :"+id+" "+exception.Message);
                    Debug.WriteLine(exception.Message);
                    return false; //foreign key constraint fail
                }
                throw;
            }
        }

        #endregion

        #region Employee Payments

        public DataTable GetEmpPayments(string id = "%", string employeeId = "%", string amount = "",
            string dateTime = "", string note = "")
        {
            //get payments from database according to given parameters
            const string sqlStatement =
                "SELECT * FROM Payments " +
                "WHERE (Payment_ID LIKE @pid AND Employee_ID LIKE @eid AND Amount LIKE @amount AND DateTime LIKE @dateTime AND Note LIKE @note)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@pid", id);
            command.Parameters.AddWithValue("@eid", employeeId);
            command.Parameters.AddWithValue("@amount", amount + "%");
            command.Parameters.AddWithValue("@dateTime", dateTime + "%");
            command.Parameters.AddWithValue("@note", "%" + note + "%");

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public bool AddEmpPayment(int id, int employeeId, decimal amount, DateTime dateTime, string note)
        {
            // add payment to database
            const string sqlStatement =
                "INSERT INTO Payments (Payment_ID, Employee_ID, Amount, DateTime, Note) " +
                "VALUES (@pid, @eid, @amount, @dateTime, @note)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@pid", id);
            command.Parameters.AddWithValue("@eid", employeeId);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@dateTime", dateTime);
            command.Parameters.AddWithValue("@note", note);

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
                    log.Error("duplicate primary key PaymentID= :" + id);
                    Debug.WriteLine(exception.Message);
                    return false; //Duplicate primary key
                }
                throw;
            }
        }

        public bool UpdateEmpPayment(int id, int employeeId, decimal amount, string dateTime, string note)
        {
            // update multiple payments in database
            const string sqlStatement =
                "UPDATE Payments " +
                "SET (Employee_ID = @eid, Amount = @amount, DateTime = @dateTime, Note = @note) " +
                "WHERE (Payment_ID = @pid)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@pid", id);
            command.Parameters.AddWithValue("@eid", employeeId);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@dateTime", dateTime);
            command.Parameters.AddWithValue("@note", note);

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                log.Error(ex.Message);
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion
    }
}