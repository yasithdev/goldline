using System;
using System.Collections.Generic;
using DataLayer.Customers;

namespace Core.Customers
{
    public class CustomerPayment
    {
        private static readonly CustomerDb CustomerDb = new CustomerDb();

        public CustomerPayment(int id, int customerId, decimal amount, DateTime dateTime, int userId)
        {
            Id = id;
            CustomerId = customerId;
            Amount = amount;
            DateTime = dateTime;
            UserId = userId;
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
        public int UserId { get; set; }

        public static IEnumerable<CustomerPayment> GetCustomerPayments(string id = "%", string customerId = "%",
            string amount = "", string dateTime = "", string userId = "%")
        {
            var reader = CustomerDb.GetCustomerPayments(id, customerId, amount, dateTime, userId).CreateDataReader();

            while (reader.Read())
            {
                yield return new CustomerPayment(
                    int.Parse(reader["Payment_ID"].ToString()),
                    int.Parse(reader["Customer_ID"].ToString()),
                    decimal.Parse(reader["Amount"].ToString()),
                    DateTime.Parse(reader["DateTime"].ToString()),
                    int.Parse(reader["User_ID"].ToString())
                    );
            }
        }

        public static bool AddCustomerPayment(CustomerPayment payment)
        {
            return CustomerDb.AddCustomerPayment(payment.Id, payment.CustomerId, payment.Amount,
                payment.DateTime,
                payment.UserId);
        }

        public static bool UpdateCustomerPayment(CustomerPayment payment)
        {
            return CustomerDb.UpdateCustomerPayment(payment.Id, payment.CustomerId, payment.Amount,
                payment.DateTime,
                payment.UserId);
        }

        public static bool DeleteCustomerPayment(int paymentId)
        {
            return CustomerDb.DeleteCustomerPayment(paymentId);
        }


        public static int GetNextCustomerPaymentId()
        {
            return CustomerDb.GetMaxCustomerPaymentId() + 1;
        }
    }
}