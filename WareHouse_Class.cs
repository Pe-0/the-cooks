using System;
using System.IO;

namespace Project
{
    public class Warehouse
    {
        private static int countPhone = 0;
        private static int countPc = 0;
        private static int countLaptop = 0;
        private string filePath = "warehouse.txt";

        public Warehouse()
        {
            LoadFromFile();
        }

        public void IncrementPhone()
        {
            countPhone++;
            Console.WriteLine("Phone = " + countPhone);
            SaveToFile();
        }

        public void DecrementPhone()
        {
            if (countPhone > 0)
            {
                countPhone--;
                Console.WriteLine("Phone = " + countPhone);
                SaveToFile();
            }
            else
            {
                Console.WriteLine("No phones in stock.");
            }
        }

        public void IncrementPc()
        {
            countPc++;
            Console.WriteLine("PC = " + countPc);
            SaveToFile();
        }

        public void DecrementPc()
        {
            if (countPc > 0)
            {
                countPc--;
                Console.WriteLine("PC = " + countPc);
                SaveToFile();
            }
            else
            {
                Console.WriteLine("No PCs in stock.");
            }
        }

        public void IncrementLaptop()
        {
            countLaptop++;
            Console.WriteLine("Laptop = " + countLaptop);
            SaveToFile();
        }

        public void DecrementLaptop()
        {
            if (countLaptop > 0)
            {
                countLaptop--;
                Console.WriteLine("Laptop = " + countLaptop);
                SaveToFile();
            }
            else
            {
                Console.WriteLine("No laptops in stock.");
            }
        }

        public static int GetCountPhone() { return countPhone; }
        public static int GetCountPc() { return countPc; }
        public static int GetCountLaptop() { return countLaptop; }

        private void SaveToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Phone," + countPhone);
                    writer.WriteLine("PC," + countPc);
                    writer.WriteLine("Laptop," + countLaptop);
                }
                Console.WriteLine("Stock saved to file.");
            }
            catch (IOException e)
            {
                Console.WriteLine("Error saving file: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Operation completed.");
            }
        } 

        private void LoadFromFile()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string? line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.Contains(","))
                            {
                                string[] parts = line.Split(',');

                                if (parts.Length == 2)
                                {
                                    string item = parts[0];
                                    int value = int.Parse(parts[1]);

                                    if (item == "Phone")
                                        countPhone = value;
                                    else if (item == "PC")
                                        countPc = value;
                                    else if (item == "Laptop")
                                        countLaptop = value;
                                }
                            }
                        }
                    }
                    Console.WriteLine("Stock loaded from file.");
                }
                else
                {
                    Console.WriteLine("No saved stock found. Starting fresh.");
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found: " + e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine("Error reading file: " + e.Message);
            }
        }


        public void DisplayStock()
        {
            Console.WriteLine(" Warehouse Stock ");
            Console.WriteLine("Phone: " + countPhone);
            Console.WriteLine("PC: " + countPc);
            Console.WriteLine("Laptop: " + countLaptop);
        }
    }
}
