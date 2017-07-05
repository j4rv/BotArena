namespace BotArena
{
    public class RobotDetectedEvent : IEvent
    {
        public RobotInfo robotInfo;

        public RobotDetectedEvent(RobotInfo robotInfo) {
            this.robotInfo = robotInfo;
        }
    }
}
