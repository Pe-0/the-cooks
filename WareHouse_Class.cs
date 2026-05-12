using System;
using System.IO;

namespace Project
{
    public class Warehouse
    {
        private static readonly string FolderPath = @"C:\Users\peter\source\repos\ConsoleApp5\Data";
        private static readonly string FileName = Path.Combine(FolderPath, "warehouse.txt");

        private Product product;
        private int quantity;

        public Warehouse()
        {
            product = new Product();
            quantity = 0;
        }

        public Warehouse(Product product, int quantity)
        {
            this.product = new Product();
            ProductItem = product;
            QUANTITY = quantity;
        }

        public Product ProductItem
        {
            set
            {
                product = value ?? new Product();
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
            return product.ID + "," + product.NAME + "," + product.PRICE + "," + product.PRODUCT_DESCRIPTION + "," + quantity;
        }

        public static void SaveAllToFile(Warehouse[] warehouseItems, int warehouseCount, string filePath)
        {
            Directory.CreateDirectory(FolderPath);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < warehouseCount; i++)
                    writer.WriteLine(warehouseItems[i].ToFileLine());
            }
        }

        public static void SaveAllToFile(Warehouse[] warehouseItems, int warehouseCount)
        {
            SaveAllToFile(warehouseItems, warehouseCount, FileName);
        }

        public static int LoadAllFromFile(Warehouse[] warehouseItems, string filePath)
        {
            int warehouseCount = 0;

            if (!File.Exists(filePath))
                return warehouseCount;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null && warehouseCount < warehouseItems.Length)
                {
                    if (TryParse(line, out Warehouse warehouseItem))
                        warehouseItems[warehouseCount++] = warehouseItem;
                }
            }

            return warehouseCount;
        }

        public static int LoadAllFromFile(Warehouse[] warehouseItems)
        {
            return LoadAllFromFile(warehouseItems, FileName);
        }

        public static bool TryParse(string line, out Warehouse warehouseItem)
        {
            warehouseItem = new Warehouse();

            if (string.IsNullOrWhiteSpace(line))
                return false;

            string[] data = line.Split(',');

            if (data.Length < 4)
                return false;

            if (!int.TryParse(data[0], out int id))
                return false;

            if (!double.TryParse(data[2], out double price))
                return false;

            int stockQuantity = 50;
            if (data.Length >= 5 && !int.TryParse(data[4], out stockQuantity))
                return false;

            Product parsedProduct = new Product();
            parsedProduct.ID = id;
            parsedProduct.NAME = data[1];
            parsedProduct.PRICE = price;
            parsedProduct.PRODUCT_DESCRIPTION = data[3];

            warehouseItem = new Warehouse(parsedProduct, stockQuantity);
            return true;
        }

        public void DisplayInfo()
        {
            product.DisplayInfo();
            Console.WriteLine("Available Quantity: " + quantity);
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
                    if (TryParse(line, out Warehouse warehouseItem))
                        warehouseItem.DisplayInfo();
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
                if (TryParse(line, out Warehouse warehouseItem))
                {
                    ProductItem = warehouseItem.ProductItem;
                    QUANTITY = warehouseItem.QUANTITY;
                }
            }
        }
    }
}
