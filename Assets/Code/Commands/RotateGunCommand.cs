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
                Transform gun = ChildFinder.FindChildWithTag(robotController.gameObject, "Gun");
                //Rotate along the Y axis
                gun.Rotate(gun.transform.up * speed);
                robotController.robot.ConsumeEnergy(GetStaminaCost());
            }
        }

        public override float GetStaminaCost()
        {
            return Mathf.Abs(speed) * 0.1f;
        }
    }
}