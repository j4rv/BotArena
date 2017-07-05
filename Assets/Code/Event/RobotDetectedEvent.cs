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
