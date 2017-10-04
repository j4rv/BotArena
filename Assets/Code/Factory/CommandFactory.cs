using System;

namespace BotArena
{
    static class CommandFactory
    {
        public static ICommand Create(Command cmd, RobotController controller, params object[] args) {
            ICommand res = null;

            switch (cmd) {
                case Command.ROTATE: {
                        RotateCommand rotate = new RotateCommand(controller);
                        float speed = (float) Convert.ToDouble(args[0]);
                        rotate.SetSpeed(speed);
                        res = rotate;

                        break;
                    }

                case Command.ROTATEGUN: {
                        RotateGunCommand rotateGun = new RotateGunCommand(controller);
                        float speed = (float) Convert.ToDouble(args[0]);
                        rotateGun.SetSpeed(speed);
                        res = rotateGun;

                        break;
                    }

                case Command.GOFORWARD: {
                        GoForwardCommand goForward = new GoForwardCommand(controller);
                        float speed = (float) Convert.ToDouble(args[0]);
                        goForward.SetSpeed(speed);
                        res = goForward;

                        break;
                    }

                case Command.ATTACK: {
                        AttackCommand attack = new AttackCommand(controller);
                        float power = (float) Convert.ToDouble(args[0]);
                        attack.SetPower(power);
                        res = attack;

                        break;
                    }

                default:
                    throw new ArgumentException("Bad Command parameter");
            }

            return res;
        }
    }
}
