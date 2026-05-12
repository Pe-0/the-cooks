using System;
using System.IO;

namespace Project
{
    public class Order
    {
        private static readonly string FolderPath = @"C:\Users\peter\source\repos\ConsoleApp5\Data";
        private static readonly string FileName = Path.Combine(FolderPath, "order.txt");

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
            customer = new Customer();
            product = new Product();
            product.ID = pro.ID;
            product.NAME = pro.NAME;
            product.PRICE = pro.PRICE;
            product.PRODUCT_DESCRIPTION = pro.PRODUCT_DESCRIPTION;
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

        public string ToFileLine()
        {
            return BuildFileLine(customer, product, ShippingCost, Discount);
        }

        public static string BuildFileLine(Customer customer, Product product, double shippingCost, double discount)
        {
            return customer.ID + "," + customer.NAME + "," + customer.ADDRESS + "," + customer.PHONE + "," + product.ID + "," + product.NAME + "," + product.PRICE + "," + product.PRODUCT_DESCRIPTION + "," + shippingCost + "," + discount;
        }

        public static bool TryParse(string line, out Order order)
        {
            order = new Order();

            if (string.IsNullOrWhiteSpace(line))
                return false;

            string[] data = line.Split(',');

            if (data.Length < 10)
                return false;

            if (!int.TryParse(data[0], out int customerId))
                return false;

            if (!int.TryParse(data[4], out int productId))
                return false;

            if (!double.TryParse(data[6], out double price))
                return false;

            if (!double.TryParse(data[8], out double shippingCost))
                return false;

            if (!double.TryParse(data[9], out double discount))
                return false;

            Customer customer = new Customer();
            customer.ID = customerId;
            customer.NAME = data[1];
            customer.ADDRESS = data[2];
            customer.PHONE = data[3];

            Product product = new Product();
            product.ID = productId;
            product.NAME = data[5];
            product.PRICE = price;
            product.PRODUCT_DESCRIPTION = data[7];

            order = new Order(customer, product, shippingCost, discount);
            return true;
        }

        public static void SaveAllToFile(string[] orderData, int orderCount, string filePath)
        {
            Directory.CreateDirectory(FolderPath);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < orderCount; i++)
                {
                    if (!string.IsNullOrWhiteSpace(orderData[i]))
                        writer.WriteLine(orderData[i]);
                }
            }
        }

        public static void SaveAllToFile(string[] orderData, int orderCount)
        {
            SaveAllToFile(orderData, orderCount, FileName);
        }

        public static int LoadAllFromFile(Order[] orders, string[] orderData, string filePath)
        {
            int orderCount = 0;

            if (!File.Exists(filePath))
                return orderCount;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null && orderCount < orders.Length)
                {
                    if (TryParse(line, out Order order))
                    {
                        orders[orderCount] = order;
                        orderData[orderCount] = line;
                        orderCount++;
                    }
                }
            }

            return orderCount;
        }

        public static int LoadAllFromFile(Order[] orders, string[] orderData)
        {
            return LoadAllFromFile(orders, orderData, FileName);
        }

        public void DisplayOrderInfo()
        {
            customer.Display();
            Console.WriteLine("Shipping Cost: " + ShippingCost + "\nDiscount: " + Discount + "\nFinal price: " + CalculateTotal(ShippingCost, Discount));
        }
    }
}
