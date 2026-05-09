using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
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
                    Console.WriteLine("Price Can’t be in negative ");
                }
            }
            get { return Price; }
        }

        public string PRODUCT_DESCRIPTION
        {
            set { Product_Description = value; }
            get { return Product_Description; }
        }
        public void SaveToFile(string filePath)
        {
            StreamWriter writer = new StreamWriter(filePath, true);
            writer.WriteLine(Id);
            writer.WriteLine(Name);
            writer.WriteLine(Price);
            writer.WriteLine(Product_Description);
            writer.WriteLine();
            writer.Close();
        }

        public void DisplayAllFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No product data found");
                return;
            }

            StreamReader reader = new StreamReader(filePath);

            while (!reader.EndOfStream)
            {
                string id = reader.ReadLine();

                if (id == "")
                    continue;

                string name = reader.ReadLine();
                string price = reader.ReadLine();
                string productDescription = reader.ReadLine();

                Console.WriteLine("ID: " + id + " Name: " + name + " Price: " + price + " Product Description: " + productDescription);
            }

            reader.Close();
        }

        public void LoadFromFile(string filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            ID = int.Parse(reader.ReadLine());
            NAME = reader.ReadLine();
            PRICE = double.Parse(reader.ReadLine());
            PRODUCT_DESCRIPTION = reader.ReadLine();
            reader.Close();
        }


        public virtual void DisplayInfo()
        {
            Console.WriteLine("\n"+"ID: " + Id + " name:" + Name + " Product Discreption is: " + Product_Description + " Price:" + Price );

        }
    }
}
