using System;
using System.IO;

namespace Project
{
    public class Warehouse
    {
        private static int countPhone ;
        private static int countPc ;
        private static int countLaptop ;
       
        public Warehouse()
        {
            countLaptop = 50 ;
            countPhone = 50 ;
            countPc = 50 ;
        }
       

        public void IncrementPhone()
        {
            countPhone++;
          
          
        }

        public void DecrementPhone()
        {
            if (countPhone > 0)
            {
                countPhone--;
              
               
            }
            else
            {
                Console.WriteLine("No phones in stock.");
            }
        }

        public void IncrementPc()
        {
            countPc++;
          
           
        }

        public void DecrementPc()
        {
            if (countPc > 0)
            {
                countPc--;
              
             
            }
            else
            {
                Console.WriteLine("No PCs in stock.");
            }
        }

        public void IncrementLaptop()
        {
            countLaptop++;
           
        }

        public void DecrementLaptop()
        {
            if (countLaptop > 0)
            {
                countLaptop--;
             
               
            }
            else
            {
                Console.WriteLine("No laptops in stock.");
            }
        }

      
        public void DisplayStockPc()
        {
            
            Console.WriteLine("PC: " + countPc);
           
        }
        public void DisplayStockLaptop()
        {

            Console.WriteLine("Laptop : " + countLaptop);

        }
        public void DisplayStockPhone()
        {

            Console.WriteLine("PC: " + countPhone);

        }

    }
}
