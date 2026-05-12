using System;
using System.IO;

namespace Project
{
    public abstract class Feedback<T>
    {
        protected Customer customer;
        protected T Comment;
        protected int Rating;

        public Feedback()
        {
            customer = new Customer();
            Comment = default(T);
            Rating = 1;
        }

        public Feedback(Customer cust, T comment, int rating)
        {
            customer = new Customer();
            customer.ID = cust.ID;
            customer.NAME = cust.NAME;
            customer.PHONE = cust.PHONE;
            customer.ADDRESS = cust.ADDRESS;
            COMMENT = comment;
            RATING = rating;
        }

        public int RATING
        {
            set
            {
                if (value >= 1 && value <= 5)
                {
                    Rating = value;
                }
                else
                {
                    Rating = 5;
                    Console.WriteLine("invalid number ");
                }
            }
            get { return Rating; }
        }

        public T COMMENT
        {
            set { Comment = value; }
            get { return Comment; }
        }

        public abstract void Display();
    }

    public class ProductFeedback<T> : Feedback<T>
    {
        private static string FileName = "feedback.txt";

        private Product product;

        public ProductFeedback()
        {
            product = new Product();
            customer = new Customer();
            COMMENT = default(T);
            RATING = 1;
        }

        public ProductFeedback(Customer cust, T comment, int rating, Product pro)
            : base(cust, comment, rating)
        {
            product = new Product();
            product.ID = pro.ID;
            product.NAME = pro.NAME;
            product.PRICE = pro.PRICE;
            product.PRODUCT_DESCRIPTION = pro.PRODUCT_DESCRIPTION;
        }

        public static string BuildFileLine(Customer customer, Product product, T comment, int rating)
        {
            return customer.ID + "," + customer.NAME + "," + customer.ADDRESS + "," + customer.PHONE + "," + product.ID + "," + product.NAME + "," + product.PRICE + "," + product.PRODUCT_DESCRIPTION + "," + comment + "," + rating;
        }

        public static void AddProductFeedback(Customer currentCustomer, Warehouse[] warehouseItems, int warehouseCount, Feedback<string>[] feedbacks, string[] feedbackData, ref int feedbackCount)
        {
            if (!Customer.HasCustomer(currentCustomer))
                return;

            Console.Write("Enter Product ID: ");
            Warehouse warehouseItem = Warehouse.FindWarehouseItemByProductID(warehouseItems, warehouseCount, ConsoleInput.ReadInt());

            if (warehouseItem == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            if (feedbackCount >= feedbacks.Length)
            {
                Console.WriteLine("Feedback list is full.");
                return;
            }

            Product product = warehouseItem.ProductItem;
            Console.Write("Enter your comment: ");
            string comment = ConsoleInput.ReadText();
            Console.Write("Enter rating from 1 to 5: ");
            int rating = ConsoleInput.ReadInt();

            feedbacks[feedbackCount] = new ProductFeedback<string>(currentCustomer, comment, rating, product);
            feedbackData[feedbackCount] = ProductFeedback<string>.BuildFileLine(currentCustomer, product, comment, rating);
            feedbackCount++;

            Console.WriteLine("Feedback added successfully.");
        }

        public static void DisplayMyFeedback(Customer currentCustomer, Feedback<string>[] feedbacks, string[] feedbackData, int feedbackCount)
        {
            if (!Customer.HasCustomer(currentCustomer))
                return;

            Console.WriteLine("\n--- My Feedback ---");
            bool found = false;

            for (int i = 0; i < feedbackCount; i++)
            {
                string[] data = feedbackData[i].Split(',');

                if (data.Length >= 10 && data[0] == currentCustomer.ID.ToString())
                {
                    feedbacks[i].Display();
                    found = true;
                }
            }

            if (!found)
                Console.WriteLine("You have no feedback.");
        }

        public static ProductFeedback<string> ParseFileLine(string line)
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

            return new ProductFeedback<string>(customer, data[8], int.Parse(data[9]), product);
        }

        public static void SaveAllToFile(string[] feedbackData, int feedbackCount, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    for (int i = 0; i < feedbackCount; i++)
                    {
                        if (feedbackData[i] != null && feedbackData[i] != "")
                            writer.WriteLine(feedbackData[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving feedback data: " + ex.Message);
            }
        }

        public static void SaveAllToFile(string[] feedbackData, int feedbackCount)
        {
            SaveAllToFile(feedbackData, feedbackCount, FileName);
        }

        public static int LoadAllFromFile(Feedback<string>[] feedbacks, string[] feedbackData, string filePath)
        {
            int feedbackCount = 0;

            try
            {
                if (!File.Exists(filePath))
                    return feedbackCount;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null && feedbackCount < feedbacks.Length)
                    {
                        try
                        {
                            ProductFeedback<string> feedback = ParseFileLine(line);

                            if (feedback != null)
                            {
                                feedbacks[feedbackCount] = feedback;
                                feedbackData[feedbackCount] = line;
                                feedbackCount++;
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
                Console.WriteLine("Error loading feedback data: " + ex.Message);
            }

            return feedbackCount;
        }

        public static int LoadAllFromFile(Feedback<string>[] feedbacks, string[] feedbackData)
        {
            return LoadAllFromFile(feedbacks, feedbackData, FileName);
        }

        public override void Display()
        {
            Console.WriteLine("Your Name : " + customer.NAME + "\nThe Product : " + product.NAME + "\nYour Rating : " + RATING + "\nYour comment : " + COMMENT);
        }
    }
}
