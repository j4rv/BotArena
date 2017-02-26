using System;
using UnityEngine;

public class RotateCommand : ICommand
{

    private float speed = 0f;

    public RotateCommand(IRobot r)
    {
        robot = r;
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

