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
                DisplayMenu();
                choice = ConsoleInput.ReadInt();
                ExecuteChoice(choice);
            } while (choice != 11);
        }

        static void DisplayMenu()
        {
            Console.WriteLine();
            Console.WriteLine("==== Online Shipping Menu ====");
            Console.WriteLine(" 1. Register / Login");
            Console.WriteLine(" 2. Display My Info");
            Console.WriteLine(" 3. Display Warehouse Products");
            Console.WriteLine(" 4. Search Product by ID");
            Console.WriteLine(" 5. Make Order");
            Console.WriteLine(" 6. Display My Orders");
            Console.WriteLine(" 7. Pay For Order");
            Console.WriteLine(" 8. Display My Payments");
            Console.WriteLine(" 9. Add Product Feedback");
            Console.WriteLine("10. Display My Feedback");
            Console.WriteLine("11. Exit and Save");
            Console.WriteLine("------------------------------");
            Console.Write("Enter your choice: ");
        }

        static void ExecuteChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    currentCustomer = Customer.RegisterOrLogin(customers, ref customerCount);
                    break;
                case 2:
                    Customer.DisplayCurrentCustomer(currentCustomer);
                    break;
                case 3:
                    Warehouse.DisplayAllProducts(warehouseItems, warehouseCount);
                    break;
                case 4:
                    Warehouse.SearchProductByID(warehouseItems, warehouseCount);
                    break;
                case 5:
                    Order.MakeOrder(currentCustomer, warehouseItems, warehouseCount, orders, orderData, ref orderCount);
                    break;
                case 6:
                    Order.DisplayMyOrders(currentCustomer, orderData, orderCount);
                    break;
                case 7:
                    Payment.PayForOrder(currentCustomer, orderData, orderCount, payments, paymentData, ref paymentCount);
                    break;
                case 8:
                    Payment.DisplayMyPayments(currentCustomer, paymentData, paymentCount);
                    break;
                case 9:
                    ProductFeedback<string>.AddProductFeedback(currentCustomer, warehouseItems, warehouseCount, feedbacks, feedbackData, ref feedbackCount);
                    break;
                case 10:
                    ProductFeedback<string>.DisplayMyFeedback(currentCustomer, feedbacks, feedbackData, feedbackCount);
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

        static void SaveDataToFile()
        {
            Customer.SaveAllToFile(customers, customerCount);
            Warehouse.SaveAllToFile(warehouseItems, warehouseCount);
            Order.SaveAllToFile(orderData, orderCount);
            Payment.SavePaymentsToFile(paymentData, paymentCount);
            ProductFeedback<string>.SaveAllToFile(feedbackData, feedbackCount);
        }

        static void LoadDataFromFile()
        {
            customerCount = Customer.LoadAllFromFile(customers);
            warehouseCount = Warehouse.LoadAllFromFile(warehouseItems);
            orderCount = Order.LoadAllFromFile(orders, orderData);
            paymentCount = Payment.LoadPaymentsFromFile(paymentData);
            feedbackCount = ProductFeedback<string>.LoadAllFromFile(feedbacks, feedbackData);

            if (warehouseCount == 0)
                warehouseCount = Warehouse.AddDefaultProducts(warehouseItems, warehouseCount);

            Console.WriteLine("Data loaded from file successfully.");
        }
    }
}
