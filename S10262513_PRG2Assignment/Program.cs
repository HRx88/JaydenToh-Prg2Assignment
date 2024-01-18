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
Queue<Customer>orders = new Queue<Customer>();
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
            bool dipped = Convert.ToBoolean(data[6]);
            string waffleflavour = data[7];
            string f1 = data[8];
            string f2 = data[9];
            string f3 = data[10];
            string t1 = data[11];
            string t2 = data[12];
            string t3 = data[13];
            string t4 = data[14];







        }
    }
}
