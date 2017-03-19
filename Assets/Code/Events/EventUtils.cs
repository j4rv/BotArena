
namespace BotArena
{
    public class EventUtils
    {
        public static void HandleEvent(Event e, Order order, IRobot robot)
        {
            if(e is RobotDetectedEvent)
            {
                RobotDetectedEvent robotDetected = (RobotDetectedEvent)e;
                robot.OnEnemyDetected(order, robotDetected.enemyInfo);
            } else if (e is WallHitEvent)
            {
                WallHitEvent wallHit = (WallHitEvent)e;
                robot.OnWallHit(wallHit.wallCollision);
            }
        }
    }
}
