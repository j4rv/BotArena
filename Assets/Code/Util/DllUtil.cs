using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BotArena
{
    internal static class DllUtil
    {
        public static IRobot LoadRobotFromDll(string dllPath) {
            IRobot res = null;

            IList<Type> robots = GetTypes<IRobot>(dllPath);

            //Only gets the first robot from the dll, for now
            res = (IRobot) Activator.CreateInstance(robots[0], new object[] { });

            return res;
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