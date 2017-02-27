using UnityEngine;


namespace BotArena
{
    public class RobotInfo
    {
        public float health;
        public float energy;
        public float agility;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 headRotation;

        public RobotInfo(IRobot robot)
        {
            health = robot.health;
            energy = robot.energy;
            agility = robot.AGILITY;
            position = robot.position;
            rotation = robot.rotation;
            headRotation = robot.headRotation;
        }
    }
}