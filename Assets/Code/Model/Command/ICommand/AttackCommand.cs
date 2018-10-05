using System;
using UnityEngine;

namespace BotArena
{
    class AttackCommand : ICommand
    {

        float power;

        public AttackCommand(RobotController controller) {
            robotController = controller;
        }

        public void SetPower(float desiredPower) {
            power = Mathf.Clamp(desiredPower, 0.5f, 5f);
        }

        protected override int GetCooldown() {
            return robotController.weapon.GetCooldown();
        }

        //Abstract methods implemented

        protected override void Execute() {
            robotController.weapon.Attack(power);
            robotController.ConsumeEnergy(GetEnergyCost());
        }

        public override float GetEnergyCost() {
            return power * robotController.weapon.GetStaminaCost();
        }

        public override Command GetCommand() {
            return Command.ATTACK;
        }
    }
}
