using System;
using System.IO;

namespace Project
{
    public class Customer : ICustomer
    {
        private static readonly string FolderPath = @"C:\Users\peter\source\repos\ConsoleApp5\Data";
        private static readonly string FileName = Path.Combine(FolderPath, "customer.txt");

        protected string Name;
        protected string Address;
        protected int Id;
        private static int CustomerCount = 0;
        protected string Phone;

        public Customer()
        {
            Name = "not null";
            Address = "Luxor";
            Id = 500;
            Phone = "01000000000";
            CustomerCount++;
        }

        public Customer(string Name, string Address, int Id, string Phone)
        {
            NAME = Name;
            ADDRESS = Address;
            ID = Id;
            PHONE = Phone;
            CustomerCount++;
        }

        public string NAME
        {
            set { Name = value; }
            get { return Name; }
        }

        public string ADDRESS
        {
            set { Address = value; }
            get { return Address; }
        }

        public int ID
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

        public string ToFileLine()
        {
            return ID + "," + NAME + "," + ADDRESS + "," + PHONE;
        }

        public static bool TryParse(string line, out Customer customer)
        {
            customer = new Customer();

            if (string.IsNullOrWhiteSpace(line))
                return false;

            string[] data = line.Split(',');

            if (data.Length < 4)
                return false;

            if (!int.TryParse(data[0], out int id))
                return false;

            customer.ID = id;
            customer.NAME = data[1];
            customer.ADDRESS = data[2];
            customer.PHONE = data[3];
            return true;
        }

        public static void SaveAllToFile(Customer[] customers, int customerCount, string filePath)
        {
            Directory.CreateDirectory(FolderPath);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < customerCount; i++)
                    writer.WriteLine(customers[i].ToFileLine());
            }
        }

        public static void SaveAllToFile(Customer[] customers, int customerCount)
        {
            SaveAllToFile(customers, customerCount, FileName);
        }

        public static int LoadAllFromFile(Customer[] customers, string filePath)
        {
            int customerCount = 0;

            if (!File.Exists(filePath))
                return customerCount;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null && customerCount < customers.Length)
                {
                    if (TryParse(line, out Customer customer))
                        customers[customerCount++] = customer;
                }
            }

            return customerCount;
        }

        public static int LoadAllFromFile(Customer[] customers)
        {
            return LoadAllFromFile(customers, FileName);
        }

        public void SaveToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
                writer.WriteLine(ToFileLine());
        }

        public void DisplayAllFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No customer data found");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (TryParse(line, out Customer customer))
                        Console.WriteLine("Name: " + customer.NAME + " Address: " + customer.ADDRESS + " ID: " + customer.ID + " Phone: " + customer.PHONE);
                }
            }
        }

        public void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line = reader.ReadLine();
                if (TryParse(line, out Customer customer))
                {
                    NAME = customer.NAME;
                    ADDRESS = customer.ADDRESS;
                    ID = customer.ID;
                    PHONE = customer.PHONE;
                }
            }
        }

        public virtual void Display()
        {
            Console.WriteLine("Your Info");
            Console.WriteLine("Your Name is : " + Name + "  Your Address is : " + Address + "   Your ID is : " + ID + "   Your Phone Number is : " + Phone);
        }

        public static void NumCustomer()
        {
            Console.WriteLine("The Number of Customer is : " + CustomerCount);
        }
    }
}
