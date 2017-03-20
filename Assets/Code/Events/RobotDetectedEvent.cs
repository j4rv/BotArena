using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotArena
{
    public class RobotDetectedEvent : Event
    {
        public RobotInfo robotInfo;

        public RobotDetectedEvent(RobotInfo robotInfo)
        {
            this.robotInfo = robotInfo;
        }
    }
}
