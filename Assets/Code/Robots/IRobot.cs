using UnityEngine;
using System;
using System.Collections.Generic;


namespace BotArena
{
    public abstract class IRobot
    {
        public string name;
        public RobotInfo info;
        public HashSet<RobotInfo> enemies;
        

        public IRobot(RobotController parent)
        {
            info = new RobotInfo();
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