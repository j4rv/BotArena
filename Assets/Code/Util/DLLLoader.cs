using System;
using System.Reflection;
using UnityEngine;

namespace BotArena
{
    internal static class DLLLoader
    {
        public static IRobot LoadRobotFromDLL(string dllPath, RobotController parent)
        {
            IRobot res = null;
            
            Assembly DLL = Assembly.LoadFile(@dllPath);
            var robotType = DLL.GetType("BotArena" + ".Robot");
            
            var instance = Activator.CreateInstance(robotType, new object[] { parent });
            
            //Checks if robotType inherits from IRobot
            if (instance is IRobot)
            {
                res = (IRobot) instance;
            }

            return res;
        }
    }
}