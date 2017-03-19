using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotArena
{
    public class RobotThreadShadedData
    {
        public List<Order> orders;
        public List<Event> events;

        public RobotThreadShadedData()
        {
            orders = new List<Order>();
            events = new List<Event>();
        }

        public Order GetLastOrder()
        { 
            //LastOrDefault because Last will throw an exception on the first turn.
            return orders.LastOrDefault(); 
        }
    }
}
