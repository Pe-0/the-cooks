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
            Comment = default;
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
        private static readonly string FolderPath = @"C:\Users\peter\source\repos\ConsoleApp5\Data";
        private static readonly string FileName = Path.Combine(FolderPath, "feedback.txt");

        private Product product;

        public ProductFeedback()
        {
            product = new Product();
            customer = new Customer();
            COMMENT = default;
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

        public static bool TryParseFileLine(string line, out ProductFeedback<string> feedback)
        {
            feedback = new ProductFeedback<string>();

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

            if (!int.TryParse(data[data.Length - 1], out int rating))
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

            string comment = string.Join(",", data, 8, data.Length - 9);
            feedback = new ProductFeedback<string>(customer, comment, rating, product);
            return true;
        }

        public static void SaveAllToFile(string[] feedbackData, int feedbackCount, string filePath)
        {
            Directory.CreateDirectory(FolderPath);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < feedbackCount; i++)
                {
                    if (!string.IsNullOrWhiteSpace(feedbackData[i]))
                        writer.WriteLine(feedbackData[i]);
                }
            }
        }

        public static void SaveAllToFile(string[] feedbackData, int feedbackCount)
        {
            SaveAllToFile(feedbackData, feedbackCount, FileName);
        }

        public static int LoadAllFromFile(Feedback<string>[] feedbacks, string[] feedbackData, string filePath)
        {
            int feedbackCount = 0;

            if (!File.Exists(filePath))
                return feedbackCount;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null && feedbackCount < feedbacks.Length)
                {
                    if (TryParseFileLine(line, out ProductFeedback<string> feedback))
                    {
                        feedbacks[feedbackCount] = feedback;
                        feedbackData[feedbackCount] = line;
                        feedbackCount++;
                    }
                }
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
