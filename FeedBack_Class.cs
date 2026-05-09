using System;

namespace Project
{
    // Feedback يرث من Driver
    public abstract class Feedback : Driver
    {
        protected int FeedbackId;
        protected string Comment;
        protected int Rating; // 1-5

        public Feedback() : base()
        {
            FeedbackId = 0;
            Comment = "No Comment";
            Rating = 1;
        }

        public Feedback(int feedbackId, string comment, int rating, int driverId, string driverName, string region, Warehouse w)
            : base(driverId, driverName, region, w)
        {
            FeedbackId = feedbackId;
            Comment = comment;
            Rating = (rating >= 1 && rating <= 5) ? rating : 1;
        }

        public int FEEDBACKID
        {
            set { FeedbackId = value; }
            get { return FeedbackId; }
        }

        public string COMMENT
        {
            set { Comment = value; }
            get { return Comment; }
        }

        public int RATING
        {
            set
            {
                if (value >= 1 && value <= 5)
                    Rating = value;
                else
                {
                    Rating = 1;
                    Console.WriteLine("Error: Rating must be between 1 and 5.");
                }
            }
            get { return Rating; }
        }

     
        public abstract void DisplayFeedback();
    }

    // Feedback خاص بالمنتجات
    public class ProductFeedback : Feedback
    {
        private int ProductId;

        public ProductFeedback(int feedbackId, string comment, int rating, int driverId, string driverName, string region, Warehouse w, int productId)
            : base(feedbackId, comment, rating, driverId, driverName, region, w)
        {
            ProductId = productId;
        }

        public override void DisplayFeedback()
        {
            Console.WriteLine($"[Product Feedback] FeedbackID:{FeedbackId} Driver:{NAME} Region:{REGION} ProductID:{ProductId} Rating:{Rating}/5 Comment:{Comment}");
        }
    }

    // Feedback خاص بالخدمات
    public class ServiceFeedback : Feedback
    {
        private string ServiceName;

        public ServiceFeedback(int feedbackId, string comment, int rating, int driverId, string driverName, string region, Warehouse w, string serviceName)
            : base(feedbackId, comment, rating, driverId, driverName, region, w)
        {
            ServiceName = serviceName;
        }

        public override void DisplayFeedback()
        {
            Console.WriteLine($"[Service Feedback] FeedbackID:{FeedbackId} Driver:{NAME} Region:{REGION} Service:{ServiceName} Rating:{Rating}/5 Comment:{Comment}");
        }
    }
}
