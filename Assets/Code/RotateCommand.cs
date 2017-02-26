using System;
using UnityEngine;
using UnityEngine.Assertions;

internal class RotateCommand : ICommand
{

    private float speed = 0f;

    public RotateCommand(RobotController r)
    {
        robotController = r;
    }

    public void SetSpeed(float speed)
    {
        Assert.IsTrue(speed <= 1 && speed >= -1);
        this.speed = speed;
    }

    //Abstract methods implemented

    public override bool CanExecute()
    {
        bool res;

        res = robotController.robot.energy >= GetStaminaCost();

        return res;
    }

    public override void Execute()
    {
        if (CanExecute())
        {
            Rigidbody rb = robotController.GetComponent<Rigidbody>();
            //Rotate along the Y axis
            Quaternion rot = Quaternion.Euler(robotController.transform.up * speed);
            rb.MoveRotation(rot);
        }
    }

    public override double GetStaminaCost()
    {
        return 5.0 * Mathf.Abs(speed);
    }
}

