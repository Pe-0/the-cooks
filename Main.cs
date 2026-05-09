using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class program
    {
        static Customer[] customers = new Customer[0];
        static Product[] products = new Product[0];
        static Order[] orders = new Order[0];
        static string[] orderData = new string[0];
        static Feedback<string>[] feedbacks = new Feedback<string>[0];
        static string[] feedbackData = new string[0];
        static Payment[] payments = new Payment[0];
        static Warehouse warehouse = new Warehouse();

        static int maxItems;
        static int customerCount;
        static int productCount;
        static int orderCount;
        static int feedbackCount;
        static int paymentCount;

        static string folderPath = @"C:\Users\peter\source\repos\ConsoleApp5\Data";
        static string customerFileName = Path.Combine(folderPath, "customer.txt");
        static string productFileName = Path.Combine(folderPath, "product.txt");
        static string orderFileName = Path.Combine(folderPath, "order.txt");
        static string feedbackFileName = Path.Combine(folderPath, "feedback.txt");

        static void Main(string[] args)
        {
            Console.Write("Enter the maximum number of items: ");
            maxItems = int.Parse(Console.ReadLine());

            customers = new Customer[maxItems];
            products = new Product[maxItems];
            orders = new Order[maxItems];
            orderData = new string[maxItems];
            feedbacks = new Feedback<string>[maxItems];
            feedbackData = new string[maxItems];
            payments = new Payment[maxItems];

            Directory.CreateDirectory(folderPath);
            LoadDataFromFile();

            int choice;
            do
            {
                try
                {
                    Console.WriteLine("\n==== Store Management Menu ====");
                    Console.WriteLine("1. Add Customer");
                    Console.WriteLine("2. Display All Customers");
                    Console.WriteLine("3. Delete Customer by ID");
                    Console.WriteLine("4. Search Customer by ID");
                    Console.WriteLine("5. Sort Customers by ID");
                    Console.WriteLine("6. Add Product");
                    Console.WriteLine("7. Display All Products");
                    Console.WriteLine("8. Delete Product by ID");
                    Console.WriteLine("9. Search Product by ID");
                    Console.WriteLine("10. Sort Products by ID");
                    Console.WriteLine("11. Add Order");
                    Console.WriteLine("12. Display All Orders");
                    Console.WriteLine("13. Add Product Feedback");
                    Console.WriteLine("14. Display All Feedback");
                    Console.WriteLine("15. Add Payment");
                    Console.WriteLine("16. Display All Payments");
                    Console.WriteLine("17. Warehouse Menu");
                    Console.WriteLine("18. Exit and Save");
                    Console.Write("Enter your choice: ");
                    choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            AddCustomer();
                            break;
                        case 2:
                            DisplayAllCustomers();
                            break;
                        case 3:
                            DeleteCustomerByID();
                            break;
                        case 4:
                            SearchCustomerByID();
                            break;
                        case 5:
                            SortCustomersByID();
                            break;
                        case 6:
                            AddProduct();
                            break;
                        case 7:
                            DisplayAllProducts();
                            break;
                        case 8:
                            DeleteProductByID();
                            break;
                        case 9:
                            SearchProductByID();
                            break;
                        case 10:
                            SortProductsByID();
                            break;
                        case 11:
                            AddOrder();
                            break;
                        case 12:
                            DisplayAllOrders();
                            break;
                        case 13:
                            AddFeedback();
                            break;
                        case 14:
                            DisplayAllFeedback();
                            break;
                        case 15:
                            AddPayment();
                            break;
                        case 16:
                            DisplayAllPayments();
                            break;
                        case 17:
                            WarehouseMenu();
                            break;
                        case 18:
                            SaveDataToFile();
                            Console.WriteLine("Data saved. Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please choose 1-18.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input! Please enter a valid number.");
                    choice = 0;
                }
            } while (choice != 18);
        }

        static void AddCustomer()
        {
            if (customerCount >= maxItems)
            {
                Console.WriteLine("Cannot add more customers. Maximum limit reached.");
                return;
            }

            try
            {
                Customer customer = new Customer();
                Console.Write("Enter Customer ID: ");
                customer.ID = int.Parse(Console.ReadLine());
                Console.Write("Enter Customer Name: ");
                customer.NAME = Console.ReadLine();
                Console.Write("Enter Customer Address: ");
                customer.ADDRESS = Console.ReadLine();
                Console.Write("Enter Customer Phone: ");
                customer.PHONE = Console.ReadLine();

                customers[customerCount++] = customer;
                Console.WriteLine("Customer added successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input! Please enter numeric values for IDs.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        static void DisplayAllCustomers()
        {
            Console.WriteLine("\n--- All Customers ---");
            for (int i = 0; i < customerCount; i++)
                customers[i].Display();
        }

        static void DeleteCustomerByID()
        {
            Console.Write("Enter the Customer ID to delete: ");
            int customerID = int.Parse(Console.ReadLine());
            int indexToDelete = -1;

            for (int i = 0; i < customerCount; i++)
            {
                if (customers[i].ID == customerID)
                {
                    indexToDelete = i;
                    break;
                }
            }

            if (indexToDelete != -1)
            {
                for (int i = indexToDelete; i < customerCount - 1; i++)
                    customers[i] = customers[i + 1];

                customerCount--;
                Console.WriteLine("Customer deleted successfully.");
            }
            else
                Console.WriteLine("Customer not found.");
        }

        static void SearchCustomerByID()
        {
            Console.Write("Enter the Customer ID to search: ");
            int customerID = int.Parse(Console.ReadLine());
            bool found = false;

            for (int i = 0; i < customerCount; i++)
            {
                if (customers[i].ID == customerID)
                {
                    customers[i].Display();
                    found = true;
                    break;
                }
            }

            if (!found)
                Console.WriteLine("Customer not found.");
        }

        static void SortCustomersByID()
        {
            for (int i = 0; i < customerCount - 1; i++)
            {
                for (int j = 0; j < customerCount - i - 1; j++)
                {
                    if (customers[j].ID > customers[j + 1].ID)
                    {
                        Customer temp = customers[j];
                        customers[j] = customers[j + 1];
                        customers[j + 1] = temp;
                    }
                }
            }

            Console.WriteLine("Customers sorted by ID.");
        }

        static void AddProduct()
        {
            if (productCount >= maxItems)
            {
                Console.WriteLine("Cannot add more products. Maximum limit reached.");
                return;
            }

            try
            {
                Product product = new Product();
                Console.Write("Enter Product ID: ");
                product.ID = int.Parse(Console.ReadLine());
                Console.Write("Enter Product Name: ");
                product.NAME = Console.ReadLine();
                Console.Write("Enter Product Price: ");
                product.PRICE = double.Parse(Console.ReadLine());
                Console.Write("Enter Product Description: ");
                product.PRODUCT_DESCRIPTION = Console.ReadLine();

                products[productCount++] = product;
                Console.WriteLine("Product added successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input! Please enter numeric values for ID and price.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        static void DisplayAllProducts()
        {
            Console.WriteLine("\n--- All Products ---");
            for (int i = 0; i < productCount; i++)
                products[i].DisplayInfo();
        }

        static void DeleteProductByID()
        {
            Console.Write("Enter the Product ID to delete: ");
            int productID = int.Parse(Console.ReadLine());
            int indexToDelete = -1;

            for (int i = 0; i < productCount; i++)
            {
                if (products[i].ID == productID)
                {
                    indexToDelete = i;
                    break;
                }
            }

            if (indexToDelete != -1)
            {
                for (int i = indexToDelete; i < productCount - 1; i++)
                    products[i] = products[i + 1];

                productCount--;
                Console.WriteLine("Product deleted successfully.");
            }
            else
                Console.WriteLine("Product not found.");
        }

        static void SearchProductByID()
        {
            Console.Write("Enter the Product ID to search: ");
            int productID = int.Parse(Console.ReadLine());
            bool found = false;

            for (int i = 0; i < productCount; i++)
            {
                if (products[i].ID == productID)
                {
                    products[i].DisplayInfo();
                    found = true;
                    break;
                }
            }

            if (!found)
                Console.WriteLine("Product not found.");
        }

        static void SortProductsByID()
        {
            for (int i = 0; i < productCount - 1; i++)
            {
                for (int j = 0; j < productCount - i - 1; j++)
                {
                    if (products[j].ID > products[j + 1].ID)
                    {
                        Product temp = products[j];
                        products[j] = products[j + 1];
                        products[j + 1] = temp;
                    }
                }
            }

            Console.WriteLine("Products sorted by ID.");
        }

        static void AddOrder()
        {
            if (orderCount >= maxItems)
            {
                Console.WriteLine("Cannot add more orders. Maximum limit reached.");
                return;
            }

            try
            {
                Customer customer = new Customer();
                Console.Write("Enter Customer ID: ");
                customer.ID = int.Parse(Console.ReadLine());
                Console.Write("Enter Customer Name: ");
                customer.NAME = Console.ReadLine();
                Console.Write("Enter Customer Address: ");
                customer.ADDRESS = Console.ReadLine();
                Console.Write("Enter Customer Phone: ");
                customer.PHONE = Console.ReadLine();

                Product product = new Product();
                Console.Write("Enter Product ID: ");
                product.ID = int.Parse(Console.ReadLine());
                Console.Write("Enter Product Name: ");
                product.NAME = Console.ReadLine();
                Console.Write("Enter Product Price: ");
                product.PRICE = double.Parse(Console.ReadLine());
                Console.Write("Enter Shipping Cost: ");
                double shippingCost = double.Parse(Console.ReadLine());
                Console.Write("Enter Discount: ");
                double discount = double.Parse(Console.ReadLine());

                Order order = new Order(customer, product, shippingCost, discount);
                orderData[orderCount] = customer.ID + "," + customer.NAME + "," + customer.ADDRESS + "," + customer.PHONE + "," + product.ID + "," + product.NAME + "," + product.PRICE + "," + shippingCost + "," + discount;
                orders[orderCount++] = order;
                Console.WriteLine("Order added successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input! Please enter numeric values where needed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        static void DisplayAllOrders()
        {
            Console.WriteLine("\n--- All Orders ---");
            for (int i = 0; i < orderCount; i++)
                orders[i].DisplayOrderInfo();
        }

        static void AddFeedback()
        {
            if (feedbackCount >= maxItems)
            {
                Console.WriteLine("Cannot add more feedback. Maximum limit reached.");
                return;
            }

            try
            {
                Console.Write("Enter Customer Name: ");
                string customerName = Console.ReadLine();
                Console.Write("Enter Customer Address: ");
                string customerAddress = Console.ReadLine();
                Console.Write("Enter Customer Phone: ");
                string customerPhone = Console.ReadLine();
                Console.Write("Enter Comment: ");
                string comment = Console.ReadLine();
                Console.Write("Enter Rating from 1 to 5: ");
                int rating = int.Parse(Console.ReadLine());
                Console.Write("Enter Product ID: ");
                int productId = int.Parse(Console.ReadLine());
                Console.Write("Enter Product Name: ");
                string productName = Console.ReadLine();
                Console.Write("Enter Product Price: ");
                double productPrice = double.Parse(Console.ReadLine());

                Customer customer = new Customer();
                customer.NAME = customerName;
                customer.ADDRESS = customerAddress;
                customer.PHONE = customerPhone;

                Product product = new Product();
                product.ID = productId;
                product.NAME = productName;
                product.PRICE = productPrice;

                feedbacks[feedbackCount] = new ProductFeedback<string>(customer, comment, rating, product);
                feedbackData[feedbackCount] = customerName + "," + customerAddress + "," + customerPhone + "," + comment + "," + rating + "," + productId + "," + productName + "," + productPrice;
                feedbackCount++;
                Console.WriteLine("Product feedback added successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input! Please enter numeric values where needed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        static void DisplayAllFeedback()
        {
            Console.WriteLine("\n--- All Feedback ---");
            for (int i = 0; i < feedbackCount; i++)
                feedbacks[i].Display();
        }

        static void AddPayment()
        {
            if (paymentCount >= maxItems)
            {
                Console.WriteLine("Cannot add more payments. Maximum limit reached.");
                return;
            }

            try
            {
                Console.WriteLine("1. Credit Card Payment");
                Console.WriteLine("2. Cash On Delivery");
                Console.Write("Enter your choice: ");
                int type = int.Parse(Console.ReadLine());

                Customer customer = new Customer();
                Console.Write("Enter Customer ID: ");
                customer.ID = int.Parse(Console.ReadLine());
                Console.Write("Enter Customer Name: ");
                customer.NAME = Console.ReadLine();
                Console.Write("Enter Customer Address: ");
                customer.ADDRESS = Console.ReadLine();
                Console.Write("Enter Customer Phone: ");
                customer.PHONE = Console.ReadLine();

                Product product = new Product();
                Console.Write("Enter Product ID: ");
                product.ID = int.Parse(Console.ReadLine());
                Console.Write("Enter Product Name: ");
                product.NAME = Console.ReadLine();
                Console.Write("Enter Product Price: ");
                product.PRICE = double.Parse(Console.ReadLine());
                Console.Write("Enter Shipping Cost: ");
                double shippingCost = double.Parse(Console.ReadLine());
                Console.Write("Enter Discount: ");
                double discount = double.Parse(Console.ReadLine());

                if (type == 1)
                {
                    Console.Write("Enter 16 digit card number: ");
                    string cardNumber = Console.ReadLine();
                    payments[paymentCount] = new CreditCardPayment(customer, product, shippingCost, discount, cardNumber);
                    paymentCount++;
                }
                else if (type == 2)
                {
                    payments[paymentCount] = new CashOnDelivery(customer, product, shippingCost, discount);
                    paymentCount++;
                }
                else
                    Console.WriteLine("Invalid payment type.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input! Please enter numeric values where needed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        static void DisplayAllPayments()
        {
            Console.WriteLine("\n--- All Payments ---");
            for (int i = 0; i < paymentCount; i++)
                payments[i].DisplayOrderInfo();
        }

        static void WarehouseMenu()
        {
            int choice;
            do
            {
                Console.WriteLine("\n==== Warehouse Menu ====");
                Console.WriteLine("1. Add Phone");
                Console.WriteLine("2. Remove Phone");
                Console.WriteLine("3. Add PC");
                Console.WriteLine("4. Remove PC");
                Console.WriteLine("5. Add Laptop");
                Console.WriteLine("6. Remove Laptop");
                Console.WriteLine("7. Back");
                Console.Write("Enter your choice: ");
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        warehouse.IncrementPhone();
                        break;
                    case 2:
                        warehouse.DecrementPhone();
                        break;
                    case 3:
                        warehouse.IncrementPc();
                        break;
                    case 4:
                        warehouse.DecrementPc();
                        break;
                    case 5:
                        warehouse.IncrementLaptop();
                        break;
                    case 6:
                        warehouse.DecrementLaptop();
                        break;
                    case 7:
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please choose 1-7.");
                        break;
                }
            } while (choice != 7);
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
                if (File.Exists(customerFileName))
                {
                    using (StreamReader reader = new StreamReader(customerFileName))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null && customerCount < maxItems)
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

                if (File.Exists(productFileName))
                {
                    using (StreamReader reader = new StreamReader(productFileName))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null && productCount < maxItems)
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

                if (File.Exists(orderFileName))
                {
                    using (StreamReader reader = new StreamReader(orderFileName))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null && orderCount < maxItems)
                        {
                            string[] data = line.Split(',');
                            if (data.Length < 9)
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

                            double shippingCost = double.Parse(data[7]);
                            double discount = double.Parse(data[8]);
                            Order order = new Order(customer, product, shippingCost, discount);
                            orderData[orderCount] = line;
                            orders[orderCount++] = order;
                        }
                    }
                }

                if (File.Exists(feedbackFileName))
                {
                    using (StreamReader reader = new StreamReader(feedbackFileName))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null && feedbackCount < maxItems)
                        {
                            string[] data = line.Split(',');
                            if (data.Length < 8)
                                continue;

                            Customer customer = new Customer();
                            customer.NAME = data[0];
                            customer.ADDRESS = data[1];
                            customer.PHONE = data[2];

                            Product product = new Product();
                            product.ID = int.Parse(data[5]);
                            product.NAME = data[6];
                            product.PRICE = double.Parse(data[7]);

                            feedbacks[feedbackCount] = new ProductFeedback<string>(customer, data[3], int.Parse(data[4]), product);

                            feedbackData[feedbackCount] = line;
                            feedbackCount++;
                        }
                    }
                }

                Console.WriteLine("Data loaded from file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading data: " + ex.Message);
            }
        }
    }
}
