using UnityEngine;

namespace BotArena
{
    public abstract class ICommand
    {
        protected RobotController robotController;
        protected bool deletable = false;

        public abstract void Execute();
        public abstract bool CanExecute();
        public abstract float GetStaminaCost();
    }
}