using UnityEngine;

public class RobotInfo
{
    private float health;
    private float energy;
    private float agility;
    private Vector3 position;
    private Vector3 rotation;
    private Vector3 headRotation;

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