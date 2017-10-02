using UnityEngine;

namespace BotArena {
    class RotateCommand : ICommand {

        float speed;

        public RotateCommand(RobotController controller) {
            robotController = controller;
        }

        public void SetSpeed(float desiredSpeed) {
            speed = Mathf.Clamp(desiredSpeed, -4, 4);
        }

        //Abstract methods implemented

        protected override void Execute() {
            //Rotate along the Y axis
            robotController.transform.Rotate(robotController.transform.up * speed);
            robotController.ConsumeEnergy(GetEnergyCost());
        }

        public override float GetEnergyCost() {
            return Mathf.Abs(speed) / robotController.GetAgility();
        }

        public override Command GetCommand() {
            return Command.ROTATE;
        }
    }
}