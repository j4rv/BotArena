using UnityEngine;

namespace BotArena
{
    class GoForwardCommand : ICommand
    {

        float speed;

        public GoForwardCommand(RobotController controller) {
            robotController = controller;
        }

        public void SetSpeed(float desiredSpeed) {
            speed = Mathf.Clamp(desiredSpeed, -6, 6);
        }

        //Abstract methods implemented

        public override bool CanExecute() {
            return base.CanExecute() && robotController.body.CanMove();
        }

		/// <summary>
		/// Move the robot forward/backwards.
		/// </summary>
        protected override void Execute() {
            robotController.GetComponent<Rigidbody>().velocity = robotController.transform.forward * speed;
            robotController.ConsumeEnergy(GetEnergyCost());
        }

        public override float GetEnergyCost() {
            return Mathf.Abs(speed) * 1.2f / robotController.GetAgility();
        }

        public override Command GetCommand() {
            return Command.GOFORWARD;
        }
    }
}