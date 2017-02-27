

using System;

namespace BotArena
{
    public abstract class ITank : IRobot
    {

        public readonly new float AGILITY = 1;
        public new float energy = 50;

        public ITank(RobotController parent) : base(parent)
        {
        }
    }
}