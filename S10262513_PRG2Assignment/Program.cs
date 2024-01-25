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
            InitData(customersList);
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
                    ListAllCustomers(customersList);
                }
                else if (choice == "2")
                {
                    DisplayOrders(customersList);
                }
                else if (choice == "3")
                {
                    RegisterNewCustomer(customersList);
                }
                else if (choice == "4")
                {
                    CreateCustomerOrder(customersList);
                }
                else if (choice == "5")
                {
                    DisplayOrderDetailsOfCustomer(customersList);
                }
                else if (choice == "6")
                {
                    ModifyOrderDetails(customersList);
                }
                else
                {
                    Console.WriteLine("Enter 1-8");
                }
            }
    }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    // option 1
    // Display order details of a customer
    static void InitData(List<Customer> c)
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
        }
    }

    static void ListAllCustomers(List<Customer>c)
    {
        Console.WriteLine($"{"Name",-10} {"MemberID",-12} {"DOB",-13} {"MemberStatus",-15} {"Points",-10} {"PunchCard",-10}");
        foreach (Customer customer in c)
        {
            Console.WriteLine($"{customer.Name,-10} {customer.MemberId,-12} {customer.Dob.ToString("dd/MM/yyyy"),-13} {customer.Rewards.Tier,-15} {customer.Rewards.Points,-10} {customer.Rewards.PunchCard,-10}");
        }
    }

    // Option 2
    // List all current orders
    // Display the information of all current orders in both the gold members and regular queue
    static void DisplayOrders(List<Customer> c)
    {
        using (StreamReader sr = new StreamReader("orders.csv"))
        {
            string? s = sr.ReadLine(); 
            while ((s = sr.ReadLine()) != null)
            {
                string[] data = s.Split(',');
                int id = Convert.ToInt32(data[0]);
                int memid = Convert.ToInt32(data[1]);
                DateTime tr = Convert.ToDateTime(data[2]);
                DateTime tf = Convert.ToDateTime(data[3]);
                string option = data[4];
                int scoop = Convert.ToInt32(data[5]);

                bool? dipped = false;
                if (!string.IsNullOrEmpty(data[6]) && bool.TryParse(data[6], out bool result))
                {
                    dipped = result;
                }
                string? waffleflavour = data[7];
                    string? f1 = data[8];
                    string? f2 = data[9];
                    string? f3 = data[10];
                    string? t1 = data[11];
                    string? t2 = data[12];
                    string? t3 = data[13];
                    string? t4 = data[14];
            List<string>f=new List<string> { f1, f2, f3 };
                List<string>t=new List<string> { t1, t2, t3, t4 };
               
                    List<Flavour> fList = new List<Flavour>();
                    List<Topping> tList = new List<Topping>();
                foreach(string flav in f)
                {
                    if (!string.IsNullOrEmpty(flav))
                    {
                        switch (flav)
                        {
                            case "Durian":
                                fList.Add(new Flavour(flav, true));
                                break;
                            case "Ube":
                                fList.Add(new Flavour(flav, true));
                                break;
                            case "Sea salt":
                                fList.Add(new Flavour(flav, true));
                                break;
                            default:
                                fList.Add(new Flavour(flav, false));
                                break;
                        }
                    }
                }
                
                foreach(string top in t)
                {
                   tList.Add(new Topping(top));

                }
                IceCream iceCream;
                
                for (int i = 0; i < c.Count; i++)
                {
                    if (c[i].MemberId == memid)
                    {
                        c[i].MakeOrder();
                        c[i].CurrentOrder.Id = id;
                        c[i].CurrentOrder.TimeReceived = tr;
                        c[i].CurrentOrder.TimeFulfilled = tf;
                        if (option == "Cup")
                        {
                            iceCream = new Cup(option, scoop, fList, tList);
                            c[i].CurrentOrder.AddIceCream(iceCream);
                        }
                        else if (option == "Cone")
                        {
                            iceCream = new Cone(option, scoop, fList, tList, dipped ?? false );
                            c[i].CurrentOrder.AddIceCream(iceCream);
                        }
                        else if (option == "Waffle")
                        {
                            iceCream = new Waffle(option, scoop, fList, tList, waffleflavour);
                            c[i].CurrentOrder.AddIceCream(iceCream);
                        }
                        c[i].OrderHistory.Add(c[i].CurrentOrder);

                        if (c[i].Rewards.Tier == "Gold" || c[i].Rewards.Tier == "Ordinary")
                        {
                            Order ord = c[i].CurrentOrder;

                            //string sq = $"Order ID: {p.Id}\nTime Received: {p.TimeReceived}\nTime Fulfilled: {p.TimeFulfilled}\n\nIce Cream Details:\n";
                            Console.WriteLine($"Order ID: {ord.Id}");
                            Console.WriteLine($"Time Received: {ord.TimeReceived}");
                            Console.WriteLine($"Time Fulfilled: {ord.TimeFulfilled}");
                            Console.WriteLine("\nIce Cream Details:\n");

                            foreach (IceCream ice in ord.IceCreamList)
                            {
                                // sq += $"\tOption: {ice.Option}\n\tScoops: {ice.Scoops}\n";
                                Console.WriteLine($"Option: {ice.Option}");
                                Console.WriteLine($"Scoops: {ice.Scoops}");

                                if (ice is Cone)
                                {
                                    Cone ce = (Cone)ice;
                                    //sq += $"\tDipped: {ce.Dipped}\n";
                                    Console.WriteLine($"Dipped: {ce.Dipped}");
                                }
                                else if (ice is Waffle)
                                {
                                    Waffle w = (Waffle)ice;
                                    //sq += $"\tWaffle Flavour: {w.WaffleFlavour}\n";
                                    Console.WriteLine($"Waffle Flavour: {w.WaffleFlavour}");
                                }
                                //sq += "\tFlavours:\n";
                                Console.WriteLine("Flavours:");
                                foreach (Flavour fla in ice.Flavours)
                                {
                                    //sq += $"\t\t{fla.Type} (Premium: {fla.Premium}, Quantity: {fla.Quantity})\n";
                                    Console.WriteLine($"\t{fla.Type} (Premium: {fla.Premium}, Quantity: {fla.Quantity})");
                                }
                                //sq += "\tToppings:\n";
                                Console.WriteLine("Toppings:");
                                foreach (Topping top in ice.Toppings)
                                {
                                    //sq += $"\t\t{top.Type}\n";
                                    Console.WriteLine($"\t{top.Type}");
                                }
                                //sq += "\n";
                                Console.WriteLine();
                            }

                            Console.WriteLine();
                    }
                }


                    
                }

               
            }
        }
    }

    // Option 3
    static void RegisterNewCustomer(List<Customer> c)
    {

    }

    // Option 4
    static void CreateCustomerOrder(List<Customer> c)
    {

    }

    // Option 5
    // Display order details of a customer
    // List the customers, prompt the user to select a customer, and retrieve order details
    static void DisplayOrderDetailsOfCustomer(List<Customer> c)
    {
        try
        {
            ListAllCustomers(customersList);
            Console.WriteLine("Select a customer by MemberId: ");
            int memId = Convert.ToInt32(Console.ReadLine());
            bool flag=false;
            for (int i = 0; i < c.Count; i++)
            {
                if (c[i].MemberId == memId)
                {
                    flag = true;
                    Console.WriteLine("Current order: ");
                    Order ord = c[i].CurrentOrder;
                    if (ord != null)
                    {
                        Console.WriteLine($"Order ID: {ord.Id}");
                        Console.WriteLine($"Time Received: {ord.TimeReceived}");
                        Console.WriteLine($"Time Fulfilled: {ord.TimeFulfilled}");
                        Console.WriteLine("\nIce Cream Details:\n");

                        foreach (IceCream ice in ord.IceCreamList)
                        {
                            Console.WriteLine($"Option: {ice.Option}");
                            Console.WriteLine($"Scoops: {ice.Scoops}");

                            if (ice is Cone)
                            {
                                Cone ce = (Cone)ice;
                                Console.WriteLine($"Dipped: {ce.Dipped}");
                            }
                            else if (ice is Waffle)
                            {
                                Waffle w = (Waffle)ice;
                                Console.WriteLine($"Waffle Flavour: {w.WaffleFlavour}");
                            }
                            Console.WriteLine("Flavours:");
                            foreach (Flavour fla in ice.Flavours)
                            {
                                Console.WriteLine($"\t{fla.Type} (Premium: {fla.Premium}, Quantity: {fla.Quantity})");
                            }
                            Console.WriteLine("Toppings:");
                            foreach (Topping top in ice.Toppings)
                            {
                                Console.WriteLine($"\t{top.Type}");
                            }
                            Console.WriteLine();
                        }

                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("No current order found.");
                    }
                    Console.WriteLine("Order History: ");
                    foreach (Order history in c[i].OrderHistory)
                    {
                        if (history != null)
                        {
                            if (history.Id == c[i].CurrentOrder.Id)
                            {
                                Console.WriteLine($"{history}");
                            }
                            foreach (IceCream ice in history.IceCreamList)
                            {
                                Console.WriteLine(ice);
                            }
                        }
                       
                    }
                    if (c[i].OrderHistory.Count == 0)
                    {
                        Console.WriteLine("No Order History found.");
                    }
                }
            }
            if (flag == false)
            {
                Console.WriteLine("Customer not found.");
             
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }   
    }
   // Option 6
   // Modify order details
    static void ModifyOrderDetails(List<Customer> c)
    {
        try
        {
            ListAllCustomers(customersList);
            
            Console.WriteLine("en");
            int memid = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < c.Count; i++)
            {
                if (memid == c[i].MemberId)
                {
                    c[i].CurrentOrder.ModifyIceCream(1);
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
                            
                            Console.WriteLine("Flavours 1: ");
                            string flavours1 = Console.ReadLine();
                            Console.WriteLine("Flavours 2: ");
                            string flavours2 = Console.ReadLine();
                            Console.WriteLine("Flavours 3: ");
                            string flavours3 = Console.ReadLine();
                            
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
                            List<Flavour>fList = new List<Flavour>();
                            List<Topping>tList = new List<Topping>();
                            //tempList[i].CurrentOrder.IceCreamList[id].Flavours=flavours;
                            //tempList[i].CurrentOrder.IceCreamList[id].Toppings=toppings;

                        }
                        else if (choice == "2")
                        {
                            Console.WriteLine("Option :");
                            string Option = Console.ReadLine();
                            Console.WriteLine("scoops: ");
                            int scoops = Convert.ToInt32(Console.ReadLine());
                            if (scoops > 3)
                            {
                                Console.WriteLine("Scoops cannot be more than 3.");
                                return;
                            }
                            else if (scoops == 1)
                            {
                                Console.WriteLine("Flavours: ");
                                string flavours = Console.ReadLine();
                            }
                            else if (scoops == 2)
                            {
                                Console.WriteLine("Flavours 1: ");
                                string flavours1 = Console.ReadLine();
                                Console.WriteLine("Flavours 2: ");
                                string flavours2 = Console.ReadLine();
                            }
                            else if (scoops == 3)
                            {
                                Console.WriteLine("Flavours 1: ");
                                string flavours1 = Console.ReadLine();
                                Console.WriteLine("Flavours 2: ");
                                string flavours2 = Console.ReadLine();
                                Console.WriteLine("Flavours 3: ");
                                string flavours3 = Console.ReadLine();
                            }
                         
                           
                            Console.WriteLine("Toppings: ");
                            string toppings = Console.ReadLine();

                            if (Option == "Cone" || Option == "cone")
                            {
                                Console.WriteLine("dipped cone: ");
                                string dipped = Console.ReadLine();
                            }
                            else if (Option == "Waffle" || Option == "waffle")
                            {
                                Console.WriteLine("waffle flavour: ");
                                string wflavour = Console.ReadLine();
                            }
                            //c[i].CurrentOrder.AddIceCream()
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
