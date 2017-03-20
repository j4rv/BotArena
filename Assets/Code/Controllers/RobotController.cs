using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;


namespace BotArena
{
    public class RobotController : MonoBehaviour
    {
        public IRobot robot;
        public GameObject gun;
        public BodyController body;
        private RobotThreadShadedData threadData;
        private HashSet<Command> avaliableCommands;

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
        

        //              UNITY METHODS

        void Start()
        {
            maxHp = 100;
            maxEnergy = 100;
            health = maxHp;
            energy = maxEnergy;
            agility = 10;

            threadData = new RobotThreadShadedData();
            avaliableCommands = new HashSet<Command>();
            robot = DLLLoader.LoadRobotFromDLL(dllPath, this);
            avaliableCommands = CommandFactory.AvaliableCommands(robot);
            transform.name = robot.name;
        }

        void FixedUpdate()
        {
            if (TurnController.IsTurnUpdate())
            {      
                energy = Mathf.Clamp(energy + 1, 0, maxEnergy);    //Recover some energy

                UpdateRobot();
                ExecuteLastOrder();

                int turn = TurnController.GetCurrentTurn();
                Order order = new Order(this, turn);
                threadData.orders.Add(order);
                threadData.events.Clear();
                
                CheckRobotAhead();
                CheckWallHit();

                robot.StartTurn(threadData);
            }
        }

        private Collision LastColision;
        void OnCollisionEnter(Collision collision)
        {
            LastColision = collision;
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

        /*private HashSet<IRobot> FindEnemies()
        {
            HashSet<IRobot> res = new HashSet<IRobot>();
            GameObject[] robots = GameObject.FindGameObjectsWithTag("Robot");

            foreach (GameObject robot in robots)
            {
                res.Add(robot.GetComponent<IRobot>());
            }

            return res;
        }*/

        
        //              ORDER METHODS

        private void ExecuteLastOrder()
        {
            int lastTurn = TurnController.GetCurrentTurn() - 1;
            Order lastOrder = threadData.GetLastOrder(); 

            if (lastOrder != null
                && lastOrder.GetTurn() == lastTurn
                && lastOrder.IsExecuted() == false)
            {
                List<ICommand> commands = lastOrder.GetCommands();

                foreach (ICommand cmd in commands)
                {
                    //Check if command is in avaliableCommands
                    cmd.Execute();
                }
                lastOrder.Executed();
            }
        }


        //              EVENT CHECKERS

        private void CheckRobotAhead()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, gun.transform.forward, out hit))
            {
                if (hit.transform.tag == "Robot")
                {
                    RobotController hitRobotController = hit.transform.GetComponent<RobotController>();
                    RobotInfo enemyInfo = hitRobotController.robot.info;
                    RobotDetectedEvent e = new RobotDetectedEvent(enemyInfo);
                    threadData.events.Add(e);
                }
            }
        }

        private void CheckWallHit()
        {
            if(LastColision != null && LastColision.transform.tag == "Wall")
            {
                WallHitEvent e = new WallHitEvent(LastColision);
                threadData.events.Add(e);
                LastColision = null;
            }
        }

    }
}