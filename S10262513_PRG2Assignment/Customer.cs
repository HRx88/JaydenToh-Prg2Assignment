using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262513_PRG2Assignment
{
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
            return CurrentOrder;
        }

        public bool IsBirthday()
        {
            return false;
        }

        public override string ToString()
        {
            return "Name: "+Name+"\tMemberId: "+MemberId+"\tDOB: "+Dob+ "\tCurrentOrder: "+CurrentOrder+ "\tRewards: "+Rewards;
        }
    }
}
