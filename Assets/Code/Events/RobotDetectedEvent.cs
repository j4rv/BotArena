using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotArena
{
    public class RobotDetectedEvent : Event
    {
        public RobotInfo enemyInfo;

        public RobotDetectedEvent(RobotInfo enemyInfo)
        {
            this.enemyInfo = enemyInfo;
        }
    }
}
