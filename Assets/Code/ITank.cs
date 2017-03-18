

using System;

namespace BotArena
{
    public abstract class ITank : IRobot
    {
        public ITank(RobotController parent) : base(parent)
        {
        }
    }
}