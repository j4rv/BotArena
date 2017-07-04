using System.Collections.Generic;
using System.Linq;

namespace BotArena
{
    public class RobotThreadSharedData
    {
        public List<Order> orders;
        public List<Event> events;

        public RobotThreadSharedData()
        {
            orders = new List<Order>();
            events = new List<Event>();
        }

        public Order GetLastOrder()
        { 
            //LastOrDefault because Last would throw an exception on the first turn.
            return orders.LastOrDefault(); 
        }
    }
}
