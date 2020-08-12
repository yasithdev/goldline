using System;
using Core.Employees;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Employees
{
    [TestClass()]
    public class EmployeeTest
    {
        [TestCleanup]
        public void After_Method()
        {
            Console.WriteLine("Method Executed!");
        }

        [ClassCleanup]
        public static void After_Class()
        {
            Console.WriteLine("EmployeeTest Class Executed");
        }
        [TestMethod()]
        public void AddEmployeeTest()
        {
            Employee em = new Employee(100, "testEmp", "0114477885", System.DateTime.Now, true);
            Employee.AddEmployee(em);
        }
        [TestMethod()]
        public void GetEmployeesTest()
        {
            Assert.IsNotNull(Employee.GetEmployees());
        }

        [TestMethod()]
        public void GetNextEmployeeIdTest()
        {
            Assert.IsNotNull(Employee.GetNextEmployeeId());
        }

       [TestMethod()]
        public void GetEmployeeByIdTest()
        {
            Assert.IsNotNull(Employee.GetEmployeeById(1));
        }

        [TestMethod()]
        public void UpdateEmployeeTest()
        {
            Employee em = new Employee(100, "testEmployeeNameChange", "0114477885", System.DateTime.Now, true);
            Employee.UpdateEmployee(em);
        }

    }
}


