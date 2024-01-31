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
using System.Security.Cryptography;
using System.Diagnostics.Metrics;
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
    static Queue<Customer> goldQueue = new Queue<Customer>();
    static Queue<Customer> regularQueue = new Queue<Customer>();
    static Queue<Order> orderQueue = new Queue<Order>();
    static void DisplayMenu()
    {
        Console.WriteLine("---------------- Menu -----------------");
        Console.WriteLine("[1] List All Customers");
        Console.WriteLine("[2] List All Current Orders");
        Console.WriteLine("[3] Register a New Customer");
        Console.WriteLine("[4] Create a Customer's Order");
        Console.WriteLine("[5] Display Order Details of a Customer");
        Console.WriteLine("[6] Modify Order Details");
        Console.WriteLine("[7] Process an Order and Checkout");
        Console.WriteLine("[8] Display monthly charged amounts breakdown & total charged amounts for the year");
        Console.WriteLine("[0] Exit");
        Console.WriteLine("--------------------------------------");
    }

    static void Main()
    {
        try
        {
            InitData(customersList);
            InitOrders(customersList);
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
                else if (choice == "7")
                {
                    ProcessOrderAndCheckout(goldQueue,regularQueue);
                }
                else if (choice == "8")
                {
                    DisplayChargedAmounts(customersList);
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
        Console.WriteLine("--------------------------------------");
    }

    // Option 2
    // List all current orders
    // Display the information of all current orders in both the gold members and regular queue
    static void InitOrders(List<Customer> c)
    {
        using (StreamReader sr = new StreamReader("orders.csv"))
        {
            // Read the header line and discard it
            string? s = sr.ReadLine();
            // Loop through each line in the file
            while ((s = sr.ReadLine()) != null)
            {
                // Split the CSV data into an array
                string[] data = s.Split(',');
                int id = Convert.ToInt32(data[0]);
                int memid = Convert.ToInt32(data[1]);
                DateTime tr = Convert.ToDateTime(data[2]);
                DateTime tf = Convert.ToDateTime(data[3]);
                string option = data[4];
                int scoop = Convert.ToInt32(data[5]);

                bool? dipped = false;
                // Parse 'dipped' if not empty, otherwise default to false
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
                // Create lists for flavors and toppings
                List<string> f = new List<string> { f1, f2, f3 };
                List<string> t = new List<string> { t1, t2, t3, t4 };

                List<Flavour> fList = new List<Flavour>();
                List<Topping> tList = new List<Topping>();
                foreach (string flav in f)
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

                foreach (string top in t)
                {
                    if (!string.IsNullOrEmpty(top))
                    {
                        tList.Add(new Topping(top));
                    }
                }
                IceCream iceCream;
                // Loop through customers to find a matching member ID
                for (int i = 0; i < c.Count; i++)
                {
                    if (c[i].MemberId == memid)
                    {
                        // Create a new order for the customer
                        c[i].MakeOrder();
                        c[i].CurrentOrder.Id = id;
                        c[i].CurrentOrder.TimeReceived = tr;
                        c[i].CurrentOrder.TimeFulfilled = tf;
                        // Create an ice cream based on the option
                        if (option == "Cup")
                        {
                            iceCream = new Cup(option, scoop, fList, tList);
                            c[i].CurrentOrder.AddIceCream(iceCream);
                        }
                        else if (option == "Cone")
                        {
                            iceCream = new Cone(option, scoop, fList, tList, dipped ?? false);
                            c[i].CurrentOrder.AddIceCream(iceCream);
                        }
                        else if (option == "Waffle")
                        {
                            iceCream = new Waffle(option, scoop, fList, tList, waffleflavour);
                            c[i].CurrentOrder.AddIceCream(iceCream);
                        }
                        // Add the current order to the customer's order history
                        c[i].OrderHistory.Add(c[i].CurrentOrder);
                    }
                }

            }
        }
    }
    static void DisplayOrders(List<Customer> c)
    {
       foreach(Customer customer in c)
       {
            if (customer.Rewards.Tier == "Gold" || customer.Rewards.Tier == "Ordinary")
            {
                Order ord =customer.CurrentOrder;
                if (ord != null)
                {
                    //string sq = $"Order ID: {p.Id}\nTime Received: {p.TimeReceived}\nTime Fulfilled: {p.TimeFulfilled}\n\nIce Cream Details:\n";
                    Console.WriteLine("---------------- Order Details -----------------");
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
                        Console.WriteLine("");
                    }

                    //Console.WriteLine();
                }
            }


        }







    }

    // Option 3
    static void RegisterNewCustomer(List<Customer> c)
    {
        try
        {
            Console.WriteLine("Enter name: ");
            string name = Console.ReadLine();

            Console.WriteLine("Enter id: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter date of birth (mm//dd/yyyy): ");
            DateTime dob = Convert.ToDateTime(Console.ReadLine());

            Customer nc = new Customer(name, id, dob);
            
            using (StreamWriter sw = new StreamWriter("customer.csv", true))
            {
                sw.WriteLine($"{name},{id}, {dob.ToString("MM/dd/yyyy")}, {nc.Rewards.Tier}, {nc.Rewards.Points}, {nc.Rewards.PunchCard}");
            }

            customersList.Add(nc);

            Console.WriteLine("Registration Successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: {e.Message}");
        }
    }

    //// Option 4
    static void CreateCustomerOrder(List<Customer> customersList)
    {
        try
        {
            // List all customers
            ListAllCustomers(customersList);

            Console.WriteLine("Select a customer (enter MemberID): ");
            int memId = Convert.ToInt32(Console.ReadLine());

            // Retrieve the selected customer
            Customer selectedCustomer = customersList.FirstOrDefault(customer => customer.MemberId == memId);

            if (selectedCustomer != null)
            {
                // Create an order object
                Order newOrder = new Order();

                do
                {
                    Console.WriteLine("Enter ice cream order details:");
                    Console.Write("Option (cup/cone/waffle): ");
                    string option = Console.ReadLine();

                    Console.Write("Scoops (1-3): ");
                    int scoops = Convert.ToInt32(Console.ReadLine());

                    if (scoops < 1 || scoops > 3)
                    {
                        Console.WriteLine("Scoops must be between 1 and 3. Please try again.");
                        continue;
                    }

                    for (int i = 0; i < scoops; i++)
                    {
                        Console.WriteLine($"Enter details for Scoop #{i + 1}");
                        IceCream iceCream = CreateIceCream(option, scoops);
                        newOrder.AddIceCream(iceCream);
                    }

                    Console.Write("Do you want to add another ice cream to the order? (Y/N): ");
                } while (Console.ReadLine().ToUpper() == "Y");

                // Set Id to currentOrder.Id + 1
                int newOrderId = selectedCustomer.CurrentOrder.Id + 1;
                newOrder.Id = newOrderId;

                // Set TimeReceived to the current time
                newOrder.TimeReceived = DateTime.Now;

                // Set TimeFulfilled to the current time
                newOrder.TimeFulfilled = DateTime.Now;

                // Add the order to the customer's order history
                selectedCustomer.OrderHistory.Add(newOrder);

                // Enqueue the customer based on their tier
                if (selectedCustomer.Rewards.Tier == "Gold")
                {
                    // Append the order to the back of the goldQueue
                    selectedCustomer.CurrentOrder.TimeReceived = DateTime.Now;
                    goldQueue.Enqueue(selectedCustomer);
                    // Remove the customer from the regularQueue if they were added previously
                    regularQueue = new Queue<Customer>(regularQueue.Where(cust => cust.MemberId != selectedCustomer.MemberId));
                }
                else
                {
                    // Append the order to the back of the regularQueue
                    selectedCustomer.CurrentOrder.TimeReceived = DateTime.Now;
                    regularQueue.Enqueue(selectedCustomer);
                    // Remove the customer from the goldQueue if they were added previously
                    goldQueue = new Queue<Customer>(goldQueue.Where(cust => cust.MemberId != selectedCustomer.MemberId));
                }


                Console.WriteLine("Order has been made successfully!");

                // Display customers in goldQueue
                Console.WriteLine("Gold Queue:");
                foreach (Customer c in goldQueue)
                {
                    Console.WriteLine(c);
                }

                //Display customers in regularQueue
                Console.WriteLine("Regular Queue:");
                foreach (Customer c in regularQueue)
                {
                    Console.WriteLine(c);
                }
            }
            else
            {
                Console.WriteLine("Customer not found!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while creating customer order: {e.Message}");
        }
    }

    static IceCream CreateIceCream(string option, int scoops)
    {
        Console.WriteLine("Available Flavours:");
        Console.WriteLine("1. Vanilla (Regular)");
        Console.WriteLine("2. Chocolate (Regular)");
        Console.WriteLine("3. Strawberry (Regular)");
        Console.WriteLine("4. Durian (Premium)");
        Console.WriteLine("5. Ube (Premium)");
        Console.WriteLine("6. Sea salt (Premium)");

        Console.Write($"Enter the flavour (1-6): ");
        int flavorChoice = Convert.ToInt32(Console.ReadLine());

        // Create a list to hold the selected flavour
        List<Flavour> selectedFlavour = new List<Flavour>();
        selectedFlavour.Add(GetFlavour(flavorChoice));

        Console.WriteLine("Available Toppings:");
        Console.WriteLine("1. Sprinkles");
        Console.WriteLine("2. Mochi");
        Console.WriteLine("3. Sago");
        Console.WriteLine("4. Oreos");

        Console.Write("Enter the topping (1-4): ");
        int toppingChoice = Convert.ToInt32(Console.ReadLine());

        // Create a list to hold the selected topping
        List<Topping> selectedTopping = new List<Topping>();
        selectedTopping.Add(GetTopping(toppingChoice));

        return new Cup(option, scoops, selectedFlavour, selectedTopping);
    }


    static Flavour GetFlavour(int flavorChoice)
    {
        switch (flavorChoice)
        {
            case 1:
                return new Flavour("Vanilla", false);
            case 2:
                return new Flavour("Chocolate", false);
            case 3:
                return new Flavour("Strawberry", false);
            case 4:
                return new Flavour("Durian", true);
            case 5:
                return new Flavour("Ube", true);
            case 6:
                return new Flavour("Sea salt", true);
            default:
                Console.WriteLine("Invalid flavor choice. Defaulting to Vanilla.");
                return new Flavour("Vanilla", false);
        }
    }

    static Topping GetTopping(int toppingChoice)
    {
        switch (toppingChoice)
        {
            case 1:
                return new Topping("Sprinkles");
            case 2:
                return new Topping("Mochi");
            case 3:
                return new Topping("Sago");
            case 4:
                return new Topping("Oreos");
            default:
                Console.WriteLine("Invalid topping choice. Defaulting to Sprinkles.");
                return new Topping("Sprinkles");
        }
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
                        Console.WriteLine("---------------- Current Order -----------------");
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
                            //Console.WriteLine("");
                        }

                        //Console.WriteLine("");
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
                            Console.WriteLine("---------------- Order History -----------------");
                            Console.WriteLine($"Order ID: {history.Id}");
                            Console.WriteLine($"Time Received: {history.TimeReceived}");
                            Console.WriteLine($"Time Fulfilled: {history.TimeFulfilled}");
                            Console.WriteLine("\nIce Cream Details:\n");

                            foreach (IceCream ices in history.IceCreamList)
                            {
                                Console.WriteLine($"Option: {ices.Option}");
                                Console.WriteLine($"Scoops: {ices.Scoops}");
                                if (ices is Cone)
                                {
                                    Cone cn = (Cone)ices;
                                    Console.WriteLine($"Dipped: {cn.Dipped}");
                                }
                                else if (ices is Waffle)
                                {
                                    Waffle wf = (Waffle)ices;
                                    Console.WriteLine($"Waffle Flavour: {wf.WaffleFlavour}");
                                }
                                Console.WriteLine("Flavours:");
                                foreach (Flavour flav in ices.Flavours)
                                {
                                    Console.WriteLine($"\t{flav.Type} (Premium: {flav.Premium}, Quantity: {flav.Quantity})");
                                }
                                Console.WriteLine("Toppings:");
                                foreach (Topping topp in ices.Toppings)
                                {
                                    Console.WriteLine($"\t{topp.Type}");
                                }
                                Console.WriteLine("");
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
            Console.WriteLine("Enter Customer MemberId: ");
            int memid = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < c.Count; i++)
            {
                if (memid == c[i].MemberId)
                {
                    int counter = 0;
                    if (c[i].CurrentOrder != null)
                    {
                        foreach (IceCream ice in c[i].CurrentOrder.IceCreamList)
                        {
                            counter += 1;
                            Console.WriteLine("---------------- Current Order -----------------");
                            Console.WriteLine($"IceCream {counter}:");
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
                            // Console.WriteLine();
                        }

                        
                    }
                    else if (c[i].CurrentOrder == null)
                    {
                        Console.WriteLine($"ID:{memid} has no current order.");
                    }
                    try
                    {
                        if (c[i].CurrentOrder != null)
                        {
                            c[i].CurrentOrder.ModifyIceCream(memid);

                            string choice = Console.ReadLine();
                            if (choice == "1")
                            {
                                Console.WriteLine("which ice cream to modify [Enter the Index] Enter 'X' to end:");
                                int id = Convert.ToInt32(Console.ReadLine());
                                if (id-1 == -1)
                                {
                                    break;
                                }
                                if (id - 1 >= 0 && id - 1 < c[i].CurrentOrder.IceCreamList.Count)
                                {
                                    IceCream iceCream = c[i].CurrentOrder.IceCreamList[id - 1];
                                    Console.WriteLine("[1]Option [Cup, Cone, Waffle]");
                                    Console.WriteLine("[2]Scoops [1-3]");
                                    Console.WriteLine("[3]Flavours [Vanilla, Chocolate, Strawberry, Durian, Ube, Sea salt]");
                                    Console.WriteLine("[4]Toppings ['Sprinkles', 'Mochi', 'Sago' , 'Oreos']");
                                    Console.WriteLine("[5]Dipped Cone [Yes/No]");
                                    Console.WriteLine("[6]Waffle Flavour ['Red Velvet', 'Charcoal', 'Pandan']");
                                    Console.WriteLine("Enter 'X' to end");
                                    Console.WriteLine("Enter your Selection: ");
                                    string Selected = Console.ReadLine();
                                    if (Selected.ToUpper() == "X")
                                    {
                                        break;
                                    }
                                    switch (Selected = char.ToUpper(Selected[0]) + Selected.Substring(1))
                                    {
                                        case "1":
                                            Console.WriteLine("Enter Your Option [Cup, Cone, Waffle]");
                                            string Option = Console.ReadLine();
                                            switch (Option = char.ToUpper(Option[0]) + Option.Substring(1))
                                            {
                                                case "Cup":
                                                    if (iceCream is Cup)
                                                    {
                                                        Cup cup = (Cup)iceCream;
                                                        cup.Option = Selected;
                                                    }

                                                    break;
                                                case "Cone":
                                                    Console.WriteLine("Dipped Cone [Yes/No]: ");
                                                    string dipped = Console.ReadLine();
                                                    if (dipped == "Yes")
                                                    {
                                                        if (iceCream is Cone)
                                                        {
                                                            Cone cone = (Cone)iceCream;
                                                            cone.Option = Selected;
                                                            cone.Dipped = true;
                                                        }
                                                    }
                                                    else if (dipped != "No")
                                                    {
                                                        Console.WriteLine("Invalid input. Please enter ['Yes' or 'No'].");

                                                    }
                                                    break;
                                                case "Waffle":
                                                    Console.WriteLine("Do you want to add a Waffle Flavor? [Yes/No]: ");
                                                    string yes = Console.ReadLine();
                                                    if (yes == "Yes")
                                                    {
                                                        Console.WriteLine("Waffle Flavour ['Red Velvet', 'Charcoal', or 'Pandan']: ");
                                                        string wflavour = Console.ReadLine();
                                                        switch (wflavour)
                                                        {
                                                            case "Red Velvet":
                                                            case "Charcoal":
                                                            case "Pandan":
                                                                if (iceCream is Waffle)
                                                                {
                                                                    Waffle waffle = (Waffle)iceCream;
                                                                    waffle.Option = Selected;
                                                                    waffle.WaffleFlavour = wflavour;
                                                                }
                                                                break;
                                                            default:
                                                                Console.WriteLine("Invalid Waffle Flavour. Please choose either ['Red Velvet', 'Charcoal', or 'Pandan']");
                                                                continue;
                                                                //break;
                                                        }

                                                    }
                                                    else if (yes != "No")
                                                    {
                                                        Console.WriteLine("Invalid input. Please enter ['Yes' or 'No'].");
                                                        continue;
                                                    }
                                                    break;
                                                default:
                                                    Console.WriteLine("Invalid Option. Please choose either ['Cup', 'Cone', or 'Waffle'].");
                                                    continue;
                                                    break;
                                            }
                                            break;

                                        case "2":
                                            Console.WriteLine("Enter the number of scoops: ");
                                            int scoops = Convert.ToInt32(Console.ReadLine());
                                            if (scoops > 3 && scoops > 0)
                                            {
                                                Console.WriteLine("Scoops cannot be more than 3.");
                                                continue;

                                            }
                                            else
                                            {
                                                c[i].CurrentOrder.IceCreamList[id - 1].Scoops = scoops;
                                                for (int s = 0; s < scoops; s++)
                                                {
                                                    Console.WriteLine($"Flavours {s + 1} [Vanilla, Chocolate, Strawberry, Durian, Ube, Sea salt]: ");
                                                    string flavours = Console.ReadLine();
                                                    switch (flavours = char.ToUpper(flavours[0]) + flavours.Substring(1))
                                                    {
                                                        case "Durian":

                                                        case "Ube":


                                                        case "Sea salt":
                                                            c[i].CurrentOrder.IceCreamList[id - 1].Flavours.Add(new Flavour(flavours, true));
                                                            break;
                                                        default:
                                                            if (flavours == "Vanilla" || flavours == "Chocolate" || flavours == "Strawberry")
                                                            {
                                                                c[i].CurrentOrder.IceCreamList[id - 1].Flavours.Add(new Flavour(flavours, false));
                                                            }
                                                            else if (flavours != "Vanilla" || flavours != "Chocolate" || flavours != "Strawberry")
                                                            {
                                                                Console.WriteLine("Invalid flavour. choose either [Vanilla, Chocolate, Strawberry, Durian, Ube, Sea salt].");
                                                                s--; // Re-ask for the current scoop
                                                            }
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                        case "3":

                                            for (int f = 0; f < c[i].CurrentOrder.IceCreamList[id - 1].Scoops; f++)
                                            {

                                                Console.WriteLine($"Flavours {f + 1} [Vanilla, Chocolate, Strawberry, Durian, Ube, Sea salt]: ");
                                                string flavours = Console.ReadLine();
                                                switch (flavours = char.ToUpper(flavours[0]) + flavours.Substring(1))
                                                {
                                                    case "Durian":

                                                    case "Ube":


                                                    case "Sea salt":
                                                        c[i].CurrentOrder.IceCreamList[id - 1].Flavours.Add(new Flavour(flavours, true));
                                                        break;
                                                    default:
                                                        if (flavours == "Vanilla" || flavours == "Chocolate" || flavours == "Strawberry")
                                                        {
                                                            c[i].CurrentOrder.IceCreamList[id - 1].Flavours.Add(new Flavour(flavours, false));
                                                        }
                                                        else if (flavours != "Vanilla" || flavours != "Chocolate" || flavours != "Strawberry")
                                                        {
                                                            Console.WriteLine("Invalid flavour. choose either [Vanilla, Chocolate, Strawberry, Durian, Ube, Sea salt].");
                                                            f--; // Re-ask for the current scoop
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "4":
                                            for (int t = 0; t < 4; t++)
                                            {
                                                Console.WriteLine($"Toppings {t + 1} ['Sprinkles', 'Mochi', 'Sago' , 'Oreos'] Enter 'X' to end: ");
                                                string toppings = Console.ReadLine();
                                                if (toppings.ToUpper() == "x")
                                                {
                                                    break;
                                                }
                                                switch (toppings = char.ToUpper(toppings[0]) + toppings.Substring(1))
                                                {
                                                    case "Sprinkles":
                                                    case "Mochi":
                                                    case "Sago":
                                                    case "Oreos":
                                                        c[i].CurrentOrder.IceCreamList[id - 1].Toppings.Add(new Topping(toppings));
                                                        break;

                                                    default:

                                                        Console.WriteLine("Invalid Topping. Please choose either ['Sprinkles', 'Mochi', 'Sago' , or 'Oreos'].");
                                                        t--;
                                                        break;
                                                }
                                            }


                                            break;
                                        case "5":
                                            if (!(c[i].CurrentOrder.IceCreamList[id - 1] is Cone))
                                            {
                                                Console.WriteLine("Error: The selected ice cream option is not a cone.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Dipped Cone [Yes/No]: ");
                                                string dipped = Console.ReadLine();
                                                if (dipped == "Yes")
                                                {
                                                    if (iceCream is Cone)
                                                    {
                                                        Cone cone = (Cone)iceCream;
                                                        cone.Option = Selected;
                                                        cone.Dipped = true;
                                                    }
                                                }
                                                else if (dipped != "No")
                                                {
                                                    Console.WriteLine("Invalid input. Please enter ['Yes' or 'No'].");

                                                }
                                            }
                                            break;
                                        case "6":
                                            if (!(c[i].CurrentOrder.IceCreamList[id - 1] is Waffle))
                                            {
                                                Console.WriteLine("Error: The selected ice cream option is not a Waffle.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Do you want to add a Waffle Flavor? [Yes/No]: ");
                                                string yes = Console.ReadLine();
                                                if (yes == "Yes")
                                                {
                                                    Console.WriteLine("Waffle Flavour ['Red Velvet', 'Charcoal', or 'Pandan']: ");
                                                    string wflavour = Console.ReadLine();
                                                    switch (wflavour)
                                                    {
                                                        case "Red Velvet":
                                                        case "Charcoal":
                                                        case "Pandan":
                                                            if (iceCream is Waffle)
                                                            {
                                                                Waffle waffle = (Waffle)iceCream;
                                                                waffle.Option = Selected;
                                                                waffle.WaffleFlavour = wflavour;
                                                            }
                                                            break;
                                                        default:
                                                            Console.WriteLine("Invalid Waffle Flavour. Please choose either ['Red Velvet', 'Charcoal', or 'Pandan']");
                                                            continue;
                                                            //break;
                                                    }

                                                }
                                                else if (yes != "No")
                                                {
                                                    Console.WriteLine("Invalid input. Please enter ['Yes' or 'No'].");
                                                    continue;
                                                }
                                            }
                                            break;
                                        default:
                                            Console.WriteLine("Invalid input. Please enter a valid option [1, 2, 3, 4, 5, 6].");
                                            continue;


                                    }




                                }
                                else
                                {
                                    Console.WriteLine("Invalid index. Please enter a valid index.");
                                    continue;
                                }

                            }
                            else if (choice == "2")
                            {
                                List<Flavour> fList = new List<Flavour>();
                                List<Topping> tList = new List<Topping>();
                                while (true)
                                {
                                    Console.WriteLine("Option [Cup, Cone, Waffle]: ");
                                    string option = Console.ReadLine();
                                    Console.WriteLine("Scoops [1-3]: ");
                                    int scoops = Convert.ToInt32(Console.ReadLine());

                                    if (scoops > 3 && scoops>0)
                                    {
                                        Console.WriteLine("Scoops cannot be more than 3.");
                                        return;
                                    }
                                    else
                                    {
                                        for (int j = 0; j < scoops; j++)
                                        {
                                            Console.WriteLine("Flavours [Vanilla, Chocolate, Strawberry, Durian, Ube, Sea salt]: ");
                                            string flavourInput = Console.ReadLine();
                                            switch (flavourInput)
                                            {
                                                case "Durian":

                                                case "Ube":

                                                case "Sea salt":
                                                    fList.Add(new Flavour(flavourInput, true));
                                                    break;
                                                default:
                                                    if (flavourInput != "Vanilla" || flavourInput != "Chocolate" || flavourInput != "Strawberry")
                                                    {
                                                        fList.Add(new Flavour(flavourInput, false));
                                                    }
                                                    break;
                                            }
                                        }
                                        for (int k = 0; k < 4; k++)
                                        {
                                            Console.WriteLine("Toppings ['Sprinkles', 'Mochi', 'Sago', 'Oreos'] Enter 'x' to end: ");
                                            string toppingInput = Console.ReadLine();
                                            if (toppingInput.ToLower() == "x")
                                            {
                                                break;
                                            }
                                            switch (toppingInput)
                                            {
                                                case "Sprinkles":
                                                case "Mochi":
                                                case "Sago":
                                                case "Oreos":
                                                    tList.Add(new Topping(toppingInput));
                                                    break;
                                                default:
                                                    Console.WriteLine("Invalid topping. Please choose from the provided options.");
                                                    break;
                                            }
                                        }
                                    }
                                    IceCream iceCream;

                                    if (option == "Cone")
                                    {
                                        Console.Write("Dipped Cone? (Yes/No): ");
                                        string dipped = Console.ReadLine().ToLower(); // Convert to lowercase for case-insensitivity
                                        if (dipped == "yes")
                                        {
                                            iceCream = new Cone(option, scoops, fList, tList, true);
                                            c[i].CurrentOrder.AddIceCream(iceCream);
                                        }
                                        else if (dipped != "no")
                                        {
                                            Console.WriteLine("Invalid input. Please enter 'Yes' or 'No'.");
                                            // Handle the error, you might want to ask the question again or take appropriate action.
                                        }
                                        else
                                        {
                                            iceCream = new Cone(option, scoops, fList, tList);
                                            c[i].CurrentOrder.AddIceCream(iceCream);
                                        }
                                    }
                                    else if (option == "Waffle")
                                    {
                                        Console.Write("Do you want to add a Waffle Flavor? (Yes/No): ");
                                        string yes = Console.ReadLine().ToLower();
                                        if (yes == "yes")
                                        {
                                            Console.Write("Waffle Flavour ['Red Velvet', 'Charcoal', or 'Pandan']: ");
                                            string waffleFlavour = Console.ReadLine();
                                            switch (waffleFlavour.ToLower())
                                            {
                                                case "red velvet":
                                                case "charcoal":
                                                case "pandan":
                                                  iceCream = new Waffle(option, scoops, fList, tList, waffleFlavour);
                                                    c[i].CurrentOrder.AddIceCream(iceCream);
                                                    break;
                                                default:
                                                    Console.WriteLine("Invalid Waffle Flavour. Please choose either 'Red Velvet', 'Charcoal', or 'Pandan'.");
                                                    // Handle the error, you might want to ask the question again or take appropriate action.
                                                    continue;
                                            }
                                        }
                                        else if (yes != "no")
                                        {
                                            Console.WriteLine("Invalid input. Please enter 'Yes' or 'No'.");
                                            // Handle the error, you might want to ask the question again or take appropriate action.
                                            continue;
                                        }
                                        else
                                        {
                                            iceCream = new Waffle(option, scoops, fList, tList);
                                            c[i].CurrentOrder.AddIceCream(iceCream);
                                        }
                                    }
                                    else
                                    {
                                        iceCream = new Cup(option, scoops, fList, tList);
                                        c[i].CurrentOrder.AddIceCream(iceCream);
                                    }
                                   
                                   Console.Write("Would you like to add another ice cream to the order? [Yes/No]: ");
                                    string anotherIceCream= Console.ReadLine();
                                    if (anotherIceCream == "No")
                                    {
                                        break;
                                    }
                                    
                                }
                            }
                            else if (choice == "3")
                            {
                                if (c[i].CurrentOrder.IceCreamList.Count != 0)
                                {
                                    Console.WriteLine("which ice cream to delete");
                                    int id = Convert.ToInt32(Console.ReadLine());
                                    c[i].CurrentOrder.DeleteIceCream(id - 1);
                                    
                                    
                                }
                                else
                                {
                                    Console.WriteLine("cannot have zero ice creams in an order");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid Choice. Please enter a valid option by choosing from the provided menu [1, 2, 3]");
                                continue;
                            }

                            int counters = 0;
                            foreach (IceCream ice in c[i].CurrentOrder.IceCreamList)
                            {
                                counters += 1;
                                Console.WriteLine("---------------- New Updated Order -----------------");
                                Console.WriteLine($"IceCream {counters}:");
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
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }



    //ADVANCED FEATURES
    // [a]
    // Process an order and checkout
    static void ProcessOrderAndCheckout(Queue<Customer> orderQueue, Queue<Customer> customersList)
    {
        if (orderQueue.Count > 0)
        {
            // Dequeue the first order in the queue
            Customer currentOrder = orderQueue.Dequeue();

            // Display all ice creams in the order
            Console.WriteLine("Ice Creams in the Order:");
            foreach (IceCream iceCream in currentOrder.CurrentOrder.IceCreamList)
            {
                Console.WriteLine(iceCream);
            }

            // Display the total bill amount
            double totalBill = currentOrder.CurrentOrder.CalculateTotal();
            Console.WriteLine($"Total Bill Amount: {totalBill:C}");

            // Get the customer associated with the order
            Customer customer = customersList.FirstOrDefault(c => c.CurrentOrder == currentOrder.CurrentOrder);

            if (customer != null)
            {
                // Display the membership status & points of the customer
                Console.WriteLine($"Membership Status: {customer.Rewards.Tier}");
                Console.WriteLine($"Points: {customer.Rewards.Points}");

                // Check if it is the customer’s birthday and calculate the final bill
                if (customer.IsBirthday())
                {
                    IceCream mostExpensiveIceCream = currentOrder.CurrentOrder.IceCreamList.OrderByDescending(ic => ic.CalculatePrice()).FirstOrDefault();
                    if (mostExpensiveIceCream != null)
                    {
                        totalBill -= mostExpensiveIceCream.CalculatePrice();
                        Console.WriteLine($"Birthday Discount Applied: Most Expensive Ice Cream is Free");
                    }
                }

                // Check if the customer has completed their punch card and calculate the final bill
                if (customer.Rewards.PunchCard >= 10)
                {
                    if (currentOrder.CurrentOrder.IceCreamList.Count > 0)
                    {
                        totalBill -= currentOrder.CurrentOrder.IceCreamList.First().CalculatePrice();
                        customer.Rewards.PunchCard = 0;
                        Console.WriteLine($"Punch Card Reward Applied: First Ice Cream is Free, Punch Card Reset");
                    }
                }

                // Check Pointcard status to determine if the customer can redeem points
                if (customer.Rewards.Tier == "Gold" || customer.Rewards.Tier == "Silver")
                {
                    Console.Write("Enter the number of points you want to use to offset the final bill: ");
                    int pointsToUse = Convert.ToInt32(Console.ReadLine());
                    if (pointsToUse > 0 && pointsToUse <= customer.Rewards.Points)
                    {
                        // Redeem points if necessary
                        customer.Rewards.RedeemPoints(pointsToUse);
                        Console.WriteLine($"Points Redeemed: {pointsToUse}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid points entered or not enough points. Points redemption skipped.");
                    }
                }

                // Display the final total bill amount
                Console.WriteLine($"Final Total Bill Amount: {totalBill:C}");

                // Prompt user to press any key to make payment
                Console.WriteLine("Press any key to make payment...");

                // Increment the punch card for every ice cream in the order (if it goes above 10, set it back down to 10)
                foreach (IceCream iceCream in currentOrder.CurrentOrder.IceCreamList)
                {
                    customer.Rewards.PunchCard++;
                    if (customer.Rewards.PunchCard > 10)
                    {
                        customer.Rewards.PunchCard = 10;
                    }
                }

                // Earn points and upgrade the member status accordingly
                customer.Rewards.AddPoints(Convert.ToInt32(totalBill));

                // Mark the order as fulfilled with the current datetime
                currentOrder.CurrentOrder.TimeFulfilled = DateTime.Now;

                // Add the fulfilled order object to the customer's order history
                customer.OrderHistory.Add(currentOrder.CurrentOrder);
            }
        }
        else
        {
            Console.WriteLine("No orders in the queue to process.");
        }
    }



    //(b)Display monthly charged amounts breakdown & total charged amounts for the year
    // prompt the user for the year
    // retrieve all order objects that were successfully fulfilled within the inputted year
    // compute and display the monthly charged amounts breakdown & the total charged
    //amounts for the input year
    static void DisplayChargedAmounts(List<Customer> c)
    {
        try
        {
            Console.Write("Enter the year: ");
            int year = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("");
            Console.WriteLine("");
            double MonthlyAmount = 0;
            double TotalAmount = 0;
            bool flag=false;
            Dictionary<string,double> Charged = new Dictionary<string,double>();
            foreach (Customer customer in c)
            {
                if (customer.OrderHistory != null )
                {
                    foreach (Order HistoryOrder in customer.OrderHistory.OrderBy(HistoryOrder => HistoryOrder.TimeFulfilled))
                    {
                        if (HistoryOrder.TimeFulfilled?.Year == year)
                        {
                            flag = true;
                            double Amount = HistoryOrder.CalculateTotal();
                            MonthlyAmount += Amount;
                            string MonthYear = $"{HistoryOrder.TimeFulfilled?.ToString("MMM yyyy")}";
                            if (Charged.ContainsKey(MonthYear))
                            {
                                Charged[MonthYear]=MonthlyAmount;
                            }
                            else
                            {
                                Charged.Add(MonthYear, MonthlyAmount);
                            }
                           

                        }
                       

                    }
                }
            }
            if (flag==true)
            {
                foreach (KeyValuePair<string, double> a in Charged)
                {
                    Console.WriteLine($"{a.Key}: {a.Value:c2}");
                    TotalAmount += a.Value;
                }
                Console.WriteLine("");
                
                Console.WriteLine($"Total: {TotalAmount,9:c2}");
            }
            else 
            { 
                Console.WriteLine($"No orders found for the year {year}.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

































}





