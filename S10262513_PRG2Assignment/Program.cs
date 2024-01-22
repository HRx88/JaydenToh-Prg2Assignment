using Microsoft.VisualBasic;
using S10262513_PRG2Assignment;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;
using System.IO;
using System.Collections;
//==========================================================
// Student Number : S10262513
// Student Name : Tan Hong Rong
// Partner Name : Jayden Toh Xuan Ming
//==========================================================

class Program
{
    static List<Customer> customersList = new List<Customer>();
    static Queue<Customer> orders = new Queue<Customer>();
    static void DisplayMenu()
    {
        // Your menu logic goes here
        Console.WriteLine("---------------- Menu -----------------");
        Console.WriteLine("[1] List All Customers");
        Console.WriteLine("[2] Register a New Customer");
        Console.WriteLine("[3] Create a Customer's Order");
        Console.WriteLine("[4]Display Order Details of a Customer");
        Console.WriteLine("[5] Modify Order Details");
        Console.WriteLine("[0] Exit");
        Console.WriteLine("--------------------------------------");
    }

    static void Main()
    {
        string choice;
        while (true)
        {
            DisplayMenu();

            Console.Write("Enter Your Option: ");
            choice = Console.ReadLine();

            if (choice == "1")
            {
                Listallcustomers();
            }

            else if (choice == "2")
            {

            }

            else if (choice == "3")
            {

            }

            else if (choice == "4")
            {

            }

            else if (choice == "5")
            {

            }

            else if (choice == "6")
            {

            }
        }
    }


    // 1) Display order details of a customer
    static void Listallcustomers()
    {
        using (StreamReader sr = new StreamReader("customers.csv"))
        {
            string? s = sr.ReadLine(); // Read the heading and discard
            while ((s = sr.ReadLine()) != null)
            {
                string[] data = s.Split(',');
                string name = data[0];
                int id = Convert.ToInt32(data[1]);
                DateTime dob = Convert.ToDateTime(data[2]);
                customersList.Add(new Customer(name, id, dob));
            }

            foreach (Customer customer in customersList)
            {
                Console.WriteLine(customer);
            }
        }
    }

    // 2) List all current orders
    // Display the information of all current orders in both the gold members and regular queue
    static void DisplayOrders()
    {
        using (StreamReader sr = new StreamReader("orders.csv"))
        {
            string? s = sr.ReadLine(); // Read the heading and discard
            while ((s = sr.ReadLine()) != null)
            {
                string[] data = s.Split(',');
                int id = Convert.ToInt32(data[0]);
                int memid = Convert.ToInt32(data[1]);
                DateTime tr = Convert.ToDateTime(data[2]);
                DateTime tf = Convert.ToDateTime(data[3]);
                string option = data[4];
                int scoop = Convert.ToInt32(data[5]);
                bool dipped = Convert.ToBoolean(data[6]);
                string waffleflavour = data[7];
                string f1 = data[8];
                string f2 = data[9];
                string f3 = data[10];
                string t1 = data[11];
                string t2 = data[12];
                string t3 = data[13];
                string t4 = data[14];

                // Process order data as needed
            }
        }
    }

    // 5) Display order details of a customer
    // List the customers, prompt the user to select a customer, and retrieve order details
    static void DisplayOrderDetailsOfCustomer()
    {
        Listallcustomers();
        Console.WriteLine("Select a customer: ");
        // Additional logic for retrieving and displaying order details
    }
}
