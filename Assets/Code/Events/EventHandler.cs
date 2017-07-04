namespace BotArena
{
    public class EventHandler
    {
        public static void HandleEvent(Event e, Order order, IRobot robot)
        {
            if(e is RobotDetectedEvent)
            {
                RobotDetectedEvent robotDetected = (RobotDetectedEvent)e;
                robot.OnRobotDetected(order, robotDetected.robotInfo);
            } else if (e is WallHitEvent)
            {
                WallHitEvent wallHit = (WallHitEvent)e;
                robot.OnWallHit(wallHit.wallCollision);
            }
        }
    }
}
