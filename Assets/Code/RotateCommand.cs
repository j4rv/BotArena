using System;
using UnityEngine;
using UnityEngine.Assertions;

public class RotateCommand : ICommand
{

    private float speed = 0f;

    public RotateCommand(IRobot r)
    {
        robot = r;
    }

    public void SetSpeed(float speed)
    {
        Assert.IsTrue(speed <= 1 && speed >= -1);
        this.speed = speed;
    }

    //Abstract methods implemented

    public override bool CanExecute()
    {
        throw new NotImplementedException();
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }

    public override double GetStaminaCost()
    {
        return 5.0 * Mathf.Abs(speed);
    }
}

