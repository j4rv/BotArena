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
            Assert.IsTrue(speed <= 1 && speed >= -1);
            this.speed = speed;
        }

        //Abstract methods implemented

        public override bool CanExecute()
        {
            bool res;

            robotController.robot.energy = 50;
            res = robotController.robot.energy >= GetStaminaCost();

            return res;
        }

        public override void Execute()
        {
            if (CanExecute())
            {
                Transform gun = ChildFinder.FindChildWithTag(robotController.gameObject, "Gun");
                //Rotate along the Y axis
                gun.Rotate(gun.transform.up * speed);
            }
        }

        public override double GetStaminaCost()
        {
            return 5 * Mathf.Abs(speed);
        }
    }
}