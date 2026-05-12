using System;

namespace Project
{
    public class Product : IProduct
    {
        protected int Id;
        protected string Name;
        protected double Price;
        protected string Product_Description;

        public Product()
        {
            Id = 0;
            Name = "null";
            Price = 0;
            Product_Description = "null";
        }

        public Product(int Id, string name, double Price, string Product_Description)
        {
            ID = Id;
            NAME = name;
            PRICE = Price;
            PRODUCT_DESCRIPTION = Product_Description;
        }

        public int ID
        {
            set
            {
                if (value >= 0)
                {
                    this.Id = value;
                }
                else
                {
                    this.Id = 0;
                    Console.WriteLine("Error in ID number");
                }
            }
            get { return Id; }
        }

        public string NAME
        {
            set { Name = value; }
            get { return Name; }
        }

        public double PRICE
        {
            set
            {
                if (value >= 0)
                    Price = value;
                else
                {
                    Price = 0;
                    Console.WriteLine("Price can't be negative.");
                }
            }
            get { return Price; }
        }

        public string PRODUCT_DESCRIPTION
        {
            set { Product_Description = value; }
            get { return Product_Description; }
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine("\nID: " + Id + " Name: " + Name + " Product Description: " + Product_Description + " Price: " + Price);
        }
    }
}
