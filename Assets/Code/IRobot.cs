using UnityEngine;
using System;
using System.Collections.Generic;


namespace BotArena
{
    public abstract class IRobot
    {
        private float maxHp;
        private float health;
        private float maxEnergy;
        private float energy;
        private float agility;
        private RobotController controller;

        public string name;
        public RobotInfo info;
        public HashSet<RobotInfo> enemies;
        

        public IRobot(RobotController parent)
        {
            maxHp = 100;
            maxEnergy = 100;
            health = maxHp;
            energy = maxEnergy;
            agility = 10;

            info = new RobotInfo();
            controller = parent;
        }
        

        //              ROBOT METHODS

        public float GetEnergy()
        {
            return energy;
        }

        public void ConsumeEnergy(float consumption)
        {
            if(consumption >= 0) { 
                energy -= consumption;
            }
        }

        public float GetAgility()
        {
            return agility;
        }

        public void UpdateInfo(Vector3 pos, Vector3 rot, Vector3 gunRot)
        {
            info.health = health;
            info.energy = energy;
            info.agility = agility;
            info.position = pos;
            info.rotation = rot;
            info.gunRotation = gunRot;
        }

        //              COMMAND METHODS

        protected void Execute(Command cmd, params object[] args)
        {
            controller.Execute(cmd, args);
        }

        protected bool CanExecute(Command cmd, params object[] args)
        {
            return controller.CanExecute(cmd, args);
        }

        //              ABSTRACT METHODS

        public abstract void Think();
        public abstract void OnEnemyAhead();

    }
}