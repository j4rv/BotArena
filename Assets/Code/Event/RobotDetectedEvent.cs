namespace BotArena
{
    public class RobotDetectedEvent : IEvent
    {
        public RobotInfo robotInfo { get; internal set;}

        public RobotDetectedEvent(RobotInfo robotInfo) {
            this.robotInfo = robotInfo;
        }
    }
}
