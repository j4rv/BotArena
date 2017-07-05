using System.Collections.Generic;
using UnityEngine;


namespace BotArena
{
    internal class RobotController : MonoBehaviour
    {
        public IRobot robot;
        private RobotThreadSharedData robotThreadSharedData;

        [SerializeField]
        public IWeaponController weapon;
        [SerializeField]
        public BodyController body;
        [SerializeField]
        private Transform radar;

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

            robot = DllUtil.LoadRobotFromDll(dllPath);
            transform.name = robot.GetName();

            robotThreadSharedData = new RobotThreadSharedData();
        }

        void FixedUpdate()
        {
            if (TurnManager.IsTurnUpdate())
            {
                TurnUpdate();
            }
        }

        private LinkedList<Collision> collisions = new LinkedList<Collision>();
        void OnCollisionEnter(Collision collision)
        {
            collisions.AddLast(collision);

            if (collision.transform.tag == Tags.WALL)
            {
                wallHit = collision;
            }
        }


        //              ROBOT METHODS

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

        private Dictionary<Command, int> commandToTurnDictionary = new Dictionary<Command, int>();
        public int GetLastTurnExecuted(Command cmd)
        {
            int result;
            commandToTurnDictionary.TryGetValue(cmd, out result);
            return result;
        }

        private HashSet<IRobot> FindEnemies()
        {
            HashSet<IRobot> res = new HashSet<IRobot>();
            GameObject[] robots = GameObject.FindGameObjectsWithTag(Tags.ROBOT);

            foreach (GameObject robot in robots)
            {
                res.Add(robot.GetComponent<IRobot>());
            }

            return res;
        }

        //              TURN METHODS

        internal void TurnUpdate()
        {
            TurnUpdateProperties();
            UpdateRobotInfo();
            ExecuteLastOrder();

            CreateOrderForNextTurn();
            NewTurnEventChecks();
            RunNewTurnOnRobotThreat();
        }

        private void TurnUpdateProperties()
        {
            energy = Mathf.Clamp(energy + 1, 0, maxEnergy);    //Recover some energy
            //TODO: Heal a bit over time? Stop healing for x turns after receiving damage?
        }

        private void UpdateRobotInfo()
        {
            Vector3 pos = transform.position;
            Vector3 rot = transform.rotation.eulerAngles;
            Vector3 gunRot = weapon.transform.rotation.eulerAngles;
            robot.UpdateInfo(health, energy, agility, pos, rot, gunRot);
        }

        private void ExecuteLastOrder()
        {
            int currentTurn = TurnManager.GetCurrentTurn();
            int lastTurn = currentTurn - 1;
            Order lastOrder = robotThreadSharedData.GetLastOrder();

            if (lastOrder != null
                && lastOrder.GetTurn() == lastTurn      //If the robot missed the last turn, this would return false
                && lastOrder.IsExecuted() == false)
            {
                List<ICommand> commands = lastOrder.GetCommands();

                foreach (ICommand cmd in commands)
                {
                    //If the command was executed, we'll update the command-turn dictionary
                    if (cmd.Call()) { 
                        commandToTurnDictionary.Remove(cmd.GetCommand());
                        commandToTurnDictionary.Add(cmd.GetCommand(), currentTurn);
                    }
                }
                lastOrder.Executed();
            }
        }

        private void CreateOrderForNextTurn()
        {
            int turn = TurnManager.GetCurrentTurn();
            Order order = new Order(this, turn);
            robotThreadSharedData.orders.Add(order);
        }

        private void RunNewTurnOnRobotThreat()
        {
            //We try to start a new turn. If we can't, we'll apply a damage penalty
            bool turnLost = !robot.StartTurn(robotThreadSharedData);

            if (turnLost)
                TakeDamage(10);
        }


        //              EVENT CHECKERS

        private void NewTurnEventChecks()
        {
            //Clear the previous turn's events and collisions
            robotThreadSharedData.events.Clear();
            collisions.Clear();

            CheckRobotAhead();
            CheckWallHit();
            CheckDeath();
        }

        private void CheckRobotAhead()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, radar.transform.forward, out hit))
            {
                if (hit.transform.tag == Tags.ROBOT)
                {
                    RobotController hitRobotController = hit.transform.GetComponent<RobotController>();
                    RobotInfo enemyInfo = hitRobotController.robot.info;
                    RobotDetectedEvent e = new RobotDetectedEvent(enemyInfo);
                    robotThreadSharedData.events.Add(e);
                }
            }
        }

        private Collision wallHit;
        private void CheckWallHit()
        {
            if (wallHit != null)
            {
                WallHitEvent e = new WallHitEvent(wallHit);
                robotThreadSharedData.events.Add(e);
            }

            wallHit = null;
        }

        private void CheckDeath()
        {
            if (health <= 0)
            {
                DeathEvent death = new DeathEvent(robot);
                robotThreadSharedData.events.Add(death);
                //TODO: Add some kind of visuals
                Destroy(gameObject);
            }
        }

    }
}