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

// Jayden Menu sssssssss
void Menu()
{
    List<Customer> customersList = new List<Customer>();

}
//1)Display order details of a customer
List<Customer>customersList=new List<Customer>();
void Listallcustomers()
{
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string? s = sr.ReadLine();
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
Listallcustomers();

//2) List all current orders
//display the information of all current orders in both the gold members and regular queue
List<Customer> co = customersList.ToList();

void DisplaycOrders()
{
    /// read data from "orders.csv" 
    using (StreamReader sr = new StreamReader("orders.csv"))
    {
        string? s = sr.ReadLine();// read the heading and discard
        while ((s=sr.ReadLine())!= null)
        {
            string[]data = s.Split(',');
           
            int id= Convert.ToInt32(data[0]);
            int memid= Convert.ToInt32(data[1]);
            DateTime tr =Convert.ToDateTime(data[2]);
            DateTime tf = Convert.ToDateTime(data[3]);
            string option = data[4];
            int scoop = Convert.ToInt32(data[5]);
            if (!string.IsNullOrEmpty(f1))
                bool dipped = Convert.ToBoolean(data[6]);
            string waffleflavour = data[7];
            string f1 = data[8];
            string f2 = data[9];
            string f3 = data[10];
            string t1 = data[11];
            string t2 = data[12];
            string t3 = data[13];
            string t4 = data[14];
            List<Flavour>fList= new List<Flavour>();
            List<Topping>tList= new List<Topping>();
            fList.Add(new Flavour(f1,true,1));
            tList.Add(new Topping( t1));
            tList.Add(new Topping(t2));
            tList.Add(new Topping(t3));
            tList.Add(new Topping(t4));
            IceCream c = new Cone();
            IceCream w = new Waffle();
            IceCream cp = new Cup();
            if (option=="cup")
            {
                cp.Scoops = scoop;
                cp.Toppings = tList;
                cp.Flavours = fList;
            }
            Console.WriteLine(cp.ToString);
            for (int i = 0; i < co.Count; i++)
            {
                if (co[i].MemberId == memid)
                {
                    co[i].MakeOrder();
                    co[i].CurrentOrder.Id= id;
                    co[i].CurrentOrder.TimeReceived = tr;
                    co[i].CurrentOrder.TimeFulfilled = tf;
                    //co[i].CurrentOrder.AddIceCream();
                }
            }
        }
        
       
    }
}
DisplaycOrders();
//5)Display order details of a customer
// list the customers
// prompt user to select a customer and retrieve the selected customer
// retrieve all the order objects of the customer, past and current
// for each order, display all the details of the order including datetime received, datetime
//fulfilled (if applicable) and all ice cream details associated with the order
void Display_order_details_of_a_customer()
{
    Listallcustomers();
    Console.WriteLine("select a customer: ");
    int id = Convert.ToInt32( Console.ReadLine());
    for (int i = 0;i<customersList.Count;i++)
    {
       if(customersList[i].MemberId == id)
       {
            
       }
    }
}