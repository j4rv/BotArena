namespace BotArena
{
    public class DeathEvent : IEvent
    {
        public IRobot deadRobot { get; internal set;}

        public IRobot DeadRobot {
            get { return deadRobot; }
            private set { deadRobot = value; }
        }

        public DeathEvent(IRobot deadRobot) {
            this.deadRobot = deadRobot;
        }
    }
}
