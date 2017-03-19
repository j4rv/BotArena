﻿using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;


namespace BotArena
{
    public abstract class IRobot
    {
        public string name;
        public RobotInfo info;
        public HashSet<RobotInfo> enemies;

        private RobotThread robotThread;
        private RobotThreadShadedData robotData;

        public IRobot(RobotController parent)
        {
            info = new RobotInfo();
            robotThread = new RobotThread();
        }


        //              THREAD METHODS

        public void StartTurn(RobotThreadShadedData robotData)
        {
            this.robotData = robotData;
            robotThread.newTurn(() => Run(robotData));
        }

        //This runs on robotThread.
        private void Run(RobotThreadShadedData robotData)
        {
            Order order = robotData.GetLastOrder();

            Think(order);
            foreach(Event e in robotData.events)
            {
                EventUtils.HandleEvent(e, order, this);
            }
        }
        

        //              ROBOT METHODS
             
        public void UpdateInfo(float hp, float en, float ag, Vector3 pos, Vector3 rot, Vector3 gunRot)
        {
            info.health = hp;
            info.energy = en;
            info.agility = ag;
            info.position = pos;
            info.rotation = rot;
            info.gunRotation = gunRot;
        }
        
        //              ABSTRACT METHODS

        public virtual void Think(Order order) { }
        public virtual void OnEnemyDetected(Order order, RobotInfo enemyInfo) { }
    }
}