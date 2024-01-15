using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262513_PRG2Assignment
{
    class Order
    {
        public int Id { get; set; }

        public DateTime TimeReceived { get; set; }

        public DateTime ? TimeFulfilled { get; set; }

        public List<IceCream> IceCreamList { get; set; }= new List<IceCream>();

        public Order()
        {
            
        }

        public Order(int id,DateTime tr)
        {
            Id = id;
            TimeReceived = tr;
            TimeFulfilled = DateTime.MinValue;
            
        }
        public void ModifyIceCream(int id)
        {
            
        }

        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Remove(iceCream);
        }
        public void DeleteIceCream(int id)
        {
            
        }
        public double CalculateTotal() { return 0; }

        public override string ToString() 
        {
            return "Id: "+Id+ "\tTimeReceived: "+TimeReceived+ "\tTimeFulfilled: "+TimeFulfilled;
        }
    }
}
