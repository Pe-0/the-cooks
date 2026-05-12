using System;
using System.IO;

namespace Project
{
    public abstract class Payment : Order
    {
        private static readonly string FolderPath = @"C:\Users\peter\source\repos\ConsoleApp5\Data";
        private static readonly string FileName = Path.Combine(FolderPath, "payment.txt");

        protected double Amount;
        protected double Balance;
        protected string PaymentMethod;
        protected bool Successful;

        public Payment()
        {
            Amount = 0;
            Balance = 0;
            PaymentMethod = "Unknown";
            Successful = false;
        }

        public Payment(Customer cust, Product pro, double shippingCost, double discount) : base(cust, pro, shippingCost, discount)
        {
            Amount = CalculateTotal(shippingCost, discount);
            Balance = 0;
            PaymentMethod = "Unknown";
            Successful = false;
        }

        public double AMOUNT
        {
            get { return Amount; }
        }

        public string PAYMENT_METHOD
        {
            get { return PaymentMethod; }
        }

        public bool IS_SUCCESSFUL
        {
            get { return Successful; }
        }

        protected void PayFromBalance(double balance)
        {
            Balance = balance;

            if (Balance >= Amount)
            {
                Balance -= Amount;
                Successful = true;
                Console.WriteLine("Payment successful. Remaining balance: " + Balance + "\nWe will contact you soon....");
            }
            else
            {
                Successful = false;
                Console.WriteLine("Error: Insufficient funds.");
            }
        }

        public static string BuildFileLine(int orderNumber, string orderLine, string paymentMethod, double amount)
        {
            return orderNumber + "," + orderLine + "," + paymentMethod + "," + amount;
        }

        public static bool TryParseFileLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return false;

            string[] data = line.Split(',');

            if (data.Length < 13)
                return false;

            if (!int.TryParse(data[0], out int orderNumber))
                return false;

            if (orderNumber < 1)
                return false;

            if (!double.TryParse(data[12], out double amount))
                return false;

            return amount >= 0;
        }

        public static void SavePaymentsToFile(string[] paymentData, int paymentCount, string filePath)
        {
            Directory.CreateDirectory(FolderPath);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < paymentCount; i++)
                {
                    if (!string.IsNullOrWhiteSpace(paymentData[i]))
                        writer.WriteLine(paymentData[i]);
                }
            }
        }

        public static void SavePaymentsToFile(string[] paymentData, int paymentCount)
        {
            SavePaymentsToFile(paymentData, paymentCount, FileName);
        }

        public static int LoadPaymentsFromFile(string[] paymentData, string filePath)
        {
            int paymentCount = 0;

            if (!File.Exists(filePath))
                return paymentCount;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null && paymentCount < paymentData.Length)
                {
                    if (TryParseFileLine(line))
                        paymentData[paymentCount++] = line;
                }
            }

            return paymentCount;
        }

        public static int LoadPaymentsFromFile(string[] paymentData)
        {
            return LoadPaymentsFromFile(paymentData, FileName);
        }
    }

    public class CreditCardPayment : Payment
    {
        private string CardNumber;

        public CreditCardPayment() : base()
        {
            CardNumber = "0000000000000000";
            PaymentMethod = "Credit Card";
        }

        public CreditCardPayment(Customer cust, Product pro, double shippingCost, double discount, string card, double balance) : base(cust, pro, shippingCost, discount)
        {
            PaymentMethod = "Credit Card";

            if (!IsValidCardNumber(card))
            {
                CardNumber = "0000000000000000";
                Successful = false;
                Console.WriteLine("Error: Card number must be 16 digits.");
                return;
            }

            CardNumber = card;
            PayFromBalance(balance);
        }

        private bool IsValidCardNumber(string card)
        {
            if (card.Length != 16)
                return false;

            for (int i = 0; i < card.Length; i++)
            {
                if (!char.IsDigit(card[i]))
                    return false;
            }

            return true;
        }
    }

    public class CashOnDelivery : Payment
    {
        private string Address;

        public CashOnDelivery() : base()
        {
            Address = "Unknown";
            PaymentMethod = "Cash On Delivery";
        }

        public CashOnDelivery(Customer cust, Product pro, double shippingCost, double discount) : base(cust, pro, shippingCost, discount)
        {
            PaymentMethod = "Cash On Delivery";
            Address = customer.ADDRESS;
            Successful = true;
            Console.WriteLine("Cash on delivery selected. Please pay when the order arrives.\nWe will contact you soon....");
        }
    }
}
