using UnityEngine;

public abstract class ICommand
{
    public float staminaCost;
    public IRobot robot;

    public abstract void Execute(object args);
    public abstract bool CanExecute(object args);
}