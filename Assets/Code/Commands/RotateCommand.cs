using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BotArena
{
    internal class RotateCommand : ICommand
    {

        private float speed = 0f;

        public RotateCommand(RobotController r)
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
            
            res = robotController.GetEnergy() >= GetStaminaCost();

            return res;
        }

        public override void Execute()
        {
            if (CanExecute())
            {
                //Rotate along the Y axis
                robotController.transform.Rotate(robotController.transform.up * speed);
                robotController.ConsumeEnergy(GetStaminaCost());
            }
        }

        public override float GetStaminaCost()
        {
            return Mathf.Abs(speed) / robotController.GetAgility();
        }
    }
}