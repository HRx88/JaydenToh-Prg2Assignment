using Microsoft.VisualBasic;
using S10262513_PRG2Assignment;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;
using System.IO;
//==========================================================
// Student Number : S10262513
// Student Name : Tan Hong Rong
// Partner Name : Jayden Toh Xuan Ming
//==========================================================

//2) List all current orders
//display the information of all current orders in both the gold members and regular queue
Queue<Order>orders = new Queue<Order>();
void DisplaycOrders()
{
    /// read data from "orders.csv" 
    using (StreamReader sr = new StreamReader("orders.csv"))
    {
        string? s = sr.ReadLine();// read the heading and discard
        while ((s=sr.ReadLine())!= null)
        {
            string[]data = s.Split(',');
            
            
        }
    }
}
