using UnityEngine;

namespace BotArena
{
    internal abstract class ICommand
    {
        protected internal RobotController robotController;
        protected bool cancellable; // commands are not cancellable by default

        public bool Call() {
            if (CanExecute()) {
                Execute();
                return true;
            }
            return false;
        }

        public virtual bool CanExecute() {
            return (robotController.GetEnergy() >= GetEnergyCost())
                && (IsOnCooldown() == false);
        }

        protected bool IsOnCooldown() {
            int lastTurn = robotController.GetLastTurnExecuted(GetCommand());

            if (lastTurn == 0 || GetCooldown() == 0) {
                return false;
            }

            int cooldownEndsTurn = lastTurn + GetCooldown();
            return TurnManager.GetCurrentTurn() <= cooldownEndsTurn;
        }

        protected virtual int GetCooldown() { return 0; }

        protected abstract void Execute();
        public abstract float GetEnergyCost();
        public abstract Command GetCommand(); //Used for tracking the last time each command was used

    }
}