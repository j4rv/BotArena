using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


internal class RobotController : MonoBehaviour
{
    public IRobot robot;
    public GameObject head;
    public Dictionary<Command, ICommand> commands;


    //Unity methods

    private void Start(string dllPath)
    {
        commands = new Dictionary<Command, ICommand>();
        var DLL = Assembly.LoadFile(dllPath);
        var robotType = DLL.GetType("DLL.Robot");

        //Checks if robotType inherits from IRobot
        if (robotType.IsAssignableFrom(typeof(IRobot)))
        {
            IRobot robotInstance = (IRobot)Activator.CreateInstance(robotType);
            robot = robotInstance;
            RotateCommand rotateCmd = new RotateCommand(this);

            //Base commands, all robots can execute them
            commands.Add(Command.ROTATE, rotateCmd);
        }

    }

    private void FixedUpdate()
    {
        UpdateRobot();
        robot.Think();
        CheckEnemyAhead();
    }

    // Robot methods

    public void UpdateRobot()
    {
        robot.position = transform.position;
        robot.rotation = transform.rotation.eulerAngles;
        robot.headRotation = head.transform.rotation.eulerAngles;
    }

    public void Execute(Command cmd, object[] args)
    {
        switch (cmd)
        {
            case Command.ROTATE:
                RotateCommand rotate = (RotateCommand)commands[cmd];
                float speed = (float)args[0];

                rotate.SetSpeed(speed);
                rotate.Execute();

                break;
        }
    }

    public bool CanExecute(Command cmd, object[] args)
    {
        bool res = false;

        switch (cmd)
        {
            case Command.ROTATE:
                RotateCommand rotate = (RotateCommand)commands[cmd];
                float speed = (float)args[0];

                rotate.SetSpeed(speed);
                res = rotate.CanExecute();

                break;
        }

        return res;
    }

    public void CheckEnemyAhead()
    {
        //check if there's an enemy ahead, if there is, execute robot.OnEnemyAhead()
        //robot.OnEnemyAhead();
    }
}
