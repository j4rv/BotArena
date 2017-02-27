using UnityEngine;
using System;
using System.Reflection;


namespace BotArena
{
    public class CommandListener : MonoBehaviour
    {
        void FixedUpdate()
        {
            var DLL = Assembly.LoadFile(@"c:\users\main\documents\visual studio 2015\Projects\ClassLibrary1\ClassLibrary1\bin\Debug\ClassLibrary1.dll");

            var robot = DLL.GetType("DLL.Robot");
            IRobot robotInstance = (IRobot)Activator.CreateInstance(robot);
            robotInstance.Think();
        }
    }
}