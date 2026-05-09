using Project;
using System;
using System.Diagnostics;

namespace Project
{
    public class Order
    {
        protected Customer customer;
        protected Product product;
        protected double Discount;
        protected double ShippingCost;


        public Order()
        {
            product = new Product();
            Discount = 0;
            ShippingCost = 0;
            customer = new Customer();

        }

        public Order(Customer cust, Product pro, double shippingCost, double discount)
        {
            product.ID = pro.ID;
            product.NAME = pro.NAME;
            product.PRICE = pro.PRICE;
            DISCOUNT = discount;
            SHIPPING_COST = shippingCost;
            customer.ID = cust.ID;
            customer.NAME = cust.NAME;
            customer.PHONE = cust.PHONE;
            customer.ADDRESS = cust.ADDRESS;
        }

        public double SHIPPING_COST
        {
            set
            {
                if (value >= 0)
                    ShippingCost = value;
                else
                {
                    ShippingCost = 0;
                    Console.WriteLine("Error: Shipping cost must be non-negative");
                }
            }
            get { return ShippingCost; }
        }

        public double DISCOUNT
        {

            set
            {
                if (value >= 0)
                {
                    Discount = value;
                }

                else
                {
                    Discount = 0;
                    Console.WriteLine("Eroor: Discount must be non-negative");
                }

            }
            get { return Discount; }

        }

        public virtual double CalculateTotal(double shipping_cost, double discount)
        {
            return (product.PRICE + shipping_cost) - discount;
        }

        public void DisplayOrderInfo()
        {
            customer.Display();
            Console.WriteLine("Shipping Cost: " + ShippingCost + "\nDiscount: " + Discount + "\nFinal price: " + CalculateTotal(ShippingCost, Discount));
        }
    }
}
