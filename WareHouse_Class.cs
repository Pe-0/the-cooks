using System;
using System.IO;

namespace Project
{
    public class Warehouse
    {
        private static string FileName = "warehouse.txt";

        private Product product;
        private int quantity;
        private double shippingCost;

        public Warehouse()
        {
            product = new Product();
            quantity = 0;
            shippingCost = 0;
        }

        public Warehouse(Product product, int quantity)
        {
            this.product = new Product();
            ProductItem = product;
            QUANTITY = quantity;
            SHIPPING_COST = 0;
        }

        public Warehouse(Product product, int quantity, double shippingCost)
        {
            this.product = new Product();
            ProductItem = product;
            QUANTITY = quantity;
            SHIPPING_COST = shippingCost;
        }

        public Product ProductItem
        {
            set
            {
                if (value == null)
                    product = new Product();
                else
                    product = value;
            }
            get { return product; }
        }

        public int QUANTITY
        {
            set
            {
                if (value >= 0)
                    quantity = value;
                else
                {
                    quantity = 0;
                    Console.WriteLine("Error: Warehouse quantity must be non-negative.");
                }
            }
            get { return quantity; }
        }

        public double SHIPPING_COST
        {
            set
            {
                if (value >= 0)
                    shippingCost = value;
                else
                    shippingCost = 0;
            }
            get { return shippingCost; }
        }

        public bool HasStock()
        {
            return quantity > 0;
        }

        public bool DecreaseStock()
        {
            if (!HasStock())
            {
                Console.WriteLine("Product is out of stock.");
                return false;
            }

            quantity--;
            return true;
        }

        public void IncreaseStock(int amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Error: Added stock must be greater than zero.");
                return;
            }

            quantity += amount;
        }

        public string ToFileLine()
        {
            return product.ID + "," + product.NAME + "," + product.PRICE + "," + product.PRODUCT_DESCRIPTION + "," + quantity + "," + shippingCost;
        }

        public static Warehouse FindWarehouseItemByProductID(Warehouse[] warehouseItems, int warehouseCount, int id)
        {
            for (int i = 0; i < warehouseCount; i++)
            {
                if (warehouseItems[i].ProductItem.ID == id)
                    return warehouseItems[i];
            }

            return null;
        }

        public static void DisplayAllProducts(Warehouse[] warehouseItems, int warehouseCount)
        {
            Console.WriteLine("\n--- Warehouse Products ---");

            if (warehouseCount == 0)
            {
                Console.WriteLine("No products available now.");
                return;
            }

            for (int i = 0; i < warehouseCount; i++)
                warehouseItems[i].DisplayInfo();
        }

        public static void SearchProductByID(Warehouse[] warehouseItems, int warehouseCount)
        {
            Console.Write("Enter Product ID: ");
            Warehouse warehouseItem = FindWarehouseItemByProductID(warehouseItems, warehouseCount, ConsoleInput.ReadInt());

            if (warehouseItem == null)
                Console.WriteLine("Product not found.");
            else
                warehouseItem.DisplayInfo();
        }

        public static int AddDefaultProducts(Warehouse[] warehouseItems, int warehouseCount)
        {
            warehouseCount = AddProductToWarehouse(warehouseItems, warehouseCount, 1, "Mobile", 15000, "Smart phone", 10, 80);
            warehouseCount = AddProductToWarehouse(warehouseItems, warehouseCount, 2, "Screen", 7000, "Computer screen", 7, 120);
            warehouseCount = AddProductToWarehouse(warehouseItems, warehouseCount, 3, "RAM", 1200, "8 GB RAM", 20, 35);
            warehouseCount = AddProductToWarehouse(warehouseItems, warehouseCount, 4, "Hard Disk", 2000, "1 TB hard disk", 15, 50);
            warehouseCount = AddProductToWarehouse(warehouseItems, warehouseCount, 5, "Keyboard", 600, "USB keyboard", 25, 30);
            return warehouseCount;
        }

        public static int AddProductToWarehouse(Warehouse[] warehouseItems, int warehouseCount, int id, string name, double price, string description, int quantity, double shippingCost)
        {
            if (warehouseCount >= warehouseItems.Length)
                return warehouseCount;

            Product product = new Product(id, name, price, description);
            warehouseItems[warehouseCount] = new Warehouse(product, quantity, shippingCost);
            warehouseCount++;
            return warehouseCount;
        }

        public static void SaveAllToFile(Warehouse[] warehouseItems, int warehouseCount, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    for (int i = 0; i < warehouseCount; i++)
                        writer.WriteLine(warehouseItems[i].ToFileLine());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving warehouse data: " + ex.Message);
            }
        }

        public static void SaveAllToFile(Warehouse[] warehouseItems, int warehouseCount)
        {
            SaveAllToFile(warehouseItems, warehouseCount, FileName);
        }

        public static int LoadAllFromFile(Warehouse[] warehouseItems, string filePath)
        {
            int warehouseCount = 0;

            try
            {
                if (!File.Exists(filePath))
                    return warehouseCount;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null && warehouseCount < warehouseItems.Length)
                    {
                        try
                        {
                            Warehouse warehouseItem = ParseFileLine(line);

                            if (warehouseItem != null)
                                warehouseItems[warehouseCount++] = warehouseItem;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading warehouse data: " + ex.Message);
            }

            return warehouseCount;
        }

        public static int LoadAllFromFile(Warehouse[] warehouseItems)
        {
            return LoadAllFromFile(warehouseItems, FileName);
        }

        public static Warehouse ParseFileLine(string line)
        {
            if (line == null || line == "")
                return null;

            string[] data = line.Split(',');

            if (data.Length < 4)
                return null;

            int stockQuantity = 50;
            double parsedShippingCost = 0;

            if (data.Length >= 5)
                stockQuantity = int.Parse(data[4]);

            if (data.Length >= 6)
                parsedShippingCost = double.Parse(data[5]);

            Product parsedProduct = new Product();
            parsedProduct.ID = int.Parse(data[0]);
            parsedProduct.NAME = data[1];
            parsedProduct.PRICE = double.Parse(data[2]);
            parsedProduct.PRODUCT_DESCRIPTION = data[3];

            return new Warehouse(parsedProduct, stockQuantity, parsedShippingCost);
        }

        public void DisplayInfo()
        {
            product.DisplayInfo();
            Console.WriteLine("Quantity    : " + quantity);
            Console.WriteLine("Shipping    : " + shippingCost);
            Console.WriteLine("------------------------------");
        }

        public void DisplayAllFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No warehouse data found.");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        Warehouse warehouseItem = ParseFileLine(line);

                        if (warehouseItem != null)
                            warehouseItem.DisplayInfo();
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
            {
                Console.WriteLine("No warehouse data found.");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line = reader.ReadLine();
                Warehouse warehouseItem = ParseFileLine(line);

                if (warehouseItem != null)
                {
                    ProductItem = warehouseItem.ProductItem;
                    QUANTITY = warehouseItem.QUANTITY;
                }
            }
        }
    }
}
