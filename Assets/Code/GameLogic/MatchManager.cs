using System.Collections.Generic;
using UnityEngine;

namespace BotArena
{
    public class MatchManager : MonoBehaviour
    {
        private Dictionary<IRobot, RobotController> robots;

        private static MatchManager instance;

        private static MatchManager Get() {
            if (instance == null)
                instance = (MatchManager) FindObjectOfType(typeof(MatchManager));
            return instance;
        }

        private void Start() {
            robots = new Dictionary<IRobot, RobotController>();

            Vector3 randomPosition = RandomUtil.RandomPositionInsideMap();
            Vector3 randomRotation = RandomUtil.RandomRotationHorizontal();

            string path = @"F:\TFG\\Libraries\BasicAI.dll"; // hardcoded for now
            InstantiateRobotFromDll(path, randomPosition, Quaternion.Euler(randomRotation));
            InstantiateRobotFromDll(path, -randomPosition, Quaternion.Euler(-randomRotation));
        }



        void FixedUpdate() {

        }

        private RobotController InstantiateRobotFromDll(string dllPath, Vector3 position, Quaternion rotation) {
            IRobot robot = DllUtil.LoadRobotFromDll(dllPath);
            string name = robot.GetName();

            RobotController robotController = RobotPrefabFactory.Create(robot, position, rotation);
            robotController.gameObject.name = name;

            robots.Add(robot, robotController);

            return robotController;
        }

    }
}