using UnityEngine;
using UnityEngine.Assertions;

namespace BotArena
{
    internal class RotateGunCommand : ICommand
    {

        private float speed = 0f;

        public RotateGunCommand(RobotController r)
        {
            robotController = r;
        }

        public void SetSpeed(float speed)
        {
            speed = Mathf.Clamp(speed, -1, 1);
            this.speed = speed;
        }

        //Abstract methods implemented

        public override bool CanExecute()
        {
            bool res;
            
            res = robotController.robot.GetEnergy() >= GetStaminaCost();

            return res;
        }

        public override void Execute()
        {
            if (CanExecute())
            {
                //Rotate along the Y axis
                robotController.gun.transform.Rotate(robotController.gun.transform.up * speed);
                robotController.robot.ConsumeEnergy(GetStaminaCost());
            }
        }

        public override float GetStaminaCost()
        {
            return Mathf.Abs(speed) * 0.1f;
        }
    }
}