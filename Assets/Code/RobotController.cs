using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;


public class RobotController : MonoBehaviour
{
    public IRobot robot;
    public GameObject head;
    public Dictionary<Command, ICommand> commands;

    private void Start(string dllPath)
    {
        commands = new Dictionary<Command, ICommand>();
        var DLL = Assembly.LoadFile(dllPath);
        var robotType = DLL.GetType("DLL.Robot");

        //Checks if robotType inherits from IRobot
        if(robotType.IsAssignableFrom(typeof(IRobot)))
        {
            IRobot robotInstance = (IRobot)Activator.CreateInstance(robotType);
            robot = robotInstance;
            RotateCommand rotateCmd = new RotateCommand(robot);

            //Base commands, all robots can execute them
            commands.Add(Command.ROTATE, rotateCmd);
        }

    }

    public void Execute(string name, object[] args)
    {
        //TODO
        throw new NotImplementedException();
    }

    public bool CanExecute(string name, object[] args)
    {
        //TODO
        throw new NotImplementedException();
    }
}
