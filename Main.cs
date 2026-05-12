using System;

namespace Project
{
    public class Program
    {
        static Customer[] customers = new Customer[100];
        static Warehouse[] warehouseItems = new Warehouse[100];
        static Order[] orders = new Order[100];
        static Payment[] payments = new Payment[100];
        static Feedback<string>[] feedbacks = new Feedback<string>[100];

        static string[] orderData = new string[100];
        static string[] paymentData = new string[100];
        static string[] feedbackData = new string[100];

        static Customer currentCustomer = null;

        static int customerCount = 0;
        static int warehouseCount = 0;
        static int orderCount = 0;
        static int paymentCount = 0;
        static int feedbackCount = 0;

        static void Main(string[] args)
        {
            LoadDataFromFile();

            int choice;
            do
            {
                try
                {
                    Console.WriteLine("\n==== Online Shipping Menu ====");
                    Console.WriteLine("1. Register / Login");
                    Console.WriteLine("2. Display My Info");
                    Console.WriteLine("3. Display Warehouse Products");
                    Console.WriteLine("4. Search Product by ID");
                    Console.WriteLine("5. Make Order");
                    Console.WriteLine("6. Display My Orders");
                    Console.WriteLine("7. Pay For Order");
                    Console.WriteLine("8. Display My Payments");
                    Console.WriteLine("9. Add Product Feedback");
                    Console.WriteLine("10. Display My Feedback");
                    Console.WriteLine("11. Exit and Save");
                    Console.Write("Enter your choice: ");
                    choice = int.Parse(Console.ReadLine() ?? "");

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
                            DisplayMyPayments();
                            break;
                        case 9:
                            AddProductFeedback();
                            break;
                        case 10:
                            DisplayMyFeedback();
                            break;
                        case 11:
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
            } while (choice != 11);
        }

        static string ReadText()
        {
            return Console.ReadLine() ?? "";
        }

        static int ReadInt()
        {
            return int.Parse(Console.ReadLine() ?? "");
        }

        static double ReadDouble()
        {
            return double.Parse(Console.ReadLine() ?? "");
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

        static Warehouse FindWarehouseItemByProductID(int id)
        {
            for (int i = 0; i < warehouseCount; i++)
            {
                if (warehouseItems[i].ProductItem.ID == id)
                    return warehouseItems[i];
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

            if (customerCount >= customers.Length)
            {
                Console.WriteLine("Customer list is full.");
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
            Console.WriteLine("\n--- Warehouse Products ---");

            if (warehouseCount == 0)
            {
                Console.WriteLine("No products available now.");
                return;
            }

            for (int i = 0; i < warehouseCount; i++)
                warehouseItems[i].DisplayInfo();
        }

        static void SearchProductByID()
        {
            Console.Write("Enter Product ID: ");
            Warehouse warehouseItem = FindWarehouseItemByProductID(ReadInt());

            if (warehouseItem == null)
                Console.WriteLine("Product not found.");
            else
                warehouseItem.DisplayInfo();
        }

        static void MakeOrder()
        {
            if (!HasCustomer())
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

            DisplayAllProducts();
            Console.Write("Enter Product ID to order: ");
            Warehouse warehouseItem = FindWarehouseItemByProductID(ReadInt());

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
            double shippingCost = 50;
            double discount = 0;

            orders[orderCount] = new Order(currentCustomer, product, shippingCost, discount);
            orderData[orderCount] = Order.BuildFileLine(currentCustomer, product, shippingCost, discount);
            orderCount++;
            warehouseItem.DecreaseStock();

            Console.WriteLine("Order created successfully.");
            Console.WriteLine("Shipping cost: " + shippingCost);
            Console.WriteLine("Remaining quantity: " + warehouseItem.QUANTITY);
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

        static bool IsOrderPaid(int orderNumber)
        {
            for (int i = 0; i < paymentCount; i++)
            {
                string[] data = paymentData[i].Split(',');

                if (data.Length > 0 && data[0] == orderNumber.ToString())
                    return true;
            }

            return false;
        }

        static void PayForOrder()
        {
            if (!HasCustomer())
                return;

            if (paymentCount >= payments.Length)
            {
                Console.WriteLine("Payment list is full.");
                return;
            }

            DisplayMyOrders();
            Console.Write("Enter order number to pay: ");
            int orderNumber = ReadInt();

            if (orderNumber < 1 || orderNumber > orderCount)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            if (IsOrderPaid(orderNumber))
            {
                Console.WriteLine("This order is already paid.");
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

            Payment payment;
            if (paymentType == 1)
            {
                Console.Write("Enter 16 digit card number: ");
                string cardNumber = ReadText();
                Console.Write("Enter your card balance: ");
                double balance = ReadDouble();
                payment = new CreditCardPayment(currentCustomer, product, shippingCost, discount, cardNumber, balance);
            }
            else if (paymentType == 2)
            {
                payment = new CashOnDelivery(currentCustomer, product, shippingCost, discount);
            }
            else
            {
                Console.WriteLine("Invalid payment method.");
                return;
            }

            if (!payment.IS_SUCCESSFUL)
                return;

            payments[paymentCount] = payment;
            paymentData[paymentCount] = Payment.BuildFileLine(orderNumber, orderData[orderNumber - 1], payment.PAYMENT_METHOD, payment.AMOUNT);
            paymentCount++;
            Console.WriteLine("Payment saved successfully.");
        }

        static void DisplayMyPayments()
        {
            if (!HasCustomer())
                return;

            Console.WriteLine("\n--- My Payments ---");
            bool found = false;

            for (int i = 0; i < paymentCount; i++)
            {
                string[] data = paymentData[i].Split(',');

                if (data.Length >= 13 && data[1] == currentCustomer.ID.ToString())
                {
                    Console.WriteLine("Order Number: " + data[0]);
                    Console.WriteLine("Product: " + data[6]);
                    Console.WriteLine("Payment Method: " + data[11]);
                    Console.WriteLine("Amount: " + data[12]);
                    Console.WriteLine();
                    found = true;
                }
            }

            if (!found)
                Console.WriteLine("You have no payments.");
        }

        static void AddProductFeedback()
        {
            if (!HasCustomer())
                return;

            Console.Write("Enter Product ID: ");
            Warehouse warehouseItem = FindWarehouseItemByProductID(ReadInt());

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
            string comment = ReadText();
            Console.Write("Enter rating from 1 to 5: ");
            int rating = ReadInt();

            feedbacks[feedbackCount] = new ProductFeedback<string>(currentCustomer, comment, rating, product);
            feedbackData[feedbackCount] = ProductFeedback<string>.BuildFileLine(currentCustomer, product, comment, rating);
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
                Customer.SaveAllToFile(customers, customerCount);
                Warehouse.SaveAllToFile(warehouseItems, warehouseCount);
                Order.SaveAllToFile(orderData, orderCount);
                Payment.SavePaymentsToFile(paymentData, paymentCount);
                ProductFeedback<string>.SaveAllToFile(feedbackData, feedbackCount);

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
                customerCount = Customer.LoadAllFromFile(customers);
                warehouseCount = Warehouse.LoadAllFromFile(warehouseItems);
                orderCount = Order.LoadAllFromFile(orders, orderData);
                paymentCount = Payment.LoadPaymentsFromFile(paymentData);
                feedbackCount = ProductFeedback<string>.LoadAllFromFile(feedbacks, feedbackData);
                Console.WriteLine("Data loaded from file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading data: " + ex.Message);
            }
        }

    }
}
