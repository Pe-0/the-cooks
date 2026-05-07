using System;

namespace Project
{
    public class Delivery
    {
        protected int DeliveryId;

        protected Order order;

        protected Driver driver;

        protected Warehouse warehouse;

        protected string status;

        protected string ProductType;

        public Delivery()
        {
            DeliveryId = 0;

            order = null;

            driver = null;

            warehouse = null;

            status = "Pending";

            ProductType = "";
        }

        public Delivery(int id, Order o, Driver d, Warehouse w, string productType)
        {
            DeliveryId = id;

            order = o;

            driver = d;

            warehouse = w;

            ProductType = productType;

            status = "Pending";
        }

        public void StartDelivery()
        {
            status = "Shipping";

            if (driver != null && order != null)
            {
                driver.DeliverOrder(order, ProductType);
            }

            Console.WriteLine("Delivery Started");
            Console.WriteLine("Status: " + status);
        }

        public void CompleteDelivery()
        {
            status = "Delivered";

            Console.WriteLine("Delivery Completed");
            Console.WriteLine("Status: " + status);
        }

        public void Display()
        {
            Console.WriteLine("Delivery ID: " + DeliveryId);

            Console.WriteLine("Product Type: " + ProductType);

            Console.WriteLine("Status: " + status);
        }
    }
}
