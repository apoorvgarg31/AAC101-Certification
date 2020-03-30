using AG_AAC101.Data.Commodity;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
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
            Console.WriteLine("All fields are mandatory and cannot be blank\n" , Color.AliceBlue);
            Commodity com = new Commodity();
            do
            {
                Console.WriteLine("Commodity Code: ");
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
            Console.WriteLine("Extracting Commodities information.......\n\n", Color.CornflowerBlue);
            using var db = new BlobDbContext();
            List<Commodity> CommodityCount = db.Commodities.ToList();
            int CommodityCount1 = db.Commodities.Count();
            Console.WriteLine("Total Number of Commodities found : {0}", CommodityCount1, Color.CornflowerBlue);
            
            if (CommodityCount?.Count > 20)
            {
                Console.WriteLine("Since commodities count is greater than 20 therefore do you want to send it over e-mail instead ? \n\nChoose : \n 1: yes\n 2: No", Color.CornflowerBlue);
                String userinput = Console.ReadLine();
                if (userinput == "1")
                {
                    var table = new ConsoleTable("ID", "CommodityCode", "CommodityName", "Unit", "EstimatedQuantity", "ActualQuantity");
                    foreach (var item in CommodityCount)
                    {

                        table.AddRow($"{item.ID}", $"{ item.CommodityCode}", $"{item.CommodityName}", $"{item.Unit}", $"{item.EstimatedQuantity}", $"{item.ActualQuantity}");

                    }

                    var EmailInput = table;
                    SendMail(EmailInput);
                }
                else
                {
                    DisplayCommoditiesbyPage();
                }
            }
            else
            {
                DisplayCommoditiesbyPage();

            }

            static void DisplayCommoditiesbyPage()
            {
                try
                {
                    using var db = new BlobDbContext();


                    int skip = 0;
                    Console.WriteLine("Please enter the number of records you want to display on one page : ", Color.Violet);
                    var take = Int32.Parse(Console.ReadLine());

                    if (take == 0)
                    {
                        Console.WriteLine("Page length cannot be 0. Hit enter to return to main menu", Color.Red);
                        Console.Read();
                        MainApplication();
                    }
                    Console.WriteLine("Displaying all commodities as per input..", Color.Violet);
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
                catch (Exception ex)
                {

                    Console.WriteLine("Page length cannot be blank. Hit enter to return to main menu.", Color.Red);
                    Console.ReadLine();
                    MainApplication();

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
                    Console.WriteLine("Please enter to return to main menu");
                    Console.Read();

                }
                else
                {
                    Console.WriteLine("Not a valid commodity ID, Hit enter to return to main menu", Color.Red);
                    Console.Read();
                    MainApplication();

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
                    Console.Write("Please enter the values that needs to be updated in the below section.\n (Leaving Blank will not wipe out the values but leaving a white space will. ) :\n");
                    Console.WriteLine("Commodity Code : ");
                    var CCode = Console.ReadLine();
                    com.CommodityCode = String.IsNullOrEmpty(CCode) ? com.CommodityCode : CCode;
                    if (CCode == " ")
                    {
                        com.CommodityCode = " ";
                    }
                    Console.WriteLine("Commodity Name : ");
                    var CName = Console.ReadLine();
                    com.CommodityName = String.IsNullOrEmpty(CName) ? com.CommodityName : CName;
                    if (CName == " ")
                    {
                        com.CommodityName = " ";
                    }
                    Console.WriteLine("Unit : ");
                    var Unit = Console.ReadLine();
                    com.Unit = String.IsNullOrEmpty(Unit) ? com.Unit : Unit;
                    if (Unit == " ")
                    {
                        com.Unit = " ";
                    }
                    Console.WriteLine("Estimated Quantity : ");
                    var Equantity = Console.ReadLine();
                    com.EstimatedQuantity = String.IsNullOrEmpty(Equantity) ? com.EstimatedQuantity : Equantity;
                    if (Equantity == " ")
                    {
                        com.EstimatedQuantity = " ";
                    }
                    Console.WriteLine("Actual Quantity : ");
                    var Aquantity = Console.ReadLine();
                    com.ActualQuantity = String.IsNullOrEmpty(Aquantity) ? com.ActualQuantity : Aquantity;
                    if (Aquantity == " ")
                    {
                        com.ActualQuantity = " ";
                    }

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
        }

        private static void SendMail(ConsoleTable EmailInput)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                Console.WriteLine("Please enter your Email : ");
                var email = Console.ReadLine();

                var subject = "Commodities Information : " + DateTime.Now;

                mail.From = new MailAddress(email);

                mail.To.Add("agarg@enstoa.com");

                mail.Subject = subject;
                var Feedback = EmailInput.ToString();

                mail.Body = Feedback;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("apoorvgarg31@gmail.com", "lrowrxdjguquysyu");
                SmtpServer.EnableSsl = true;
                Console.WriteLine("The Commodities information being sent to email. Sit back and Relax ....", Color.Blue);

                SmtpServer.Send(mail);
                Console.WriteLine("Information sent successfully. Hit enter to return to main menu. ", Color.Green);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email cannot be sent. Please check the below error - \n");
                if (ex.ToString().Contains("The specified string is not in the form required for an e-mail address."))
                {
                    Console.WriteLine("Please check the email address in From Section: ");
                }
                else
                {
                    Console.WriteLine(ex.ToString());
                }
                Console.WriteLine("Hit enter to return to main menu and restart the process : ");
                Console.ReadLine();

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
                Console.WriteLine("#              6.  Clear Console                                   #", Color.Green);
                Console.WriteLine("#              7.  Close Program                                   #", Color.Green);
                Console.WriteLine("#                                                                  #", Color.AntiqueWhite);
                Console.WriteLine("####################################################################", Color.AntiqueWhite);
                Console.WriteLine("####################################################################", Color.AntiqueWhite);
                Console.WriteLine("Please choose an option : ", Color.AntiqueWhite);
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
                        Console.WriteLine("Lets' extract the commodities information before deleting. \n");
                        DeleteCommodity();
                        break;
                    case "6":
                        Console.Clear();
                        break;
                    case "7":
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
