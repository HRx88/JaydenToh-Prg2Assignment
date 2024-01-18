using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262513_PRG2Assignment
{
    //==========================================================
    // Student Number : S10262513
    // Student Name : Tan Hong Rong
    // Partner Name : Jayden Toh Xuan Ming
    //==========================================================
    class Customer
    {

        public string Name { get; set; }

        public int MemberId { get; set; }

        public DateTime Dob { get; set; }

        public Order CurrentOrder { get; set; }

        public List<Order> OrderHistory { get; set; } = new List<Order>();

        public PointCard Rewards { get; set; }

        public Customer()
        {

        }

        public Customer(string n, int m, DateTime d)
        {
            Name = n;
            MemberId = m;
            Dob = d;
            CurrentOrder = new Order();
            Rewards = new PointCard();


        }

        public Order MakeOrder()
        {
            CurrentOrder.IceCreamList = new List<IceCream>();
            
            Rewards.Punch();
            return CurrentOrder;
        }

        public bool IsBirthday()
        {
            if (DateTime.Today.Month == Dob.Month && DateTime.Today.Day == Dob.Day)
            {
                return true;
            }
            else
            {
                return false;
            }
          
        }

        public override string ToString()
        {
            return "Name: "+Name+"\tMemberId: "+MemberId+"\tDOB: "+Dob+ "\tCurrentOrder: "+CurrentOrder+ "\tRewards: "+Rewards;
        }
    }
}
