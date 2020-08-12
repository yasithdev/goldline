using System;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace DataLayer.Inventory
{
    public class InventoryDb
    {
        protected readonly MySqlConnection Connection;
        protected static MySqlTransaction Transaction;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public InventoryDb()
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

        ~InventoryDb()
        {
            try
            {
                Connection.Close();
            }
            catch (MySqlException e)
            {
                log.Info("Connection close error:",e);
            }
        }

        #region Get

        public int GetMaxId()
        {
            const string sqlStatement = "SELECT MAX(Product_ID) FROM Products";
            var command = new MySqlCommand(sqlStatement, Connection);
            var maxId = command.ExecuteScalar().ToString();
            return (maxId == "") ? 0 : int.Parse(maxId);
        }

        public DataTable GetItems(string id = "%", string name = "")
        {
            const string sqlStatement =
                "SELECT * FROM ItemsView " +
                "WHERE Item_ID LIKE @id AND ProductName LIKE @name";

            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", "%" + name + "%");

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public DataTable GetSupplierItems(string supplierId = "%", string itemName = "")
        {
            const string sqlStatement =
                "SELECT * FROM SupplierItemsView " +
                "WHERE Supplier_ID LIKE @supplierId AND " +
                "ProductName LIKE @itemName";

            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@supplierId", supplierId);
            command.Parameters.AddWithValue("@itemName", "%" + itemName + "%");

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public DataTable GetItemTypes()
        {
            const string sqlStatement =
                "SELECT ItemTypes.TypeName FROM ItemTypes";

            var command = new MySqlCommand(sqlStatement, Connection);
            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public string GetProductIdByName(string name)
        {
            const string sqlStatement =
                "SELECT Product_ID FROM Products WHERE ProductName LIKE @name";

            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@name", name);

            return command.ExecuteScalar().ToString();
        }

        public DataTable GetTyres(string id = "%", string name = "")
        {
            const string sqlStatement =
                "SELECT * FROM TyresView " +
                "WHERE Item_ID LIKE @id AND ProductName LIKE @name";

            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", "%" + name + "%");

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public DataTable GetAlloyWheels(string id = "%", string name = "")
        {
            const string sqlStatement =
                "SELECT * FROM AlloyWheelsView " +
                "WHERE Item_ID LIKE @id AND ProductName LIKE @name";

            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", "%" + name + "%");

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public DataTable GetBatteries(string id = "%", string name = "")
        {
            const string sqlStatement =
                "SELECT * FROM BatteriesView " +
                "WHERE Item_ID LIKE @id AND ProductName LIKE @name";

            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", "%" + name + "%");

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        public DataTable GetServices(string id = "%", string name = "")
        {
            const string sqlStatement =
                "SELECT * FROM ServicesView " +
                "WHERE Service_ID LIKE @id AND ProductName LIKE @pName";

            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@pName", "%" + name + "%");

            var dataTable = new DataTable();
            new MySqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        #endregion

        #region Update

        public bool UpdateStockDetails(int itemId, int stock)
        {
            const string sqlStatement =
                "Update Items SET Items.Stock = Items.Stock + @stock WHERE Items.Item_ID LIKE @id";
            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@stock", stock);
            command.Parameters.AddWithValue("@id", itemId);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                if (e.Number == 1062) return false;
                throw;
            }
        }

        private void UpdateItemInfo(int id, string name, int stock, decimal price)
        {
            const string sqlStatement1 = "UPDATE Products SET ProductName = @name WHERE Product_ID = @id";
            var command1 = new MySqlCommand(sqlStatement1, Connection);
            command1.Parameters.AddWithValue("@name", name);
            command1.Parameters.AddWithValue("@id", id);

            const string sqlStatement2 = "UPDATE Items SET Stock = @stock, Price = @price WHERE Item_ID = @id";
            var command2 = new MySqlCommand(sqlStatement2, Connection);
            command2.Parameters.AddWithValue("@id", id);
            command2.Parameters.AddWithValue("@stock", stock);
            command2.Parameters.AddWithValue("@price", price);

            command1.ExecuteNonQuery();
            command2.ExecuteNonQuery();

        }

        public bool UpdateTyreInfo(int id, string name, int stock, decimal price, string brand, string dimension,
            string country)
        {
            try
            {
                UpdateItemInfo(id, name, stock, price);

                const string sqlStatement =
                    "UPDATE Tyres SET Brand = @brand, Dimension = @dimension, Country = @country WHERE Item_ID = @id";
                var command = new MySqlCommand(sqlStatement, Connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@brand", brand);
                command.Parameters.AddWithValue("@dimension", dimension);
                command.Parameters.AddWithValue("@country", country);

                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                if (e.Number == 1062) return false;
                throw;
            }
        }

        public bool UpdateAlloyWheelInfo(int id, string name, int stock, decimal price, string brand, string dimension)
        {
            try
            {
                UpdateItemInfo(id, name, stock, price);

                const string sqlStatement =
                    "UPDATE AlloyWheels SET Brand = @brand, Dimension = @dimension WHERE Item_ID = @id";
                var command = new MySqlCommand(sqlStatement, Connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@brand", brand);
                command.Parameters.AddWithValue("@dimension", dimension);

                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                if (e.Number == 1062) return false;
                throw;
            }
        }

        public bool UpdateBatteryInfo(int id, string name, int stock, decimal price, string brand)
        {
            try
            {
                UpdateItemInfo(id, name, stock, price);

                const string sqlStatement = "UPDATE Batteries SET Brand = @brand WHERE Item_ID = @id";
                var command = new MySqlCommand(sqlStatement, Connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@brand", brand);

                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                if (e.Number == 1062) return false;
                throw;
            }
        }

        public bool UpdateServiceInfo(int id, string name)
        {
            const string sqlStatement =
                "UPDATE Products SET (Product_ID = @id, ProductName = @sname)";

            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@sName", name);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                if (e.Number == 1062) return false;
                throw;
            }
        }

        #endregion

        #region AddNewInfo
        private void AddNewItem(int id, string name, int stock, decimal price, int itemType)
        {
            const string sqlStatement1 =
                "INSERT INTO Products (ProductName, Product_ID) VALUES (@name, @id)";
            var command1 = new MySqlCommand(sqlStatement1, Connection);
            command1.Parameters.AddWithValue("@name", name);
            command1.Parameters.AddWithValue("@id", id);

            const string sqlStatement2 =
                "INSERT INTO Items (Stock, Price, Item_ID, Type_ID) VALUES (@stock, @price, @id, @itemType)";
            var command2 = new MySqlCommand(sqlStatement2, Connection);
            command2.Parameters.AddWithValue("@id", id);
            command2.Parameters.AddWithValue("@stock", stock);
            command2.Parameters.AddWithValue("@price", price);
            command2.Parameters.AddWithValue("@itemType", itemType);

            command1.Transaction = Transaction;
            command2.Transaction = Transaction;
            command1.ExecuteNonQuery();
            command2.ExecuteNonQuery();
        }

        public bool AddNewTyreInfo(int id, string name, int stock, decimal price, string brand, string dimension,
            string country)
        {
            try
            {

                Transaction = Connection.BeginTransaction();
                AddNewItem(id, name, stock, price, 1);

                const string sqlStatement =
                    "INSERT INTO Tyres(Brand, Dimension, Country, Item_ID) " +
                    "VALUES(@brand, @dimension, @country, @id)";
                var command = new MySqlCommand(sqlStatement, Connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@brand", brand);
                command.Parameters.AddWithValue("@dimension", dimension);
                command.Parameters.AddWithValue("@country", country);

                command.Transaction = Transaction;
                command.ExecuteNonQuery();
                Transaction.Commit();
                return true;
            }
            catch (MySqlException e)
            {
                Transaction.Rollback();
                if (e.Number == 1062) return false;
                throw;
            }
        }

        public bool AddNewAlloyWheelInfo(int id, string name, int stock, decimal price, string brand, string dimension)
        {
            try
            {
                Transaction = Connection.BeginTransaction();
                AddNewItem(id, name, stock, price, 2);

                const string sqlStatement =
                    "INSERT INTO AlloyWheels(Brand, Dimension, Item_ID) " + "VALUES(@brand, @dimension, @id)";
                var command = new MySqlCommand(sqlStatement, Connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@brand", brand);
                command.Parameters.AddWithValue("@dimension", dimension);

                command.Transaction = Transaction;
                command.ExecuteNonQuery();
                Transaction.Commit();
                return true;
            }
            catch (MySqlException e)
            {
                Transaction.Rollback();
                if (e.Number == 1062) return false;
                throw;
            }
        }

        public bool AddNewBatteryInfo(int id, string name, int stock, decimal price, string brand)
        {
            try
            {
                Transaction = Connection.BeginTransaction();
                AddNewItem(id, name, stock, price, 3);

                const string sqlStatement =
                    "INSERT INTO Batteries(Brand, Item_ID) " + "VALUES(@brand, @id)";
                var command = new MySqlCommand(sqlStatement, Connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@brand", brand);

                command.Transaction = Transaction;
                command.ExecuteNonQuery();
                Transaction.Commit();
                return true;
            }
            catch (MySqlException e)
            {
                Transaction.Rollback();
                if (e.Number == 1062) return false;
                throw;
            }
        }

        public bool AddNewServiceInfo(int id, string name)
        {
            const string sqlStatement =
                "INSERT Into Products VALUES (@id, @sname)";

            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@sName", name);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                if (e.Number == 1062) return false;
                throw;
            }
        }

        #endregion
    }
}