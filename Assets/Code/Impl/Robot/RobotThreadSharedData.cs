using System.Collections.Generic;
using System.Linq;

namespace BotArena
{
    class RobotThreadSharedData
    {
        internal List<Order> orders;
        internal List<IEvent> events;

        public RobotThreadSharedData() {
            orders = new List<Order>();
            events = new List<IEvent>();
        }

        public Order GetLastOrder() {
            //LastOrDefault because Last would throw an exception on the first turn.
            return orders.LastOrDefault();
        }
    }
}
