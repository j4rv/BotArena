using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class IRobot {

    public float health;
    public float energy;
    public float agility;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 headRotation;

    protected Dictionary<string, ICommand> commands;

    private HashSet<RobotInfo> enemies;
    private RobotController controller;


    public IRobot(RobotController parent)
    {
        controller = parent;
    }


    public HashSet<IRobot> FindEnemies()
    {
        HashSet<IRobot> res = new HashSet<IRobot>();
        GameObject[] robots = GameObject.FindGameObjectsWithTag("Robot");

        foreach(GameObject robot in robots)
        {
            res.Add(robot.GetComponent<IRobot>());
        }

        return res;
    }
    

    protected void Execute(string name, params object[] args)
    {
        controller.Execute(name, args);
    }

    protected bool CanExecute(string name, params object[] args)
    {
        return controller.CanExecute(name, args);
    }

    public abstract void Think();
    public abstract void OnEnemyAhead();

}
