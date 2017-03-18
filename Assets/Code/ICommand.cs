using UnityEngine;

namespace BotArena
{
    public abstract class ICommand
    {
        public RobotController robotController;

        public abstract void Execute();
        public abstract bool CanExecute();
        public abstract float GetStaminaCost();
    }
}