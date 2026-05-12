using System;
using System.IO;

namespace Project
{
    public abstract class Payment : Order
    {
        private static string FileName = "payment.txt";

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

        public static bool IsOrderPaid(string[] paymentData, int paymentCount, int orderNumber)
        {
            for (int i = 0; i < paymentCount; i++)
            {
                string[] data = paymentData[i].Split(',');

                if (data.Length > 0 && data[0] == orderNumber.ToString())
                    return true;
            }

            return false;
        }

        public static void PayForOrder(Customer customer, string[] orderData, int orderCount, Payment[] payments, string[] paymentData, ref int paymentCount)
        {
            if (!Customer.HasCustomer(customer))
                return;

            if (paymentCount >= payments.Length)
            {
                Console.WriteLine("Payment list is full.");
                return;
            }

            Order.DisplayMyOrders(customer, orderData, orderCount);
            Console.Write("Enter order number to pay: ");
            int orderNumber = ConsoleInput.ReadInt();

            if (orderNumber < 1 || orderNumber > orderCount)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            if (IsOrderPaid(paymentData, paymentCount, orderNumber))
            {
                Console.WriteLine("This order is already paid.");
                return;
            }

            string[] data = orderData[orderNumber - 1].Split(',');
            if (data.Length < 10 || data[0] != customer.ID.ToString())
            {
                Console.WriteLine("Order not found.");
                return;
            }

            Product product = new Product();
            product.ID = int.Parse(data[4]);
            product.NAME = data[5];
            product.PRICE = double.Parse(data[6]);
            product.PRODUCT_DESCRIPTION = data[7];

            double shippingCost = double.Parse(data[8]);
            double discount = double.Parse(data[9]);

            Console.WriteLine("1. Credit Card");
            Console.WriteLine("2. Cash On Delivery");
            Console.Write("Choose payment method: ");
            int paymentType = ConsoleInput.ReadInt();

            Payment payment;
            if (paymentType == 1)
            {
                Console.Write("Enter 16 digit card number: ");
                string cardNumber = ConsoleInput.ReadCardNumber();
                Console.Write("Enter your card balance: ");
                double balance = ConsoleInput.ReadDouble();
                payment = new CreditCardPayment(customer, product, shippingCost, discount, cardNumber, balance);
            }
            else if (paymentType == 2)
            {
                payment = new CashOnDelivery(customer, product, shippingCost, discount);
            }
            else
            {
                Console.WriteLine("Invalid payment method.");
                return;
            }

            if (!payment.IS_SUCCESSFUL)
                return;

            payments[paymentCount] = payment;
            paymentData[paymentCount] = BuildFileLine(orderNumber, orderData[orderNumber - 1], payment.PAYMENT_METHOD, payment.AMOUNT);
            paymentCount++;
            Console.WriteLine("Payment saved successfully.");
        }

        public static void DisplayMyPayments(Customer customer, string[] paymentData, int paymentCount)
        {
            if (!Customer.HasCustomer(customer))
                return;

            Console.WriteLine("\n--- My Payments ---");
            bool found = false;

            for (int i = 0; i < paymentCount; i++)
            {
                string[] data = paymentData[i].Split(',');

                if (data.Length >= 13 && data[1] == customer.ID.ToString())
                {
                    Console.WriteLine("Order Number: " + data[0]);
                    Console.WriteLine("Product: " + data[6]);
                    Console.WriteLine("Payment Method: " + data[11]);
                    Console.WriteLine("Amount: " + data[12]);
                    Console.WriteLine();
                    found = true;
                }
            }

            if (!found)
                Console.WriteLine("You have no payments.");
        }

        public static bool IsValidFileLine(string line)
        {
            if (line == null || line == "")
                return false;

            string[] data = line.Split(',');

            if (data.Length < 13)
                return false;

            int orderNumber = int.Parse(data[0]);
            if (orderNumber < 1)
                return false;

            double amount = double.Parse(data[12]);
            return amount >= 0;
        }

        public static void SavePaymentsToFile(string[] paymentData, int paymentCount, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    for (int i = 0; i < paymentCount; i++)
                    {
                        if (paymentData[i] != null && paymentData[i] != "")
                            writer.WriteLine(paymentData[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving payment data: " + ex.Message);
            }
        }

        public static void SavePaymentsToFile(string[] paymentData, int paymentCount)
        {
            SavePaymentsToFile(paymentData, paymentCount, FileName);
        }

        public static int LoadPaymentsFromFile(string[] paymentData, string filePath)
        {
            int paymentCount = 0;

            try
            {
                if (!File.Exists(filePath))
                    return paymentCount;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null && paymentCount < paymentData.Length)
                    {
                        try
                        {
                            if (IsValidFileLine(line))
                                paymentData[paymentCount++] = line;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading payment data: " + ex.Message);
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
                if (card[i] < '0' || card[i] > '9')
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
