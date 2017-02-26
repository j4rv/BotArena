using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class IRobot {

    public float health;
    public float energy;
    public readonly float AGILITY;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 headRotation;

    protected Dictionary<Command, ICommand> commands;

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
    

    protected void Execute(Command cmd, params object[] args)
    {
        controller.Execute(cmd, args);
    }

    protected bool CanExecute(Command cmd, params object[] args)
    {
        return controller.CanExecute(cmd, args);
    }

    //Methods that should be written in an external DLL
    public void Think() { }
    public void OnEnemyAhead() { }

}
