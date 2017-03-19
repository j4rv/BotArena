using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


namespace BotArena
{
    public class RobotController : MonoBehaviour
    {
        [SerializeField]
        private string dllPath;
        [SerializeField]
        private float maxHp;
        [SerializeField]
        private float health;
        [SerializeField]
        private float maxEnergy;
        [SerializeField]
        private float energy;
        [SerializeField]
        private float agility;

        private Thread robotThread;
        
        public IRobot robot;
        public GameObject gun;
        public Dictionary<int, Order> orders;


        //              UNITY METHODS

        void Start()
        {
            maxHp = 100;
            maxEnergy = 100;
            health = maxHp;
            energy = maxEnergy;
            agility = 10;

            orders = new Dictionary<int, Order>();
            robot = DLLLoader.LoadRobotFromDLL(dllPath, this);
            transform.name = robot.name;
        }

        void FixedUpdate()
        {
            if (TurnController.IsTurnUpdate())
            {
                //Recover some energy
                energy = Mathf.Clamp(energy + 1, 0, maxEnergy);
                UpdateRobot();
                ExecuteLastOrder();

                Order order = new Order(this);
                orders.Add(TurnController.GetCurrentTurn(), order);

                robotThread = new Thread(() => robot.Think(order));
                robotThread.Start();

                CheckEnemyAhead(order);
            }
        }


        //              ROBOT METHODS

        public void UpdateRobot()
        {
            Vector3 pos = transform.position;
            Vector3 rot = transform.rotation.eulerAngles;
            Vector3 gunRot = gun.transform.rotation.eulerAngles;
            robot.UpdateInfo(health, energy, agility, pos, rot, gunRot);
        }

        public float GetEnergy()
        {
            return energy;
        }

        public void ConsumeEnergy(float consumption)
        {
            if (consumption >= 0)
            {
                energy -= consumption;
            }
        }

        public float GetAgility()
        {
            return agility;
        }

        private HashSet<IRobot> FindEnemies()
        {
            HashSet<IRobot> res = new HashSet<IRobot>();
            GameObject[] robots = GameObject.FindGameObjectsWithTag("Robot");

            foreach (GameObject robot in robots)
            {
                res.Add(robot.GetComponent<IRobot>());
            }

            return res;
        }


        //              COMMAND METHODS

        private void ExecuteLastOrder()
        {
            int lastTurn = TurnController.GetCurrentTurn() - 1;

            if (orders.ContainsKey(lastTurn)){ 
                Order order = orders[TurnController.GetCurrentTurn() - 1];
                if (order.IsExecuted() == false) { 
                    List<ICommand> commands = order.GetCommands();

                    foreach (ICommand cmd in commands)
                    {
                        cmd.Execute();
                    }
                    order.Executed();
                }
            }
        }

        private void CheckEnemyAhead(Order order)
        {
            //check if there's an enemy ahead, if there is, execute robot.OnEnemyAhead()
            //TODO
                        
            RobotInfo enemyInfo = new RobotInfo();
            robotThread = new Thread(() => robot.OnEnemyDetected(order, enemyInfo));
            robotThread.Start();
        }
    }
}