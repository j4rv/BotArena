using System;
using UnityEngine;

namespace BotArena
{
    internal class AttackCommand : ICommand
    {

        private float power;

        public AttackCommand(RobotController controller) {
            robotController = controller;
        }

        public void SetPower(float power) {
            power = Mathf.Clamp(power, 0.5f, 5f);
            this.power = power;
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
