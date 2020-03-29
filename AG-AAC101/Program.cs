using AG_AAC101.Data.Commodity;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Console = Colorful.Console;

namespace AG_AAC101
{
    class Program
    {
        public static void Main(string[] args)
        {
            MainApplication(); // Calling the main application
        }


        public static void AddCommodity()
        {
            BlobDbContext db = new BlobDbContext();
            Console.WriteLine("All fields are mandatory and cannot be blank\n");
            Commodity com = new Commodity();
            do
            {
                Console.WriteLine("Commodity Code : ");
                com.CommodityCode = Console.ReadLine();
            }
            while (String.IsNullOrEmpty(com.CommodityCode));
            do
            {
                Console.WriteLine("Commodity Name : ");
                com.CommodityName = Console.ReadLine();
            }
            while (String.IsNullOrEmpty(com.CommodityName));

            do
            {
                Console.WriteLine("Unit : ");
                com.Unit = Console.ReadLine();

            }
            while (String.IsNullOrEmpty(com.Unit));

            do
            {
                Console.WriteLine("Estimated Quantity : ");
                com.EstimatedQuantity = Console.ReadLine();
            }
            while (String.IsNullOrEmpty(com.EstimatedQuantity));

            do
            {
                Console.WriteLine("Actual Quantity : ");
                com.ActualQuantity = Console.ReadLine();

            }
            while (String.IsNullOrEmpty(com.ActualQuantity));


            db.Commodities.Add(com);
            db.SaveChanges();
            Console.WriteLine("\nCommodity added successfully. Hit enter to return to main menu", Color.Green);
            Console.ReadLine();

        }

        public static void DeleteCommodity()
        {

            DisplayAllCommodities();
            BlobDbContext db = new BlobDbContext();
            Commodity com = new Commodity();
            Console.WriteLine("Please enter the Commodity ID that you want to delete : ");
            var input = Console.ReadLine();
            Console.WriteLine();
            if (Int32.TryParse(input, out int userInput))
            {
                var rowcount = db.Commodities.Find(userInput);
                if (rowcount != null)
                {
                    db.Commodities.Remove(rowcount);
                    db.SaveChanges();
                    Console.WriteLine("Commodity successfully deleted. Returning to main menu..", Color.Green);

                }
                else
                {
                    Console.WriteLine("Not a valid commodity ID, Hit enter to return to main menu", Color.Red);
                    Console.ReadLine();


                }
            }


        }

        public static void DisplayAllCommodities()
        {
            int skip = 0;
            int take = 5;

            using var db = new BlobDbContext();
            Console.WriteLine("Displaying all the existing commodities..", Color.Violet);

            List<Commodity> CommodityInfo = db.Commodities.Skip(skip).Take(take).ToList();
             
            while (CommodityInfo?.Count > 0)
            {
                var table = new ConsoleTable("ID", "CommodityCode", "CommodityName", "Unit", "EstimatedQuantity", "ActualQuantity");
                foreach (var item in CommodityInfo)
                {

                    table.AddRow($"{item.ID}", $"{ item.CommodityCode}", $"{item.CommodityName}", $"{item.Unit}", $"{item.EstimatedQuantity}", $"{item.ActualQuantity}");

                }
                table.Write();
                Console.WriteLine("Press Enter to continue with the list of Commodities", Color.Yellow);
                Console.WriteLine("Type exit to return to Main Menu", Color.AntiqueWhite);
                String input = Console.ReadLine();
                String Linput = input.ToLower();
                if (Linput == "exit")
                {
                    MainApplication();
                }

                else
                {
                    skip += CommodityInfo.Count;
                    CommodityInfo = db.Commodities.Skip(skip).Take(take).ToList();
                }
            }

        }

        public static void DisplayCommoditiesbyCode(string input)
        {
            using var db = new BlobDbContext();
            Commodity com = new Commodity();


            Console.WriteLine();
            if (Int32.TryParse(input, out int userInput))
            {
                var rowcount = db.Commodities.Find(userInput);
                if (rowcount != null)
                {
                    var table = new ConsoleTable("ID", "CommodityCode", "CommodityName", "Unit", "EstimatedQuantity", "ActualQuantity");
                    table.AddRow($"{rowcount.ID}", $"{rowcount.CommodityCode}", $"{rowcount.CommodityName}", $"{rowcount.Unit}", $"{rowcount.EstimatedQuantity}", $"{rowcount.ActualQuantity}");
                    table.Write();
                    Console.WriteLine();

                }
                else
                {
                    Console.WriteLine("Not a valid commodity ID, Hit enter to return to main menu", Color.Red);
                    Console.Read();

                }
            }

        }

        public static void UpdateCommodity()
        {
            using var db = new BlobDbContext();
            Console.WriteLine("Please enter the Commodity ID that you want to update : ");
            var input = Console.ReadLine();

            DisplayCommoditiesbyCode(input);
            if (Int32.TryParse(input, out int userInput))
            {
                //var rowcount = db.Commodities.Find(userInput);
                Commodity com = db.Commodities.FirstOrDefault(item => item.ID == userInput);

                if (com != null)
                {
                    Console.Write("Please enter the values that needs to be updated in the below section :\n");
                    Console.WriteLine("Commodity Code : ");
                    var CCode = Console.ReadLine();
                    com.CommodityCode = String.IsNullOrWhiteSpace(CCode) ? com.CommodityCode : CCode;

                    Console.WriteLine("Commodity Name : ");
                    var CName = Console.ReadLine();
                    com.CommodityName = String.IsNullOrWhiteSpace(CName) ? com.CommodityName : CName;

                    Console.WriteLine("Unit : ");
                    var Unit = Console.ReadLine();
                    com.Unit = String.IsNullOrWhiteSpace(Unit) ? com.Unit : Unit;

                    Console.WriteLine("Estimated Quantity : ");
                    var Equantity = Console.ReadLine();
                    com.EstimatedQuantity = String.IsNullOrWhiteSpace(Equantity) ? com.EstimatedQuantity : Equantity;

                    Console.WriteLine("Actual Quantity : ");
                    var Aquantity = Console.ReadLine();
                    com.ActualQuantity = String.IsNullOrWhiteSpace(Aquantity) ? com.ActualQuantity : Aquantity;


                    db.Commodities.Update(com);
                    db.SaveChanges();
                    Console.WriteLine("Commodity updated successfully. Hit Enter to return to Main menu", Color.Green);
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Not a valid commodity ID, Hit enter to return to main menu", Color.Red);
                    Console.ReadLine();


                }
            }
        }//Indiviual Function parenthesis

        public static void MainApplication()
        {
            bool keepLooping = true;
            string selection;
            while (keepLooping)
            {
                Console.WriteLine("####################################################################", Color.AntiqueWhite);
                Console.WriteLine("####################################################################", Color.AntiqueWhite);
                Console.WriteLine("#                                                                  #", Color.AntiqueWhite);
                Console.WriteLine("#            WELCOME TO THE AAC 101 Certification Program          #", Color.Aqua);
                Console.WriteLine("#             (Please follow on-screen guide to navigate)          #", Color.IndianRed);
                Console.WriteLine("#              1.  Display All Commodities                         #", Color.Green);
                Console.WriteLine("#              2.  Display Commodity by Code                       #", Color.Green);
                Console.WriteLine("#              3.  Add Commodity                                   #", Color.Green);
                Console.WriteLine("#              4.  Update Commodity                                #", Color.Green);
                Console.WriteLine("#              5.  Delete Commodity                                #", Color.Green);
                Console.WriteLine("#              6.  Close Program                                   #", Color.Green);
                Console.WriteLine("#                                                                  #", Color.AntiqueWhite);
                Console.WriteLine("####################################################################", Color.AntiqueWhite);
                Console.WriteLine("####################################################################", Color.AntiqueWhite);
                Console.WriteLine("Please choose an option : ",Color.AntiqueWhite);
                selection = Console.ReadLine();
                Console.WriteLine();

                switch (selection)
                {
                    case "1":
                        DisplayAllCommodities();
                        break;
                    case "2":
                        Console.WriteLine("Please enter the Commodity ID : ");
                        var input = Console.ReadLine();
                        DisplayCommoditiesbyCode(input);
                        break;
                    case "3":
                        AddCommodity();
                        break;
                    case "4":
                        UpdateCommodity();
                        break;
                    case "5":
                        DeleteCommodity();
                        break;
                    case "6":
                        Console.WriteLine("Exiting Program.... See ya soon. ", Color.Red);
                        keepLooping = false;
                        Thread.Sleep(2000); //Delay for 2 seconds  and close window
                        System.Environment.Exit(1);
                        break;
                    default:
                        Console.WriteLine("'{0}' is not a valid option, please try again!", selection);
                        break;
                }
                Console.WriteLine();
            }
        }
    }
}//Namespace parenthesis
