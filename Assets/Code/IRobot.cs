using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class IRobot : MonoBehaviour {

    private float health;
    private float energy;
    private float maxSpeed;
    private GameObject head;
    protected Dictionary<string, ICommand> commands;

    public float GetHealth()
    {
        return health;
    }

    public float GetEnergy()
    {
        return energy;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetRotation()
    {
        return transform.rotation.eulerAngles;
    }

    public Vector3 GetHeadRotation()
    {
        return head.transform.rotation.eulerAngles;
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
    
    protected void Execute(string name, object args)
    {
        commands[name].Execute(args);
    }

    protected bool CanExecute(string name, object args)
    {
        return commands[name].CanExecute(args);
    }

    protected abstract void think();
    protected abstract void onEnemyAhead();

}
