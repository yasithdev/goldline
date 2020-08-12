using System.Collections.Generic;
using Core.Inventory;
using MySql.Data.MySqlClient;

namespace DataLayer.Inventory
{
    class InventoryDB
    {
        protected readonly MySqlConnection Connection;

        public InventoryDB()
        {
            Connection = new MySqlConnection(ConnectionStrings.LocalConnString);
        }

        public IEnumerable<Tyre> GetTyres(string id = "%" , string name = "%")
        {
            const string sqlStatement =
                "SELECT * FROM  Products " + 
                "JOIN Items ON (Products.Product_ID = Items.Item_ID)" + 
                "JOIN Tyres ON (Products.Product_ID = Tyres.Item_ID)" +
                "WHERE Product_ID LIKE @id AND Name LIKE @name";

            var command = new MySqlCommand(sqlStatement, Connection);
            command.Parameters.AddWithValue("@id", id + "%");
            command.Parameters.AddWithValue("@name", name + "%");

            Connection.Open();

            var sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                // create tyre from retrieved data
                var pId = sqlDataReader["Product_ID"].ToString();
                var pName = sqlDataReader["Name"].ToString();
                var pStock = int.Parse(sqlDataReader["Stock"].ToString());
                var pPrice = decimal.Parse(sqlDataReader["Price"].ToString());
                var pBrand = sqlDataReader["Brand"].ToString();
                var pDimension = sqlDataReader["Dimension"].ToString();
                var pCountry = sqlDataReader["Country"].ToString();

                yield return new Tyre(pId,pName,pStock,pPrice,pBrand,pDimension,pCountry);      
            }
            Connection.Close();
        }

        public void UpdateTyreInfo(Tyre tyre)
        {
            const string sqlStatement1 = "UPDATE Product " +
                "SET (Name = @name) " +
                "WHERE (Product_ID = @id)";
            const string sqlStatement2 = "UPDATE Items " +
               "SET (Stock = @stock, Price = @price) " +
               "WHERE (Item_ID = @id)";
            const string sqlStatement3 = "UPDATE Tyres " +
               "SET (Brand = @brand, Dimension = @dimension, Country = @country) " +
               "WHERE (Item_ID = @id)";

            var command1 = new MySqlCommand(sqlStatement1, Connection);
            command1.Parameters.AddWithValue("@name", tyre.Name);
            command1.Parameters.AddWithValue("@id", tyre.Id);

            var command2 = new MySqlCommand(sqlStatement2, Connection);
            command2.Parameters.AddWithValue("@stock", tyre.Stock);
            command2.Parameters.AddWithValue("@price", tyre.Price);

            var command3 = new MySqlCommand(sqlStatement3, Connection);
            command3.Parameters.AddWithValue("@brand", tyre.Brand);
            command3.Parameters.AddWithValue("@dimension", tyre.Dimension);
            command3.Parameters.AddWithValue("@country", tyre.Country);

            Connection.Open();
            command1.ExecuteNonQuery();
            command2.ExecuteNonQuery();
            command3.ExecuteNonQuery();
            Connection.Close();
        }

        public IEnumerable<string[]> GetSuggestions(string id = "%", string name = "%", string type = "%")
        {
            // give suggestions for products according to given parameters
            const string sqlStatement1 =
                "SELECT * FROM  Products " + "WHERE Product_ID LIKE @id AND Name LIKE @name";

            var command = new MySqlCommand(sqlStatement1, Connection);
            command.Parameters.AddWithValue("@id", id + "%");
            command.Parameters.AddWithValue("@name", name + "%");

            Connection.Open();

            var sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                yield return new string[] { sqlDataReader["Product_ID"].ToString(), sqlDataReader["Name"].ToString() };
            }
            Connection.Close();
        }

        public Product GetProduct(string productId, string name)
        {

            // get item from database
            const string sqlStatement1 = "SELECT * FROM Items" + "WHERE Item_ID LIKE @productId";
            const string sqlstatement2 = "SELECT * FROM ItemTypes" + "WHERE TypeId LIKE @typeId";
            const string sqlStatement3 = "SELECT * FROM @table" + "WHERE Item_ID LIKE @productId";

            Connection.Open();

            var command1 = new MySqlCommand(sqlStatement1, Connection);
            command1.Parameters.AddWithValue("@productId", productId);

            var sqlDataReader1 = command1.ExecuteReader();

            //get desired table
            var command2 = new MySqlCommand(sqlstatement2, Connection);
            command2.Parameters.AddWithValue("@typeId", sqlDataReader1["Type_ID"]);

            string type = command2.ExecuteReader()["TypeName"].ToString();

            var command3 = new MySqlCommand(sqlStatement3, Connection);
            command2.Parameters.AddWithValue("@table", type);
            command2.Parameters.AddWithValue("@productId", productId);

            var sqlDataReader2 = command3.ExecuteReader();

            //            switch (type) {
            //                case ("tyre"):
            Connection.Close();
            return new Tyre
                (sqlDataReader1["Item_ID"].ToString(),
                name,
                int.Parse(sqlDataReader1["Stock"].ToString()),
                decimal.Parse(sqlDataReader1["Price"].ToString()),
                sqlDataReader2["Brand"].ToString(),
                sqlDataReader2["Dimension"].ToString(),
                sqlDataReader2["Country"].ToString());
            //            }
        }

        public void UpdateItemInfo(Product product)
        {
            //Update item info
        }
    }
}

