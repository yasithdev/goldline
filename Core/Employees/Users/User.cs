using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Employees.Users;

namespace Core.Employees.Users
{
    public abstract class User : Employee
    {
        public static UserDb UserDb = new UserDb();

        protected User(int id, string name, string contactInfo, DateTime joinDateTime, bool active, string username,
            string password) : base(id, name, contactInfo, joinDateTime, active)
        {
            Username = username;
            Password = password;
            IsUser = true;
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string TypeName { get; set; } //set by derived classes

        public static User TryLogin(string username, string password)
        {
            var users = GetUsers(username: username, password: password, active:"1").ToArray();
            return users.Length == 1 ? users[0] : null;
        }

        public static bool Logout(User user)
        {
            // record logout time in log;
            return true;
        }

        public static IEnumerable<string> GetUserTypes()
        {
            return UserDb.GetUserTypes();
        }


        public static IEnumerable<User> GetUsers(string id = "%", string name = "", string contactInfo = "",
            string joinDateTime = "", string active = "%", string username = "%", string password = "%",
            string typeName = "%")
        {
            var reader =
                UserDb.GetUsers(id, name, contactInfo, joinDateTime, active, username, password, typeName)
                    .CreateDataReader();

            while (reader.Read())
            {
                // create manager, inventory manager or cashier objects depending on the retrieved data
                var eId = int.Parse(reader["Employee_ID"].ToString());
                var eName = reader["Name"].ToString();
                var eContact = reader["ContactInfo"].ToString();
                var eJoinDt = Convert.ToDateTime(reader["JoinDateTime"].ToString());
                var eActive = bool.Parse(reader["Active"].ToString());
                var eUsername = reader["Username"].ToString();
                var ePassword = reader["Password"].ToString();

                // Create specific user objects depending on type declared
                switch (reader["TypeName"].ToString())
                {
                    case "Manager":
                    {
                        yield return new Manager(eId, eName, eContact, eJoinDt, eActive, eUsername, ePassword);
                        break;
                    }
                    case "Inventory Manager":
                    {
                        yield return new InventoryManager(eId, eName, eContact, eJoinDt, eActive, eUsername, ePassword);
                        break;
                    }
                    case "Cashier":
                    {
                        yield return new Cashier(eId, eName, eContact, eJoinDt, eActive, eUsername, ePassword);
                        break;
                    }
                }
            }
        }

        public static bool IsUserNameAvailable(string uname)
        {
            var reader = UserDb.GetUsers(username : uname).CreateDataReader();
            return !reader.HasRows;
        }

        public static bool AddUser(User user)
        {
            return UserDb.AddUser(user.Id, user.Name, user.ContactInfo, user.JoinDateTime, user.Active, user.Username,user.Password, user.TypeName);
        }

        public static bool UpdateUser(User user)
        {
            return UserDb.UpdateUser(user.Id, user.Name, user.ContactInfo, user.JoinDateTime, user.Active, user.Username,user.Password, user.TypeName);
        }
    }
}