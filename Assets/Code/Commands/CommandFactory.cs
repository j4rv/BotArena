using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotArena
{
    class CommandFactory
    {
        public static ICommand Create(Command cmd, RobotController controller, params object[] args)
        {
            ICommand res = null;

            switch (cmd)
            {
                case Command.ROTATE:
                    {
                        RotateCommand rotate = new RotateCommand(controller);
                        float speed = (float)Convert.ToDouble(args[0]);
                        rotate.SetSpeed(speed);
                        res = rotate;

                        break;
                    }

                case Command.ROTATEGUN:
                    {
                        RotateGunCommand rotateGun = new RotateGunCommand(controller);
                        float speed = (float)Convert.ToDouble(args[0]);
                        rotateGun.SetSpeed(speed);
                        res = rotateGun;

                        break;
                    }

                case Command.GOFORWARD:
                    {
                        GoForwardCommand goForward = new GoForwardCommand(controller);
                        float speed = (float)Convert.ToDouble(args[0]);
                        goForward.SetSpeed(speed);
                        res = goForward;

                        break;
                    }
            }

            return res;
        }

        public static HashSet<Command> AvaliableCommands(IRobot robot)
        {
            HashSet<Command> res = new HashSet<Command>();

            //common commands
            res.Add(Command.ROTATE);
            res.Add(Command.ROTATEGUN);
            res.Add(Command.GOFORWARD);

            if(robot is ITank)
            {
                //...
            }

            return res;
        }
    }
}
