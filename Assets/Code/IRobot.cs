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
    private HashSet<RobotInfo> enemies;

    protected Dictionary<string, ICommand> commands;
    

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
    
    protected void Execute(string name, object args)
    {
        commands[name].Execute(args);
    }

    protected bool CanExecute(string name, object args)
    {
        return commands[name].CanExecute(args);
    }

    public abstract void Think();
    public abstract void OnEnemyAhead();

}
