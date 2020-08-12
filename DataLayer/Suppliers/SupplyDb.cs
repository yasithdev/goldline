using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataLayer.Suppliers
{
    public class SupplyDb
    {
        protected readonly MySqlConnection Connection;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SupplyDb()
        {
            Connection = new MySqlConnection(ConnectionStrings.LocalConnString);
            try
            {
                Connection.Open();
            }
            catch (MySqlException e)
            {
                Log.Error("Connection open error:",e);
            }
        }

        ~SupplyDb()
        {
            try
            {
                Connection.Close();
            }
            catch (MySqlException e)
            {
                Log.Error("Connection close error:",e);
            }
        }

        #region Suppliers

        public DataTable GetSuppliers(string id = "%", string name = "")
        {
            const string sqlStatement =
                "SELECT * FROM Suppliers WHERE Supplier_ID LIKE @id AND Name LIKE @name";
            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", "%" + name + "%");

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public bool UpdateSupplier(string id, string name, string contactInfo)
        {
            const string sqlStatement =
                "UPDATE Suppliers SET Name = @name,ContactInfo = @contactInfo " +
                "WHERE Supplier_ID LIKE @id";
            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@contactInfo", contactInfo);
            command.Parameters.AddWithValue("@id", id);

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                Log.Info("Update Supplier:",e);
                return false;
            }
        }

        public bool AddNewSupplier(int id, string name, string contactInfo)
        {
            const string sqlStatement =
                "INSERT INTO Suppliers(Supplier_ID, Name, ContactInfo) " +
                "VALUES(@id, @name, @contactInfo)";
            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@contactInfo", contactInfo);

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                Log.Info("Add new supplier:",e);
                return false;
            }
        }

        public bool RemoveSupplier(int id)
        {
            const string sqlStatement =
                "DELETE FROM Suppliers WHERE Supplier_ID LIKE @id";
            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@id", id);

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }

        public int GetMaxSupplierId()
        {
            const string sqlStatement = "SELECT MAX(Supplier_ID) FROM Suppliers";
            var command = new MySqlCommand(sqlStatement, Connection);

            var maxId = command.ExecuteScalar().ToString();
            return maxId == "" ? 0 : int.Parse(maxId);
        }

        #endregion

        #region Suppliers Connect With SuppliedItems

        public bool AddSuppliedItem(int supplierId, int itemId)
        {
            const string sqlStatement =
                "INSERT INTO SuppliedItems(Supplier_ID, Item_ID) " +
                "VALUES(@supplierId, @itemId)";
            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@supplierId", supplierId);
            command.Parameters.AddWithValue("@itemId", itemId);

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                Log.Info("Add new supplied item:", e);
                return false;
            }
        }

        public bool RemoveSuppliedItem(int supplierId, int itemId)
        {
            const string sqlStatement =
                "DELETE FROM SuppliedItems WHERE Supplier_ID LIKE @supplierId AND Item_ID LIKE @itemId";

            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@supplierId", supplierId);
            command.Parameters.AddWithValue("@itemId", itemId);

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                Log.Info("Remove supplied Item:",e);
                return false;
            }
        }

        #endregion
    }
}