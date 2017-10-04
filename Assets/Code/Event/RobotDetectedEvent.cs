namespace BotArena
{
    public class RobotDetectedEvent : IEvent
    {
        RobotInfo robotInfo;

        public RobotDetectedEvent(RobotInfo robotInfo) {
            this.robotInfo = robotInfo;
        }
    }
}
