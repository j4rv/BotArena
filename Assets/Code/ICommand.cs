using UnityEngine;

public abstract class ICommand
{
    public IRobot robot;

    public abstract void Execute();
    public abstract bool CanExecute();
    public abstract double GetStaminaCost();
}