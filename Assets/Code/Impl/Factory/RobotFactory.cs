using UnityEngine;

namespace BotArena
{
    static class RobotFactory
    {

        private static readonly Compiler<IRobot> compiler = new Compiler<IRobot>();

        public static IRobot Create(string robotFilename) {
            return compiler.CompileAndCreateFromFilename(robotFilename);
        }

    }
}
