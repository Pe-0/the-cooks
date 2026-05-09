using System;

namespace Project
{
    public abstract class Payment :Order
    {
        protected double Amount;
       

        public Payment()
        {
            Amount = 0;
            
        }

        public Payment(Customer cust, Product pro, double shippingCost, double discount) :base(cust,  pro,  shippingCost, discount)
        {
            Amount = CalculateTotal(shippingCost, discount);
        }

    }

    public class CreditCardPayment : Payment
    {
        private string CardNumber;
        private double Balance;
        

        public CreditCardPayment() : base()
        {
            CardNumber = "0000000000000000";
            Balance = 100000;
        }

        public CreditCardPayment(Customer cust, Product pro, double shippingCost, double discount,string card) :base( cust,  pro,  shippingCost, discount)
        {
            if ( card.Length == 16)
                CardNumber = card;
            else
            {
                CardNumber = "0000000000000000";
                Console.WriteLine("Error: Card number must be 16 digits.");
            }
           
            Balance = 100000;

            if (Balance >= Amount)
            {
                

                Balance -= Amount;
                Console.WriteLine("Payment successful. Remaining balance: " + Balance+"\nWe will contact you soon....");
            }
            else
            {
                
                Console.WriteLine("Error: Insufficient funds.");
            }
        }
      

    }

    public class CashOnDelivery : Payment
    {
        private string Address;
        private double Balance;
        public CashOnDelivery() : base()
        {
            Address = "Unknown";
        }

        public CashOnDelivery(Customer cust, Product pro, double shippingCost, double discount) : base(cust, pro, shippingCost, discount)
        {
            Address = customer.ADDRESS;
            Balance = 100000;

            if (Balance >= Amount)
            {


                Balance -= Amount;
                Console.WriteLine("Payment successful. Remaining balance: " + Balance + "\nWe will contact you soon....");
            }
            else
            {

                Console.WriteLine("Error: Insufficient funds.");
            }
        }


       
    }
}
