using System;
using System.IO;

namespace Project
{
    public class Customer : ICustomer
    {
        private static string FileName = "customer.txt";

        protected string Name = "not null";
        protected string Address = "Luxor";
        protected int Id;
        private static int CustomerCount = 0;
        protected string Phone = "01000000000";

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
                    Console.WriteLine("Error ID input.");
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
                    if (ConsoleInput.IsValidPhone(value))
                        Phone = value;
                    else
                        throw new Exception("Invalid phone number format");
                }
                catch
                {
                    Console.WriteLine("Wrong phone number.");
                    Phone = "01000000000";
                }
            }
            get { return Phone; }
        }

        public string ToFileLine()
        {
            return ID + "," + NAME + "," + ADDRESS + "," + PHONE;
        }

        public static int FindCustomerIndexByID(Customer[] customers, int customerCount, int id)
        {
            for (int i = 0; i < customerCount; i++)
            {
                if (customers[i].ID == id)
                    return i;
            }

            return -1;
        }

        public static Customer RegisterOrLogin(Customer[] customers, ref int customerCount)
        {
            Console.Write("Enter your ID: ");
            int id = ConsoleInput.ReadInt();

            int index = FindCustomerIndexByID(customers, customerCount, id);
            if (index != -1)
            {
                Console.WriteLine("Login successful.");
                customers[index].Display();
                return customers[index];
            }

            if (customerCount >= customers.Length)
            {
                Console.WriteLine("Customer list is full.");
                return null;
            }

            Customer customer = new Customer();
            customer.ID = id;
            Console.Write("Enter your name: ");
            customer.NAME = ConsoleInput.ReadText();
            Console.Write("Enter your address: ");
            customer.ADDRESS = ConsoleInput.ReadText();
            Console.Write("Enter your phone number: ");
            customer.PHONE = ConsoleInput.ReadPhone();

            customers[customerCount] = customer;
            customerCount++;
            Console.WriteLine("Registration successful.");
            return customer;
        }

        public static bool HasCustomer(Customer customer)
        {
            if (customer == null)
            {
                Console.WriteLine("Please register or login first.");
                return false;
            }

            return true;
        }

        public static void DisplayCurrentCustomer(Customer customer)
        {
            if (!HasCustomer(customer))
                return;

            customer.Display();
        }

        public static Customer ParseFileLine(string line)
        {
            if (line == null || line == "")
                return null;

            string[] data = line.Split(',');

            if (data.Length < 4)
                return null;

            Customer customer = new Customer();

            customer.ID = int.Parse(data[0]);
            customer.NAME = data[1];
            customer.ADDRESS = data[2];
            customer.PHONE = data[3];
            return customer;
        }

        public static void SaveAllToFile(Customer[] customers, int customerCount, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    for (int i = 0; i < customerCount; i++)
                        writer.WriteLine(customers[i].ToFileLine());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving customer data: " + ex.Message);
            }
        }

        public static void SaveAllToFile(Customer[] customers, int customerCount)
        {
            SaveAllToFile(customers, customerCount, FileName);
        }

        public static int LoadAllFromFile(Customer[] customers, string filePath)
        {
            int customerCount = 0;

            try
            {
                if (!File.Exists(filePath))
                    return customerCount;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null && customerCount < customers.Length)
                    {
                        try
                        {
                            Customer customer = ParseFileLine(line);

                            if (customer != null)
                                customers[customerCount++] = customer;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading customer data: " + ex.Message);
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
                Console.WriteLine("No customer data found.");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        Customer customer = ParseFileLine(line);

                        if (customer != null)
                            customer.Display();
                    }
                    catch
                    {
                    }
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
                Customer customer = ParseFileLine(line);

                if (customer != null)
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
            Console.WriteLine();
            Console.WriteLine("----- Customer Info -----");
            Console.WriteLine("Name    : " + Name);
            Console.WriteLine("Address : " + Address);
            Console.WriteLine("ID      : " + ID);
            Console.WriteLine("Phone   : " + Phone);
            Console.WriteLine("-------------------------");
        }

        public static void NumCustomer()
        {
            Console.WriteLine("The Number of Customer is: " + CustomerCount);
        }
    }
}
