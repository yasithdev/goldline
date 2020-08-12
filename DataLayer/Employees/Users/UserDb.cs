using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;

namespace DataLayer.Employees.Users
{
    public class UserDb : EmployeeDb
    {
        private readonly MySqlConnection _connection;
        private static readonly log4net.ILog log =
          log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public UserDb()
        {
            try
            {
                _connection = new MySqlConnection(ConnectionStrings.LocalConnString);
                _connection.Open();
            }
            catch (Exception)
            {
                log.Error("could not open the database connection");
            }
        }

        ~UserDb()
        {
            _connection.Close();
        }

        #region Users
        public DataTable GetUsers(string id = "%", string name = "", string contactInfo = "", string joinDateTime = "", string active = "%", string username = "%", string password = "%", string typeName = "%")
        {
            // Get list of users from dabase with given parameters
            const string sqlStatement =
                "SELECT * FROM Usersview WHERE (" +
                "Employee_ID LIKE @id AND " +
                "Name LIKE @name AND " +
                "TypeName LIKE @typeName AND " +
                "ContactInfo LIKE @contactInfo AND " +
                "JoinDateTime LIKE @joinDateTime AND " +
                "Active LIKE @active AND " +
                "Username LIKE BINARY @userName AND " +
                "Password LIKE BINARY @password)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", "%" + name + "%");
            command.Parameters.AddWithValue("@typeName", typeName);
            command.Parameters.AddWithValue("@contactInfo", contactInfo + "%");
            command.Parameters.AddWithValue("@joinDateTime", joinDateTime + "%");
            command.Parameters.AddWithValue("@active", active);
            command.Parameters.AddWithValue("@userName", username);
            command.Parameters.AddWithValue("@password", password);

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        /// <summary>
        ///     Adds a new user to Database.
        /// </summary>
        public bool AddUser(int id, string name, string contactInfo, DateTime joinDateTime, bool active, string username, string password, string typeName)
        {
            const string sqlStatement =
                "CALL AddUser(@id, @name, @contactInfo, @joinDateTime, @active, @username, @password, @typeName)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@contactInfo", contactInfo);
            command.Parameters.AddWithValue("@joinDateTime", joinDateTime);
            command.Parameters.AddWithValue("@active", active);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@typeName", typeName);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                log.Error("couldn't add user,"+ex.Message+"userID :"+id);
                return false;
            }
        }


        // update existing user in database
        public bool UpdateUser(int id, string name, string contactInfo, DateTime joinDateTime, bool active, string username, string password, string typeName)
        {
            // Update employees table with updated user object
            UpdateEmployee(id, name, contactInfo, joinDateTime, active);

            const string sqlStatement =
                "UPDATE Users " +
                "SET Type_ID = (SELECT Type_ID FROM UserTypes WHERE TypeName = @typeName), Username = @username, Password = @password " +
                "WHERE (User_ID = @uid)";

            var command = new MySqlCommand(sqlStatement, _connection);
            command.Parameters.AddWithValue("@uid", id);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@typeName", typeName);

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                log.Debug(ex.Message);
                return false;
            }
        }

        public IEnumerable<string> GetUserTypes()
        {
            const string sqlStatement = "SELECT TypeName FROM UserTypes";

            var command = new MySqlCommand(sqlStatement, _connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                yield return reader["TypeName"].ToString();
            }
            reader.Close();
        }

        #endregion
    }
}