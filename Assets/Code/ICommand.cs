using UnityEngine;

internal abstract class ICommand
{
    public RobotController robotController;

    public abstract void Execute();
    public abstract bool CanExecute();
    public abstract double GetStaminaCost();
}