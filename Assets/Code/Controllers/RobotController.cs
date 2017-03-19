using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;


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

        private RobotThread robotThread;

        public IRobot robot;
        public GameObject gun;
        public List<Order> orders;
        public HashSet<Command> avaliableCommands;


        //              UNITY METHODS

        void Start()
        {
            maxHp = 100;
            maxEnergy = 100;
            health = maxHp;
            energy = maxEnergy;
            agility = 10;

            orders = new List<Order>();
            avaliableCommands = new HashSet<Command>();
            robot = DLLLoader.LoadRobotFromDLL(dllPath, this);
            avaliableCommands = CommandFactory.AvaliableCommands(robot);
            transform.name = robot.name;
            robotThread = new RobotThread();
        }

        void FixedUpdate()
        {
            if (TurnController.IsTurnUpdate())
            {
                int turn = TurnController.GetCurrentTurn();
                //Recover some energy
                energy = Mathf.Clamp(energy + 1, 0, maxEnergy);
                UpdateRobot();
                ExecuteLastOrder();

                Order order = new Order(this, turn);
                orders.Add(order);

                robotThread.NewJob(() => robot.Think(order));
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
            Order lastOrder = orders.LastOrDefault(); //LastOrDefault because Last will throw an exception on the first turn.

            if (lastOrder != null
                && lastOrder.GetTurn() == lastTurn
                && lastOrder.IsExecuted() == false)
            {
                List<ICommand> commands = lastOrder.GetCommands();

                foreach (ICommand cmd in commands)
                {
                    cmd.Execute();
                }
                lastOrder.Executed();

            }
        }

        private void CheckEnemyAhead(Order order)
        {
            //check if there's an enemy ahead, if there is, execute robot.OnEnemyAhead()
            //TODO
            RaycastHit hit;

            if (Physics.Raycast(transform.position, gun.transform.forward, out hit)) { 
                if(hit.collider.isTrigger)
                    if(hit.transform.tag == "Robot")
                    {
                        RobotController hitRobotController = hit.transform.GetComponent<RobotController>();
                        RobotInfo enemyInfo = hitRobotController.robot.info;
                        robotThread.NewJob(() => robot.OnEnemyDetected(order, enemyInfo));
                    }
            }
        }

    }
}