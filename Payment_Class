using System;

namespace Project
{
    public class Payment
    {
        protected double Amount;
        protected Product product;

        public Payment()
        {
            Amount = 0;
            product = null;
        }

        public Payment(Product p)
        {
            product = p;
            Amount = p.PRICE;
        }

        public virtual void Display()
        {
            Console.WriteLine("Payment Amount: " + Amount);
        }

        public void DeliveryInfo(string address)
        {
            Console.WriteLine("Delivery Address: " + address);
        }

        public virtual void ProcessPayment()
        {
            Console.WriteLine("Processing Payment...");
        }
    }

    public class CreditCardPayment : Payment
    {
        private string[] CardNumber;
        private double Balance;

        public CreditCardPayment() : base()
        {
            CardNumber = new string[16];
            for (int i = 0; i < 16; i++)
                CardNumber[i] = "0";

            Balance = 1000;
        }

        public CreditCardPayment(Product p, string[] card) : base(p)
        {
            if (card != null && card.Length == 16)
                CardNumber = card;
            else
            {
                CardNumber = new string[16];
                Console.WriteLine("Error: Card number must be 16 digits.");
            }

            Balance = 1000;

            if (Balance >= Amount)
            {
                Balance -= Amount;
                Console.WriteLine("Payment successful. Remaining balance: " + Balance);
            }
            else
            {
                Console.WriteLine("Error: Insufficient funds.");
            }
        }

        public override void ProcessPayment()
        {
            Console.Write("Card Number: ");
            for (int i = 0; i < CardNumber.Length; i++)
                Console.Write(CardNumber[i]);

            Console.WriteLine();
            Console.WriteLine("Amount: " + Amount);
        }

        public override void Display()
        {
            base.Display();

            Console.Write("Card Number: ");
            for (int i = 0; i < CardNumber.Length; i++)
                Console.Write(CardNumber[i]);

            Console.WriteLine();
            Console.WriteLine("Balance: " + Balance);
        }
    }

    public class CashOnDelivery : Payment
    {
        private string Address;

        public CashOnDelivery() : base()
        {
            Address = "Unknown";
        }

        public CashOnDelivery(Product p, string address) : base(p)
        {
            Address = address;
        }

        public override void ProcessPayment()
        {
            Console.WriteLine("Cash on Delivery");
            Console.WriteLine("Amount: " + Amount);
            Console.WriteLine("Address: " + Address);
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine("Address: " + Address);
        }
    }
}
