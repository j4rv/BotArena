using UnityEngine;

namespace BotArena
{
    internal class RobotPrefabFactory
    {
        public static RobotController Create(IRobot robot, Vector3 position, Quaternion rotation) {
            RobotController res = null;

            if (robot is ITank) {
                res = Resources.Load<RobotController>("Prefabs/Robots/Tank");
                res = Object.Instantiate(res, position, rotation);
                res.robot = robot;
            }

            return res;
        }
    }
}
