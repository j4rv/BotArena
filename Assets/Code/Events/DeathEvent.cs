using System;
using UnityEngine;

namespace BotArena
{
    public class DeathEvent : Event
    {
        private RobotController deadRobot;

        public RobotController DeadRobot
        {
            get { return deadRobot; }
            private set { deadRobot = value; }
        }
        
        public DeathEvent(RobotController deadRobot)
        {
            this.deadRobot = deadRobot;
        }
    }
}
