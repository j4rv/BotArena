using UnityEngine;

namespace BotArena
{
    public class EventHandler
    {
        public static void HandleEvent(IEvent eventToHandle, IRobot robot) {
            if (eventToHandle is RobotDetectedEvent) {
                RobotDetectedEvent robotDetected = (RobotDetectedEvent) eventToHandle;
                robot.OnRobotDetected(robotDetected.robotInfo);
            }
            else if (eventToHandle is WallHitEvent) {
                WallHitEvent wallHit = (WallHitEvent) eventToHandle;
                robot.OnWallHit(wallHit.wallCollision);
            }
            else if (eventToHandle is DeathEvent) {
                DeathEvent death = (DeathEvent) eventToHandle;
                robot.OnDeath(death.DeadRobot);
            }
        }
    }
}
