using UnityEngine;
using System.Collections.Generic;

namespace BotArena {
    public abstract class IRobot {

        public RobotInfo info                   { get; internal set;}
        public string player                    { get; internal set;}
        public string name                      { get; protected set;}
        protected Order order;
        protected HashSet<RobotInfo> enemies;

        readonly RobotThread robotThread;

        //default values, can be overriden depending on the robot
        internal float maxHealth = 100;
        internal float maxEnergy = 100;
        internal float agility = 10;
        internal float energyRecoveryRate = 1.5f;

        protected IRobot(){
            info = new RobotInfo();
            info.player = player;
            robotThread = new RobotThread();
        }


        //              THREAD METHODS

        internal bool StartTurn(RobotThreadSharedData robotData){
            return robotThread.newTurn(() => Run(robotData));
        }

        //This runs on robotThread.
        void Run(RobotThreadSharedData robotData){
            order = robotData.GetLastOrder();
            Think();

            foreach(IEvent e in robotData.events)
            {
                EventHandler.HandleEvent(e, this);
            }
        }

        //              ORDER METHODS
        //          Just to make Robots more convenient and clean to program

        bool AddCommand(Command command, object[] args){
            return order.AddCommand(command, args);
        }

        bool RemoveCommand(Command command) {
            return order.RemoveCommand(command);
        }

        void ClearCommands() {
        	order.Reset();
        }
        

        //              ROBOT METHODS
             
        internal void UpdateInfo(float hp, float en, 
                                 Vector3 pos, Vector3 rot, Vector3 gunRot){
            info.health = hp;
            info.energy = en;
            info.position = pos;
            info.rotation = rot;
            info.gunRotation = gunRot;
        }

        public RobotInfo GetInfo(){
            return info;
        }

		internal void SetEnemies(HashSet<RobotInfo> enemiesSet){
			enemies = enemiesSet;
		}
        
        //              ABSTRACT METHODS

        public virtual void Think() { }
        public virtual void OnRobotDetected(RobotInfo robotInfo) { }
        public virtual void OnWallHit(Collision wallCollision) { }
        public virtual void OnDeath(IRobot deadRobot) { }
    }
}