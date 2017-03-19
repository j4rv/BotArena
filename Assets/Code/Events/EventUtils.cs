using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotArena
{
    public class EventUtils
    {
        public static void HandleEvent(Event e, Order order, IRobot robot)
        {
            if(e is RobotDetectedEvent)
            {
                RobotDetectedEvent robotDetected = (RobotDetectedEvent) e;
                robot.OnEnemyDetected(order, robotDetected.enemyInfo);
            }
        }
    }
}
