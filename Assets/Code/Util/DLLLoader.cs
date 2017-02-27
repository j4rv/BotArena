using System;
using System.Reflection;

namespace BotArena
{
    internal static class DLLLoader
    {
        public static IRobot LoadRobotFromDLL(string dllPath)
        {
            IRobot res = null;
            var DLL = Assembly.LoadFile(dllPath);
            var robotType = DLL.GetType("DLL.Robot");

            //Checks if robotType inherits from IRobot
            if (robotType.IsAssignableFrom(typeof(IRobot)))
            {
                res = (IRobot)Activator.CreateInstance(robotType);
            }

            return res;
        }
    }
}