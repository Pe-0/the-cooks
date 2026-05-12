using System;
using System.IO;

namespace Project
{
    public class Order
    {
        private static string FileName = "order.txt";

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

        public static void MakeOrder(Customer customer, Warehouse[] warehouseItems, int warehouseCount, Order[] orders, string[] orderData, ref int orderCount)
        {
            if (!Customer.HasCustomer(customer))
                return;

            if (warehouseCount == 0)
            {
                Console.WriteLine("No products available now.");
                return;
            }

            if (orderCount >= orders.Length)
            {
                Console.WriteLine("Order list is full.");
                return;
            }

            Warehouse.DisplayAllProducts(warehouseItems, warehouseCount);
            Console.Write("Enter Product ID to order: ");
            Warehouse warehouseItem = Warehouse.FindWarehouseItemByProductID(warehouseItems, warehouseCount, ConsoleInput.ReadInt());

            if (warehouseItem == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            if (!warehouseItem.HasStock())
            {
                Console.WriteLine("This product is out of stock.");
                return;
            }

            Product product = warehouseItem.ProductItem;
            double shippingCost = warehouseItem.SHIPPING_COST;
            double discount = 0;

            orders[orderCount] = new Order(customer, product, shippingCost, discount);
            orderData[orderCount] = BuildFileLine(customer, product, shippingCost, discount);
            orderCount++;
            warehouseItem.DecreaseStock();

            Console.WriteLine("Order created successfully.");
            Console.WriteLine("Shipping cost: " + shippingCost);
            Console.WriteLine("Remaining quantity: " + warehouseItem.QUANTITY);
        }

        public static void DisplayMyOrders(Customer customer, string[] orderData, int orderCount)
        {
            if (!Customer.HasCustomer(customer))
                return;

            Console.WriteLine("\n--- My Orders ---");
            bool found = false;

            for (int i = 0; i < orderCount; i++)
            {
                string[] data = orderData[i].Split(',');

                if (data.Length >= 10 && data[0] == customer.ID.ToString())
                {
                    Console.WriteLine("Order Number: " + (i + 1));
                    Console.WriteLine("Product: " + data[5]);
                    Console.WriteLine("Price: " + data[6]);
                    Console.WriteLine("Description: " + data[7]);
                    Console.WriteLine("Shipping Cost: " + data[8]);
                    Console.WriteLine("Discount: " + data[9]);
                    Console.WriteLine();
                    found = true;
                }
            }

            if (!found)
                Console.WriteLine("You have no orders.");
        }

        public static Order ParseFileLine(string line)
        {
            if (line == null || line == "")
                return null;

            string[] data = line.Split(',');

            if (data.Length < 10)
                return null;

            Customer customer = new Customer();
            customer.ID = int.Parse(data[0]);
            customer.NAME = data[1];
            customer.ADDRESS = data[2];
            customer.PHONE = data[3];

            Product product = new Product();
            product.ID = int.Parse(data[4]);
            product.NAME = data[5];
            product.PRICE = double.Parse(data[6]);
            product.PRODUCT_DESCRIPTION = data[7];

            return new Order(customer, product, double.Parse(data[8]), double.Parse(data[9]));
        }

        public static void SaveAllToFile(string[] orderData, int orderCount, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    for (int i = 0; i < orderCount; i++)
                    {
                        if (orderData[i] != null && orderData[i] != "")
                            writer.WriteLine(orderData[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving order data: " + ex.Message);
            }
        }

        public static void SaveAllToFile(string[] orderData, int orderCount)
        {
            SaveAllToFile(orderData, orderCount, FileName);
        }

        public static int LoadAllFromFile(Order[] orders, string[] orderData, string filePath)
        {
            int orderCount = 0;

            try
            {
                if (!File.Exists(filePath))
                    return orderCount;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null && orderCount < orders.Length)
                    {
                        try
                        {
                            Order order = ParseFileLine(line);

                            if (order != null)
                            {
                                orders[orderCount] = order;
                                orderData[orderCount] = line;
                                orderCount++;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading order data: " + ex.Message);
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
