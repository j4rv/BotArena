using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BotArena
{
    internal static class DllUtil
    {
        static Random rng = new Random();

        public static IRobot CreateFirstRobotFromDll(string dllPath) {
            IList<Type> robots = FindRobotsInDll(dllPath);
            
            return (IRobot) Activator.CreateInstance(robots[0], new object[] { });
        }

        public static IRobot CreateRandomRobotFromDll(string dllPath) {
            IList<Type> robots = GetTypes<IRobot>(dllPath);
            int randomIndex = (int) (rng.NextDouble() * robots.Count);
            
            return (IRobot) Activator.CreateInstance(robots[randomIndex], new object[] { });
        }

        private static IList<Type> FindRobotsInDll(string dllPath) {
            return GetTypes<IRobot>(dllPath);
        }


        ///Learning some Linq!
        ///http://stackoverflow.com/questions/3353699/using-reflection-to-get-all-classes-of-certain-base-type-in-dll
        private static IList<Type> GetTypes<T>(String dllPath) {
            Assembly dll = Assembly.LoadFile(@dllPath);
            return (from t in dll.GetTypes()
                    where t.IsSubclassOf(typeof(T))
                    select t).ToList();
        }
    }

}