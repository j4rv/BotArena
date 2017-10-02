using UnityEngine;

namespace BotArena {
    
    class RotateGunCommand : ICommand {

        float speed;

        public RotateGunCommand(RobotController controller) {
            robotController = controller;
        }

        public void SetSpeed(float desiredSpeed) {
            speed = Mathf.Clamp(desiredSpeed, -6, 6);
        }

        //Abstract methods implemented

        protected override void Execute() {
            //Rotate along the Y axis
            robotController.weapon.transform.RotateAround(robotController.transform.position, robotController.transform.up, speed);
            robotController.ConsumeEnergy(GetEnergyCost());
        }

        public override float GetEnergyCost() {
            return Mathf.Abs(speed) / robotController.GetAgility();
        }

        public override Command GetCommand() {
            return Command.ROTATEGUN;
        }
    }
}