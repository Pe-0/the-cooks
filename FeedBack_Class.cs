using Project;
using System;

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

            product.ID = pro.ID;
            product.NAME = pro.NAME;
            product.PRICE = pro.PRICE;

        }

        public override void Display()
        {
            Console.WriteLine("Tour Name : " + customer.NAME + "\nThe Product : " + product.NAME + "\nYour Rating : " + RATING + "\nYour comment : " + COMMENT);
        }

    }


}
