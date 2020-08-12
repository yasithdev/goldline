using System;
using System.Collections.Generic;
using System.Reflection;
using DataLayer.Employees;
using log4net;

namespace Core.Employees
{
    public class Employee : Entity
    {
        private static readonly EmployeeDb EmployeeDb = new EmployeeDb();
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Employee(int id, string name, string contactInfo, DateTime joinDateTime, bool active)
            : base(id, name, contactInfo)
        {
            JoinDateTime = joinDateTime;
            Active = active;
            IsUser = false;
        }

        public DateTime JoinDateTime { get; set; }
        public bool Active { get; set; }
        public bool IsUser { get; protected set; }
        public string LastPaymentDate { get; private set; }

        public static IEnumerable<Employee> GetEmployees(string id = "%", string name = "", string contactInfo = "",
            string joinDateTime = "", string active = "%", string lastPaymentDate = "")
        {
            var reader = EmployeeDb.GetEmployees(id, name, contactInfo, joinDateTime, active).CreateDataReader();
            while (reader.Read())
            {
                yield return new Employee(
                    int.Parse(reader["Employee_ID"].ToString()),
                    reader["Name"].ToString(),
                    reader["ContactInfo"].ToString(),
                    Convert.ToDateTime(reader["JoinDateTime"].ToString()).Date,
                    bool.Parse(reader["Active"].ToString())
                    )
                {
                    LastPaymentDate = reader["LastPaymentDate"].ToString(),
                    IsUser = reader["IsUser"].ToString() == "1"
                };
            }
        }

        public static Employee GetEmployeeById(int id) //return a single Employee given the employee id
        {
            var reader = EmployeeDb.GetEmployees(id: id.ToString()).CreateDataReader();
            if (reader.Read())
            {
                return new Employee(
                    int.Parse(reader["Employee_ID"].ToString()),
                    reader["Name"].ToString(),
                    reader["ContactInfo"].ToString(),
                    Convert.ToDateTime(reader["JoinDateTime"].ToString()).Date,
                    bool.Parse(reader["Active"].ToString())
                    ) {LastPaymentDate = reader["LastPaymentDate"].ToString(), IsUser = reader["IsUser"].ToString() == "1"};
            }
            Logger.Debug("No employees returned by GetEmployeesId(). Id Entered : " + id);
            return null;
        }

        public static bool AddEmployee(Employee employee)
        {
            return EmployeeDb.AddEmployee(employee.Id, employee.Name, employee.ContactInfo, employee.JoinDateTime,
                employee.Active);
        }

        public static bool UpdateEmployee(Employee employee)
        {
            return EmployeeDb.UpdateEmployee(employee.Id, employee.Name, employee.ContactInfo, employee.JoinDateTime,
                employee.Active);
        }

        public static bool DeleteEmployee(Employee employee)
        {
            return EmployeeDb.DeleteEmployee(employee.Id);
        }

        public static int GetNextEmployeeId()
        {
            return EmployeeDb.GetMaxEmployeeId() + 1;
        }
    }
}