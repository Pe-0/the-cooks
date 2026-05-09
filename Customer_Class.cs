using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    public  class Customer : ICustomer
    {
        //Peter Part
        protected string Name;
        protected string Address;
        protected int Id;
        private static int CustomerCount = 0;
        protected string Phone;

        public Customer()//constructor without parameters + Customer Counter
        {
            Name = "not null";
            Address = "Luxor";
            Id = 500;
            Phone = "01000000000";
            CustomerCount++;
        }
        public Customer(string Name, string Addrress, int Id, string Phone)//constructor with parameters + CustomerCount
        {
            NAME = Name;
            ADDRESS = Addrress;
            ID = Id;
            PHONE = Phone;
            CustomerCount++;
        }
        public string NAME //Name property
        {
            set { Name = value; }
            get { return Name; }
        }
        public string ADDRESS //Address property
        {
            set { Address = value; }
            get { return Address; }
        }
        public int ID //id property
        {
            set
            {
                if (value > 0)
                    Id = value;
                else
                {
                    Id = 500;
                    Console.WriteLine("Error ID input ");
                }
            }
            get { return Id; }
        }
        public string PHONE
        {
            set
            {
                try
                {
                    if (value.Length == 11 && value[0] == '0' && value[1] == '1' && (value[2] == '0' || value[2] == '1' || value[2] == '2' || value[2] == '5'))
                        Phone = value;
                    else
                        throw new Exception("Invalid phone number format");
                }
                catch
                {
                    Console.WriteLine("Wrong phone number");
                    Phone = "01000000000";
                }
            }
            get { return Phone; }
        }
        public void SaveToFile(string filePath)
        {
            StreamWriter writer = new StreamWriter(filePath, true);
            writer.WriteLine(Name);
            writer.WriteLine(Address);
            writer.WriteLine(Id);
            writer.WriteLine(Phone);
            writer.WriteLine();
            writer.Close();
        }

        public void DisplayAllFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No customer data found");
                return;
            }

            StreamReader reader = new StreamReader(filePath);

            while (!reader.EndOfStream)
            {
                string name = reader.ReadLine();

                if (name == "")
                    continue;

                string address = reader.ReadLine();
                string id = reader.ReadLine();
                string phone = reader.ReadLine();

                Console.WriteLine("Name: " + name + " Address: " + address + " ID: " + id + " Phone: " + phone);
            }

            reader.Close();
        }

        public void LoadFromFile(string filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            NAME = reader.ReadLine();
            ADDRESS = reader.ReadLine();
            ID = int.Parse(reader.ReadLine());
            PHONE = reader.ReadLine();
            Console.WriteLine("Customer data loaded from file successfully." + "Your Name is :" + NAME + " Your Address is :" + ADDRESS + " Your ID is :" + ID + " Your Phone Number is :" + PHONE);
            reader.Close();
        }

        public virtual void Display()//Customer Info
        {
            Console.WriteLine("Your Info");
            Console.WriteLine("Your Name is : " + Name + "  Your Address is : " + Address + "   Your ID is : " + ID + "   Your Phone Number is : " + Phone);
        }
        public static void NumCustomer() // CustomerCount
        {
            Console.WriteLine("The Number of Customer is : " + CustomerCount);
        }

    }
}
