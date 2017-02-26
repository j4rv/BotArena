using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class RobotController : MonoBehaviour
{
    public IRobot robot;
    public HashSet<ICommand> commands;

    internal void Execute(string name, object[] args)
    {
        //TODO
        throw new NotImplementedException();
    }

    internal bool CanExecute(string name, object[] args)
    {
        //TODO
        throw new NotImplementedException();
    }
}
