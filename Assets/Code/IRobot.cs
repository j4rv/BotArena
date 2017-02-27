using UnityEngine;
using System;
using System.Collections.Generic;


namespace BotArena
{
    public abstract class IRobot
    {

        public float health;
        public float energy;
        public readonly float AGILITY;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 headRotation;
        public HashSet<Command> commands;

        private HashSet<RobotInfo> enemies;
        private RobotController controller;


        public IRobot(RobotController parent)
        {
            controller = parent;
        }


        public RobotInfo GetRobotInfo()
        {
            return new RobotInfo(this);
        }

        public HashSet<IRobot> FindEnemies()
        {
            HashSet<IRobot> res = new HashSet<IRobot>();
            GameObject[] robots = GameObject.FindGameObjectsWithTag("Robot");

            foreach (GameObject robot in robots)
            {
                res.Add(robot.GetComponent<IRobot>());
            }

            return res;
        }


        protected void Execute(Command cmd, params object[] args)
        {
            controller.Execute(cmd, args);
        }

        protected bool CanExecute(Command cmd, params object[] args)
        {
            return controller.CanExecute(cmd, args);
        }

        //Abstract methods
        public abstract void Think();
        public abstract void OnEnemyAhead();

    }
}