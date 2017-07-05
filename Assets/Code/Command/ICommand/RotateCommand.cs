using UnityEngine;

namespace BotArena
{
    internal class RotateCommand : ICommand
    {

        private float speed = 0f;

        public RotateCommand(RobotController controller)
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
            robotController.transform.Rotate(robotController.transform.up * speed);
            robotController.ConsumeEnergy(GetEnergyCost());
        }

        public override float GetEnergyCost()
        {
            return Mathf.Abs(speed) / robotController.GetAgility();
        }

        public override Command GetCommand()
        {
            return Command.ROTATE;
        }
    }
}