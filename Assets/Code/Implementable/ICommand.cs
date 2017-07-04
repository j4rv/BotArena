using UnityEngine;

namespace BotArena
{
    public abstract class ICommand
    {
        protected RobotController robotController;
        protected bool deletable = false;

        public bool Call()
        {
            if (CanExecute())
            {
                Execute();
                return true;
            }
            return false;
        }

        public bool CanExecute()
        {
            return (robotController.GetEnergy() >= GetStaminaCost())
                && (IsOnCooldown() == false);
        }

        protected bool IsOnCooldown()
        {
            int lastTurn = robotController.GetLastTurnExecuted(GetCommand());

            if(lastTurn == 0 || GetCooldown() == 0)
            {
                return false;
            }

            int cooldownEndsTurn = lastTurn + GetCooldown();
            return TurnManager.GetCurrentTurn() <= cooldownEndsTurn;
        }

        protected virtual int GetCooldown() { return 0; }

        protected abstract void Execute();        
        public abstract float GetStaminaCost();
        public abstract Command GetCommand();

    }
}