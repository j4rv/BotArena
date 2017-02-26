using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ITank : IRobot { 

    public readonly new float AGILITY = 1;

    public ITank(RobotController parent) : base(parent)
    {
    }   
}