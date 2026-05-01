using System;
using System.Diagnostics;

namespace Project
{
    public class Order : Customer
    {
        protected int IdP;
        protected string NameP;
        protected double PriceP;
        protected double Discount;
        private double ShippingCost;
        private string Region;

        public Order()
        {
            IdP = 0;
            NameP = "none";
            PriceP = 0;
            Discount = 0;
            ShippingCost = 0;
            Region = "Unknown";

        }

        public Order(string name, string address, int id, string phone ,string region,int idp,string namep,double pricep, double shippingCost, double discount)
            : base(name, address, id, phone)
        {
            IDP = idp;
            NAMEP = namep;
            PRICEP = pricep;
            DISCOUNT = discount;
            SHIPPING_COST = shippingCost;
            REGION = region;
        }


        public int IDP
        {
            set
            {
                if (value >= 0)
                {
                    this.IdP = value;
                }
                else
                {
                    this.IdP = 0;
                    Console.WriteLine("Error in ID number");
                }
            }
            get { return IdP; }
        }

        public string NAMEP
        {
            set { NameP = value; }
            get { return NameP; }
        }
        public double PRICEP
        {
            set
            {
                if (value >= 0)
                    PriceP = value;
                else
                {
                    PriceP = 0;
                    Console.WriteLine("Price Can’t be in negative ");
                }
            }
            get { return PriceP; }
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
                if(value >= 0)
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
        public string REGION
        {
            set { Region = value; }
            get { return Region; }
        }

        public virtual double CalculateTotal(double price,double shipping_cost, double discount)
        {
            return (price + shipping_cost) - discount;
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine("Region: " + Region + " Shipping Cost: " + ShippingCost+" Discount: "+Discount+" Final price: "+ CalculateTotal(PriceP, ShippingCost, Discount));
        }
    }
}
