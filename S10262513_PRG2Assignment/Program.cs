using Microsoft.VisualBasic;
using S10262513_PRG2Assignment;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;
using System.IO;
using System.Collections;
using System;
using System.Xml.Serialization;
using Microsoft.VisualBasic.FileIO;
using System.Xml.Linq;
//==========================================================
// Student Number : S10262513
// Student Name : Tan Hong Rong
// Partner Name : Jayden Toh Xuan Ming
//==========================================================

class Program
{
    static List<Customer> customersList = new List<Customer>();
    static Queue<Customer> orders = new Queue<Customer>();
    static List<Customer> tempList = customersList.ToList();
    static void DisplayMenu()
    {
        // Your menu logic goes here
        Console.WriteLine("---------------- Menu -----------------");
        Console.WriteLine("[1] List All Customers");
        Console.WriteLine("[2] Register a New Customer");
        Console.WriteLine("[3] Create a Customer's Order");
        Console.WriteLine("[4] Display Order Details of a Customer");
        Console.WriteLine("[5] Modify Order Details");
        Console.WriteLine("[0] Exit");
        Console.WriteLine("--------------------------------------");
    }

    static void Main()
    {
        try
        {
            string choice;
            while (true)
            {
                DisplayMenu();

                Console.Write("Enter Your Option: ");
                choice = Console.ReadLine();
                if (choice == "0")
                    break;
                else if (choice == "1")
                {
                    Listallcustomers(customersList);
                }
                else if (choice == "2")
                {
                    DisplayOrders(customersList);
                }
                else if (choice == "3")
                {

                }
                else if (choice == "4")
                {
                    
                }
                else if (choice == "5")
                {
                    DisplayOrderDetailsOfCustomer(customersList);
                }
                else if (choice == "6")
                {
                    Modifyorderdetails(customersList);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    // 1) Display order details of a customer
    static void Listallcustomers(List<Customer>c)
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
                string tier = data[3];
                int point = Convert.ToInt32(data[4]);
                int punchCard = Convert.ToInt32(data[5]);

                c.Add(new Customer(name, id, dob));
                for (int i = 0; i < c.Count; i++)
                {
                    if (c[i].MemberId == id)
                    {
                        c[i].Rewards.Tier = tier;
                        c[i].Rewards.Points = point;
                        c[i].Rewards.PunchCard = punchCard;
                        break;
                    }
                }
            }

            Console.WriteLine($"{"Name",-10} {"MemberID",-12} {"DOB",-13} {"MemberStatus",-15} {"Points",-10} {"PunchCard",-10}");
            foreach (Customer customer in c)
            {
                Console.WriteLine($"{customer.Name,-10} {customer.MemberId,-12} {customer.Dob.ToString("dd/MM/yyyy"),-13} {customer.Rewards.Tier,-15} {customer.Rewards.Points,-10} {customer.Rewards.PunchCard,-10}");
            }
        }
    }


    // 2) List all current orders
    // Display the information of all current orders in both the gold members and regular queue
    static void DisplayOrders(List<Customer>c)
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

                if (data[6] == null && data[7] == null && data[8] == null && data[10] == null && data[11] == null && data[12] == null && data[13] == null && data[14] == null)
                {
                    continue;
                }
                else
                {
                    bool dipped = Convert.ToBoolean(data[6]);
                    string waffleflavour = data[7];
                    string f1 = data[8];
                    string f2 = data[9];
                    string f3 = data[10];
                    string t1 = data[11];
                    string t2 = data[12];
                    string t3 = data[13];
                    string t4 = data[14];

                    List<Flavour> fList = new List<Flavour>();
                    List<Topping> tList = new List<Topping>();
                    switch (f1)
                    {
                        case "Durian":
                            break;
                        case "Ube":
                            break;

                        case "Sea salt":
                            break;
                    }

                    tList.Add(new Topping(t1));
                    tList.Add(new Topping(t2));
                    tList.Add(new Topping(t3));
                    tList.Add(new Topping(t4));
                    IceCream iceCream;

                    if (option == "cup")
                    {
                        iceCream = new Cup(option, scoop, fList, tList);
                        for (int i = 0; i < c.Count; i++)
                        {
                            if (c[i].MemberId == memid)
                            {
                                c[i].MakeOrder();
                                c[i].CurrentOrder.Id = id;
                                c[i].CurrentOrder.TimeReceived = tr;
                                c[i].CurrentOrder.TimeFulfilled = tf;
                                c[i].CurrentOrder.AddIceCream(iceCream);
                            }
                        }
                    }
                    else if (option == "cone")
                    {
                        iceCream = new Cone(option, scoop, fList, tList, dipped);
                        for (int i = 0; i < c.Count; i++)
                        {
                            if (c[i].MemberId == memid)
                            {
                                c[i].MakeOrder();
                                c[i].CurrentOrder.Id = id;
                                c[i].CurrentOrder.TimeReceived = tr;
                                c[i].CurrentOrder.TimeFulfilled = tf;
                                c[i].CurrentOrder.AddIceCream(iceCream);
                            }
                        }
                    }
                    else if (option == "waffle")
                    {
                        iceCream = new Waffle(option, scoop, fList, tList, waffleflavour);
                        for (int i = 0; i < c.Count; i++)
                        {
                            if (c[i].MemberId == memid)
                            {
                                c[i].MakeOrder();
                                c[i].CurrentOrder.Id = id;
                                c[i].CurrentOrder.TimeReceived = tr;
                                c[i].CurrentOrder.TimeFulfilled = tf;
                                c[i].CurrentOrder.AddIceCream(iceCream);
                            }
                        }
                    }
                }
            }
        }
    }

    // 5) Display order details of a customer
    // List the customers, prompt the user to select a customer, and retrieve order details
    static void DisplayOrderDetailsOfCustomer(List<Customer>c)
    {
        Console.WriteLine($"{"Name",-10} {"MemberID",-12} {"DOB",-13} {"MemberStatus",-15} {"Points",-10} {"PunchCard",-10}");
        foreach (Customer customer in c)
        {
            Console.WriteLine($"{customer.Name,-10} {customer.MemberId,-12} {customer.Dob.ToString("dd/MM/yyyy"),-13} {customer.Rewards.Tier,-15} {customer.Rewards.Points,-10} {customer.Rewards.PunchCard,-10}");
        }
        Console.WriteLine("Select a customer: ");
        int memId = Convert.ToInt32(Console.ReadLine());
        for (int i = 0; i < c.Count; i++)
        {
            if (c[i].MemberId == memId)
            {
                Console.WriteLine("Current order: ");
                //foreach (Order o in c[i].CurrentOrder)
                //{
                //    Console.WriteLine($"{o}");
                //    foreach (IceCream ice in o.IceCreamList)
                //    {
                //        Console.WriteLine(ice);
                //    }
                //}
                Console.WriteLine("Order History: ");
                foreach(Order o in c[i].OrderHistory)
                {
                    Console.WriteLine($"{o}");
                    foreach(IceCream ice in o.IceCreamList)
                    {
                        Console.WriteLine(ice);
                    }
                }
                
            }
      }

            
    }
   // 6) Modifyorderdetails
    static void Modifyorderdetails(List<Customer>c)
    {
        try
        {
            Console.WriteLine($"{"Name",-10} {"MemberID",-12} {"DOB",-13} {"MemberStatus",-15} {"Points",-10} {"PunchCard",-10}");
            foreach (Customer customer in c)
            {
                Console.WriteLine($"{customer.Name,-10} {customer.MemberId,-12} {customer.Dob.ToString("dd/MM/yyyy"),-13} {customer.Rewards.Tier,-15} {customer.Rewards.Points,-10} {customer.Rewards.PunchCard,-10}");
            }

            Console.WriteLine("en");
            int memid = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < c.Count; i++)
            {
                if (memid == c[i].MemberId)
                {
                    Console.WriteLine($"{ c[i].CurrentOrder.IceCreamList}");
                    Console.WriteLine("[1]choose an existing ice cream object to modify");
                    Console.WriteLine("[2] add an entirely new ice cream object to the order");
                    Console.WriteLine("[3] choose an existing ice cream object to delete from the order");
                    try
                    {
                        string choice = Console.ReadLine();
                        if (choice == "1")
                        {
                            Console.WriteLine("which ice cream to modify");
                            int id = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Option :");
                            string Option= Console.ReadLine();
                            Console.WriteLine("scoops: ");
                            int scoops = Convert.ToInt32(Console.ReadLine());
                            if (scoops > 3)
                            {
                                Console.WriteLine("Scoops cannot be more than 3.");
                                return;
                            }
                            Console.WriteLine("Flavours: ");
                            string flavours = Console.ReadLine();
                            Console.WriteLine("Toppings: ");
                            string toppings = Console.ReadLine();

                            if(Option == "Cone" || Option == "cone")
                            {
                                Console.WriteLine("dipped cone: ");
                                string dipped = Console.ReadLine();
                            }
                            else if(Option=="Waffle"|| Option == "waffle")
                            {
                                Console.WriteLine("waffle flavour: ");
                                string wflavour= Console.ReadLine();
                            }

                            c[i].CurrentOrder.IceCreamList[id].Option = Option;
                            c[i].CurrentOrder.IceCreamList[id].Scoops = scoops;
                            //tempList[i].CurrentOrder.IceCreamList[id].Flavours=flavours;
                            //tempList[i].CurrentOrder.IceCreamList[id].Toppings=toppings;

                        }
                        else if (choice == "2")
                        {
                            Console.WriteLine();
                        }
                        else if (choice == "3")
                        {
                            if (c[i].CurrentOrder.IceCreamList.Count != 0)
                            {
                                Console.WriteLine("which ice cream to delete");
                                int id = Convert.ToInt32(Console.ReadLine());
                                c[i].CurrentOrder.DeleteIceCream(id);
                            }
                            else
                            {
                                Console.WriteLine("cannot have zero ice creams in an order");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }


                }
            }

        }
        catch(Exception e) 
        {
            Console.WriteLine(e.Message);
        }
    }
}
