using UnityEngine;

namespace BotArena
{
    internal class AttackCommand : ICommand {

        private float power;

        public AttackCommand(RobotController controller)
        {
            robotController = controller;
        }

        public void SetPower(float power)
        {
            power = Mathf.Clamp(power, 0.5f, 5f);
            this.power = power;
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
                //Tell the robot to attack!
                robotController.weapon.Attack(power);
                robotController.ConsumeEnergy(GetStaminaCost());
            }
        }

        public override float GetStaminaCost()
        {
            return power * robotController.weapon.GetStaminaCost();
        }
    }
}
