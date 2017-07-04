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
        
        protected override void Execute()
        {
            //Go forward
            robotController.GetComponent<Rigidbody>().velocity = robotController.transform.forward * speed;
            robotController.ConsumeEnergy(GetStaminaCost());
        }

        public override float GetStaminaCost()
        {
            return Mathf.Abs(speed) / robotController.GetAgility();
        }

        public override Command GetCommand()
        {
            return Command.GOFORWARD;
        }
    }
}