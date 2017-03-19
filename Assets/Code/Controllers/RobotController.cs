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
        public GameObject body;
        private RobotThreadShadedData robotData;
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

            robotData = new RobotThreadShadedData();
            avaliableCommands = new HashSet<Command>();
            robot = DLLLoader.LoadRobotFromDLL(dllPath, this);
            avaliableCommands = CommandFactory.AvaliableCommands(robot);
            transform.name = robot.name;
        }

        void FixedUpdate()
        {
            if (TurnController.IsTurnUpdate())
            {      
                energy = Mathf.Clamp(energy + 0.075f, 0, maxEnergy);    //Recover some energy

                UpdateRobot();
                ExecuteLastOrder();

                int turn = TurnController.GetCurrentTurn();
                Order order = new Order(this, turn);
                robotData.orders.Add(order);
                robotData.events.Clear();
                
                CheckEnemyAhead();
                CheckWallHit();

                robot.StartTurn(robotData);
            }
        }

        private Collision LastColision;
        void OnCollisionEnter(Collision collision)
        {
            Debug.Log("collision");
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
            Order lastOrder = robotData.GetLastOrder(); 

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


        //              EVENT CHECKERS

        private void CheckEnemyAhead()
        {
            //check if there's an enemy ahead, if there is, execute robot.OnEnemyAhead()
            //TODO
            RaycastHit hit;

            if (Physics.Raycast(transform.position, gun.transform.forward, out hit))
            {
                if (hit.transform.tag == "Robot")
                {
                    RobotController hitRobotController = hit.transform.GetComponent<RobotController>();
                    RobotInfo enemyInfo = hitRobotController.robot.info;
                    RobotDetectedEvent e = new RobotDetectedEvent(enemyInfo);
                    robotData.events.Add(e);
                }
            }
        }

        private void CheckWallHit()
        {
            if(LastColision != null && LastColision.transform.tag == "Wall")
            {
                WallHitEvent e = new WallHitEvent(LastColision);
                robotData.events.Add(e);
            }
        }

    }
}