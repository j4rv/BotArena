using UnityEngine;

namespace BotArena
{
    internal class GoForwardCommand : ICommand
    {

        private float speed = 0f;

        public GoForwardCommand(RobotController controller)
        {
            robotController = controller;
        }

        public void SetSpeed(float speed)
        {
            speed = Mathf.Clamp(speed, -5, 5);
            this.speed = speed;
        }

        //Abstract methods implemented

        public override bool CanExecute()
        {
            bool res;
            
            res = robotController.GetEnergy() >= GetStaminaCost()
                && robotController.body.CanMove();

            return res;
        }

        public override void Execute()
        {
            if (CanExecute())
            {
                //Go forward
                robotController.GetComponent<Rigidbody>().velocity = robotController.transform.forward * speed;
                robotController.ConsumeEnergy(GetStaminaCost());
            }
        }

        public override float GetStaminaCost()
        {
            return Mathf.Abs(speed) / robotController.GetAgility();
        }
    }
}