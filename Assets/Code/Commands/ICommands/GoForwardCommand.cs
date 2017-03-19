using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BotArena
{
    internal class GoForwardCommand : ICommand
    {

        private float speed = 0f;

        public GoForwardCommand(RobotController r)
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
                //Go forward
                Transform body = robotController.body.transform;
                robotController.GetComponent<Rigidbody>().AddForce(body.forward * 1000 * speed);
                robotController.ConsumeEnergy(GetStaminaCost());
            }
        }

        public override float GetStaminaCost()
        {
            return Mathf.Abs(speed) * 2 / robotController.GetAgility();
        }
    }
}