using System.Collections.Generic;
using UnityEngine;


namespace BotArena
{
    public class RobotController : MonoBehaviour
    {
        public IRobot robot;
        public IWeaponController weapon;
        public BodyController body;
        private RobotThreadSharedData threadData;

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

            threadData = new RobotThreadSharedData();
            robot = DllUtil.LoadRobotFromDll(dllPath, this);
            transform.name = robot.name;
        }

        void FixedUpdate()
        {
            if (TurnController.IsTurnUpdate())
            {
                TurnUpdate();
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
            Vector3 gunRot = weapon.transform.rotation.eulerAngles;
            robot.UpdateInfo(health, energy, agility, pos, rot, gunRot);
        }

        public float GetEnergy()
        {
            return energy;
        }

        internal void ConsumeEnergy(float consumption)
        {
            if (consumption >= 0)
                energy -= consumption;
        }

        public float GetAgility()
        {
            return agility;
        }

        internal void TakeDamage(float damage)
        {
            if (damage >= 0)
                health -= damage;
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
                    
        //              TURN METHODS

        private void TurnUpdate()
        {
            bool turnLost = false;
            energy = Mathf.Clamp(energy + 1, 0, maxEnergy);    //Recover some energy

            UpdateRobot();
            ExecuteLastOrder();

            int turn = TurnController.GetCurrentTurn();
            Order order = new Order(this, turn);
            threadData.orders.Add(order);
            threadData.events.Clear();

            CheckRobotAhead();
            CheckWallHit();

            turnLost = ! robot.StartTurn(threadData);
            if (turnLost)
                TakeDamage(10);  
        }

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

            if (Physics.Raycast(transform.position, weapon.transform.forward, out hit))
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