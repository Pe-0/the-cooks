namespace ConsoleApp5

{
    //Peter Part
    public interface ICustomer
    {
        string NAME { get; set; }
        string ADDRESS { get; set; }
        int ID { get; set; }
        string PHONE { get; set; }
        void Display();
    }

    public interface IProduct
    {
        int ID { get; set; }
        string NAME { get; set; }
        double PRICE { get; set; }
        string PRODUCT_DESCRIPTION { get; set; }
        void DisplayInfo();
    }
}
