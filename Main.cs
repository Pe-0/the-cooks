using System;
using System.IO;

namespace Project
{
    public class program
    {
        static Customer[] customers = new Customer[100];
        static Product[] products = new Product[100];
        static Order[] orders = new Order[100];
        static Payment[] payments = new Payment[100];
        static Feedback<string>[] feedbacks = new Feedback<string>[100];

        static string[] orderData = new string[100];
        static string[] feedbackData = new string[100];

        static Customer currentCustomer = null;

        static int customerCount = 0;
        static int productCount = 0;
        static int orderCount = 0;
        static int paymentCount = 0;
        static int feedbackCount = 0;

        static string folderPath = @"C:\Users\peter\source\repos\ConsoleApp5\Data";
        static string customerFileName = Path.Combine(folderPath, "customer.txt");
        static string productFileName = Path.Combine(folderPath, "product.txt");
        static string orderFileName = Path.Combine(folderPath, "order.txt");
        static string feedbackFileName = Path.Combine(folderPath, "feedback.txt");

        static void Main(string[] args)
        {
            Directory.CreateDirectory(folderPath);
            LoadDataFromFile();

            int choice;
            do
            {
                try
                {
                    Console.WriteLine("\n==== Online Shipping Menu ====");
                    Console.WriteLine("1. Register / Login");
                    Console.WriteLine("2. Display My Info");
                    Console.WriteLine("3. Display Products");
                    Console.WriteLine("4. Search Product by ID");
                    Console.WriteLine("5. Make Order");
                    Console.WriteLine("6. Display My Orders");
                    Console.WriteLine("7. Pay For Order");
                    Console.WriteLine("8. Add Product Feedback");
                    Console.WriteLine("9. Display My Feedback");
                    Console.WriteLine("10. Exit and Save");
                    Console.Write("Enter your choice: ");
                    choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            RegisterOrLogin();
                            break;
                        case 2:
                            DisplayMyInfo();
                            break;
                        case 3:
                            DisplayAllProducts();
                            break;
                        case 4:
                            SearchProductByID();
                            break;
                        case 5:
                            MakeOrder();
                            break;
                        case 6:
                            DisplayMyOrders();
                            break;
                        case 7:
                            PayForOrder();
                            break;
                        case 8:
                            AddProductFeedback();
                            break;
                        case 9:
                            DisplayMyFeedback();
                            break;
                        case 10:
                            SaveDataToFile();
                            Console.WriteLine("Data saved. Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input! Please enter a valid number.");
                    choice = 0;
                }
            } while (choice != 10);
        }

        static string ReadText()
        {
            return Console.ReadLine() ;
        }

        static int ReadInt()
        {
            return int.Parse(Console.ReadLine());
        }

        static double ReadDouble()
        {
            return double.Parse(Console.ReadLine());
        }

        static bool HasCustomer()
        {
            if (currentCustomer == null)
            {
                Console.WriteLine("Please register or login first.");
                return false;
            }

            return true;
        }

        static int FindCustomerIndexByID(int id)
        {
            for (int i = 0; i < customerCount; i++)
            {
                if (customers[i].ID == id)
                    return i;
            }

            return -1;
        }

        static Product FindProductByID(int id)
        {
            for (int i = 0; i < productCount; i++)
            {
                if (products[i].ID == id)
                    return products[i];
            }

            return null;
        }

        static void RegisterOrLogin()
        {
            Console.Write("Enter your ID: ");
            int id = ReadInt();

            int index = FindCustomerIndexByID(id);
            if (index != -1)
            {
                currentCustomer = customers[index];
                Console.WriteLine("Login successful.");
                currentCustomer.Display();
                return;
            }

            Customer customer = new Customer();
            customer.ID = id;
            Console.Write("Enter your name: ");
            customer.NAME = ReadText();
            Console.Write("Enter your address: ");
            customer.ADDRESS = ReadText();
            Console.Write("Enter your phone number: ");
            customer.PHONE = ReadText();

            customers[customerCount++] = customer;
            currentCustomer = customer;
            Console.WriteLine("Registration successful.");
        }

        static void DisplayMyInfo()
        {
            if (!HasCustomer())
                return;

            currentCustomer.Display();
        }

        static void DisplayAllProducts()
        {
            Console.WriteLine("\n--- Available Products ---");

            if (productCount == 0)
            {
                Console.WriteLine("No products available now.");
                return;
            }

            for (int i = 0; i < productCount; i++)
                products[i].DisplayInfo();
        }

        static void SearchProductByID()
        {
            Console.Write("Enter Product ID: ");
            Product product = FindProductByID(ReadInt());

            if (product == null)
                Console.WriteLine("Product not found.");
            else
                product.DisplayInfo();
        }

        static void MakeOrder()
        {
            if (!HasCustomer())
                return;

            if (productCount == 0)
            {
                Console.WriteLine("No products available now.");
                return;
            }

            DisplayAllProducts();
            Console.Write("Enter Product ID to order: ");
            Product product = FindProductByID(ReadInt());

            if (product == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            double shippingCost = 50;
            double discount = 0;

            orders[orderCount] = new Order(currentCustomer, product, shippingCost, discount);
            orderData[orderCount] = currentCustomer.ID + "," + currentCustomer.NAME + "," + currentCustomer.ADDRESS + "," + currentCustomer.PHONE + "," + product.ID + "," + product.NAME + "," + product.PRICE + "," + product.PRODUCT_DESCRIPTION + "," + shippingCost + "," + discount;
            orderCount++;

            Console.WriteLine("Order created successfully.");
            Console.WriteLine("Shipping cost: " + shippingCost);
        }

        static void DisplayMyOrders()
        {
            if (!HasCustomer())
                return;

            Console.WriteLine("\n--- My Orders ---");
            bool found = false;

            for (int i = 0; i < orderCount; i++)
            {
                string[] data = orderData[i].Split(',');

                if (data.Length >= 10 && data[0] == currentCustomer.ID.ToString())
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

        static void PayForOrder()
        {
            if (!HasCustomer())
                return;

            DisplayMyOrders();
            Console.Write("Enter order number to pay: ");
            int orderNumber = ReadInt();

            if (orderNumber < 1 || orderNumber > orderCount)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            string[] data = orderData[orderNumber - 1].Split(',');
            if (data.Length < 10 || data[0] != currentCustomer.ID.ToString())
            {
                Console.WriteLine("Order not found.");
                return;
            }

            Product product = new Product();
            product.ID = int.Parse(data[4]);
            product.NAME = data[5];
            product.PRICE = double.Parse(data[6]);
            product.PRODUCT_DESCRIPTION = data[7];

            double shippingCost = double.Parse(data[8]);
            double discount = double.Parse(data[9]);

            Console.WriteLine("1. Credit Card");
            Console.WriteLine("2. Cash On Delivery");
            Console.Write("Choose payment method: ");
            int paymentType = ReadInt();

            if (paymentType == 1)
            {
                Console.Write("Enter 16 digit card number: ");
                string cardNumber = ReadText();
                payments[paymentCount++] = new CreditCardPayment(currentCustomer, product, shippingCost, discount, cardNumber);
            }
            else if (paymentType == 2)
            {
                payments[paymentCount++] = new CashOnDelivery(currentCustomer, product, shippingCost, discount);
            }
            else
            {
                Console.WriteLine("Invalid payment method.");
            }
        }

        static void AddProductFeedback()
        {
            if (!HasCustomer())
                return;

            Console.Write("Enter Product ID: ");
            Product product = FindProductByID(ReadInt());

            if (product == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            Console.Write("Enter your comment: ");
            string comment = ReadText();
            Console.Write("Enter rating from 1 to 5: ");
            int rating = ReadInt();

            feedbacks[feedbackCount] = new ProductFeedback<string>(currentCustomer, comment, rating, product);
            feedbackData[feedbackCount] = currentCustomer.ID + "," + currentCustomer.NAME + "," + currentCustomer.ADDRESS + "," + currentCustomer.PHONE + "," + product.ID + "," + product.NAME + "," + product.PRICE + "," + product.PRODUCT_DESCRIPTION + "," + comment + "," + rating;
            feedbackCount++;

            Console.WriteLine("Feedback added successfully.");
        }

        static void DisplayMyFeedback()
        {
            if (!HasCustomer())
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

        static void SaveDataToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(customerFileName))
                {
                    for (int i = 0; i < customerCount; i++)
                        writer.WriteLine(customers[i].ID + "," + customers[i].NAME + "," + customers[i].ADDRESS + "," + customers[i].PHONE);
                }

                using (StreamWriter writer = new StreamWriter(productFileName))
                {
                    for (int i = 0; i < productCount; i++)
                        writer.WriteLine(products[i].ID + "," + products[i].NAME + "," + products[i].PRICE + "," + products[i].PRODUCT_DESCRIPTION);
                }

                using (StreamWriter writer = new StreamWriter(orderFileName))
                {
                    for (int i = 0; i < orderCount; i++)
                        writer.WriteLine(orderData[i]);
                }

                using (StreamWriter writer = new StreamWriter(feedbackFileName))
                {
                    for (int i = 0; i < feedbackCount; i++)
                        writer.WriteLine(feedbackData[i]);
                }

                Console.WriteLine("Data saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving data: " + ex.Message);
            }
        }

        static void LoadDataFromFile()
        {
            try
            {
                LoadCustomers();
                LoadProducts();
                LoadOrders();
                LoadFeedback();
                Console.WriteLine("Data loaded from file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading data: " + ex.Message);
            }
        }

        static void LoadCustomers()
        {
            if (!File.Exists(customerFileName))
                return;

            using (StreamReader reader = new StreamReader(customerFileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null && customerCount < customers.Length)
                {
                    string[] data = line.Split(',');

                    if (data.Length < 4)
                        continue;

                    Customer customer = new Customer();
                    customer.ID = int.Parse(data[0]);
                    customer.NAME = data[1];
                    customer.ADDRESS = data[2];
                    customer.PHONE = data[3];

                    customers[customerCount++] = customer;
                }
            }
        }

        static void LoadProducts()
        {
            if (!File.Exists(productFileName))
                return;

            using (StreamReader reader = new StreamReader(productFileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null && productCount < products.Length)
                {
                    string[] data = line.Split(',');

                    if (data.Length < 4)
                        continue;

                    Product product = new Product();
                    product.ID = int.Parse(data[0]);
                    product.NAME = data[1];
                    product.PRICE = double.Parse(data[2]);
                    product.PRODUCT_DESCRIPTION = data[3];

                    products[productCount++] = product;
                }
            }
        }

        static void LoadOrders()
        {
            if (!File.Exists(orderFileName))
                return;

            using (StreamReader reader = new StreamReader(orderFileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null && orderCount < orders.Length)
                {
                    string[] data = line.Split(',');

                    if (data.Length < 10)
                        continue;

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

                    double shippingCost = double.Parse(data[8]);
                    double discount = double.Parse(data[9]);

                    orders[orderCount] = new Order(customer, product, shippingCost, discount);
                    orderData[orderCount] = line;
                    orderCount++;
                }
            }
        }

        static void LoadFeedback()
        {
            if (!File.Exists(feedbackFileName))
                return;

            using (StreamReader reader = new StreamReader(feedbackFileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null && feedbackCount < feedbacks.Length)
                {
                    string[] data = line.Split(',');

                    if (data.Length < 10)
                        continue;

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

                    string comment = data[8];
                    int rating = int.Parse(data[9]);

                    feedbacks[feedbackCount] = new ProductFeedback<string>(customer, comment, rating, product);
                    feedbackData[feedbackCount] = line;
                    feedbackCount++;
                }
            }
        }
    }
}
