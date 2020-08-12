using System;

namespace Core.Employees.Users
{
    public class Manager : User
    {
        public Manager(int id, string name, string contactInfo, DateTime joinDateTime, bool active, string username,
            string password) : base(id, name, contactInfo, joinDateTime, active, username, password)
        {
            TypeName = "Manager";
    }

        //public bool AddEmployee()
        //{
        //    //add normal workers to DB
        //}

        //public bool AddCreditCustomer(string customerName, string contactNo)
        //{
        //    // add credit customer to DB
        //}

        //public bool EnterEmployeeDetails(string empName, string nic)
        //{
        //}

        //public bool EnterAccountDetails(string username, string password)
        //{
        //}

        //public bool SaveEmployee(string empName, string nic)
        //{
        //    //save employee details in database once created (non-system user)
        //}

        //public bool SaveEmployee(string empName, string nic, string username, string password)
        //{
        //    //save employee details in database once created  (system user)
        //}

        //public bool RequestPayEmployee()
        //{
        //    //request window to record employee payments
        //}

        //public bool AddEmployeeToPayList(string empName)
        //{
        //    //add selected employees to a list
        //}

        //public bool Search(string empName, string empId)
        //{
        //    //serach for employee by employee id and name
        //}

        //public bool SavePayment(int employeeId, decimal amount, DateTime dateTime, string note)
        //{
        //    //save payments done to employees, once enetered
        //    //can call EmpPayment method
        //}

        //public bool ArchiveEmployee(string empId)
        //{
        //    //archive an employee when an emmployee no longer works
        //}

        //public Employee SelectEmployee(int employeeId)
        //{
        //    return null;
        //    //select employees to be updated
        //}
    }
}