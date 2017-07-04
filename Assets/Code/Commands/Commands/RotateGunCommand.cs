using UnityEngine;

namespace BotArena
{
    internal class RotateGunCommand : ICommand
    {

        private float speed = 0f;

        public RotateGunCommand(RobotController controller)
        {
            robotController = controller;
        }

        public void SetSpeed(float speed)
        {
            speed = Mathf.Clamp(speed, -5, 5);
            this.speed = speed;
        }

        //Abstract methods implemented
        
        protected override void Execute()
        {            
             //Rotate along the Y axis
             robotController.weapon.transform.RotateAround(robotController.transform.position, robotController.transform.up, speed);
             robotController.ConsumeEnergy(GetStaminaCost());
        }

        public override float GetStaminaCost()
        {
            return Mathf.Abs(speed) / robotController.GetAgility();
        }

        public override Command GetCommand()
        {
            return Command.ROTATEGUN;
        }
    }
}