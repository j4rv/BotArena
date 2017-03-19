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
                        rotateGun.SetSpeed(speed * 2);
                        res = rotateGun;

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

            if(robot is ITank)
            {
                //...
            }

            return res;
        }
    }
}
