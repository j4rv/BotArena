using UnityEngine;
using System.Collections.Generic;


namespace BotArena
{
    public abstract class IRobot
    {
        public string name;
        public RobotInfo info;
        public HashSet<RobotInfo> enemies;

        private RobotThread robotThread;

        public IRobot(RobotController parent)
        {
            info = new RobotInfo();
            robotThread = new RobotThread();
        }


        //              THREAD METHODS

        public bool StartTurn(RobotThreadSharedData robotData)
        {
            return robotThread.newTurn(() => Run(robotData));
        }

        //This runs on robotThread.
        private void Run(RobotThreadSharedData robotData)
        {
            Order order = robotData.GetLastOrder();
            Think(order);

            foreach(Event e in robotData.events)
            {
                EventHandler.HandleEvent(e, order, this);
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
        public virtual void OnRobotDetected(Order order, RobotInfo robotInfo) { }
        public virtual void OnWallHit(Collision wallCollision) { }
        public virtual void OnDeath(IRobot deadRobot) { }
    }
}