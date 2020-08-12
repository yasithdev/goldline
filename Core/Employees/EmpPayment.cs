using System;
using System.Collections.Generic;
using DataLayer.Employees;

namespace Core.Employees
{
    public class EmpPayment
    {
        private static readonly EmployeeDb EmployeeDb = new EmployeeDb();

        public EmpPayment(int id, int employeeId, decimal amount, DateTime dateTime, string note)
        {
            Id = id;
            EmployeeId = employeeId;
            Amount = amount;
            DateTime = dateTime;
            Note = note;
        }

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
        public string Note { get; set; }


        public static IEnumerable<EmpPayment> GetEmpPayments(string id = "%", string employeeId = "%", string amount = "",
            string dateTime = "", string note = "")
        {
            var reader = EmployeeDb.GetEmpPayments(id, employeeId, amount, dateTime, note).CreateDataReader();

            while (reader.Read())
            {
                yield return new EmpPayment(
                    int.Parse(reader["Payment_ID"].ToString()),
                    int.Parse(reader["Employee_ID"].ToString()),
                    decimal.Parse(reader["Amount"].ToString()),
                    Convert.ToDateTime(reader["DateTime"].ToString()),
                    reader["Note"].ToString()
                    );
            }
        }

        public static bool AddEmpPayment(EmpPayment empPayment)
        {
            return EmployeeDb.AddEmpPayment(empPayment.Id, empPayment.EmployeeId, empPayment.Amount,
                empPayment.DateTime,
                empPayment.Note);
        }

        public static bool UpdateEmpPayment(EmpPayment empPayment)
        {
            return EmployeeDb.UpdateEmpPayment(empPayment.Id, empPayment.EmployeeId, empPayment.Amount,
                empPayment.DateTime.ToShortDateString(),
                empPayment.Note);
        }

        public static int GetNextEmployeePaymentId()
        {
            return EmployeeDb.GetMaxEmployeePaymentId() + 1;
        }
    }
}