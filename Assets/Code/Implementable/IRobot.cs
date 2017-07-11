using UnityEngine;
using System.Collections.Generic;


namespace BotArena
{
    public abstract class IRobot
    {
        public RobotInfo info;

        protected string name;
        protected Order order;
        protected HashSet<RobotInfo> enemies;

        private RobotThread robotThread;

        //default values, can be overriden depending on the robot
        internal float maxHealth = 100;
        internal float maxEnergy = 100;
        internal float agility = 10;
        internal float energyRecoveryRate = 2;

        internal IRobot()
        {
            info = new RobotInfo();
            robotThread = new RobotThread();
        }


        //              THREAD METHODS

        internal bool StartTurn(RobotThreadSharedData robotData)
        {
            return robotThread.newTurn(() => Run(robotData));
        }

        //This runs on robotThread.
        private void Run(RobotThreadSharedData robotData)
        {
            order = robotData.GetLastOrder();
            Think();

            foreach(IEvent e in robotData.events)
            {
                EventHandler.HandleEvent(e, this);
            }
        }
        

        //              ROBOT METHODS
             
        internal void UpdateInfo(float hp, float en, float ag, Vector3 pos, Vector3 rot, Vector3 gunRot)
        {
            info.health = hp;
            info.energy = en;
            info.agility = ag;
            info.position = pos;
            info.rotation = rot;
            info.gunRotation = gunRot;
        }

        public string GetName()
        {
            return name;
        }
        
        //              ABSTRACT METHODS

        public virtual void Think() { }
        public virtual void OnRobotDetected(RobotInfo robotInfo) { }
        public virtual void OnWallHit(Collision wallCollision) { }
        public virtual void OnDeath(IRobot deadRobot) { }
    }
}