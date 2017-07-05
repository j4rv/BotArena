namespace BotArena
{
    public class DeathEvent : Event
    {
        private IRobot deadRobot;

        public IRobot DeadRobot
        {
            get { return deadRobot; }
            private set { deadRobot = value; }
        }
        
        public DeathEvent(IRobot deadRobot)
        {
            this.deadRobot = deadRobot;
        }
    }
}
