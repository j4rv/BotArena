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

        private void UpdateRobot()
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

        private Dictionary<Command, int> commandsTurnDictionary = new Dictionary<Command, int>();
        public int GetLastTurnExecuted(Command cmd)
        {
            int result;
            commandsTurnDictionary.TryGetValue(cmd, out result);
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

        private void TurnUpdate()
        {
            energy = Mathf.Clamp(energy + 1, 0, maxEnergy);    //Recover some energy

            UpdateRobot();
            ExecuteLastOrder();

            int turn = TurnManager.GetCurrentTurn();
            Order order = new Order(this, turn);
            threadData.orders.Add(order);
            threadData.events.Clear();
            collisions.Clear();

            TurnCheck();

            NewTurn();
        }

        private void ExecuteLastOrder()
        {
            int currentTurn = TurnManager.GetCurrentTurn();
            int lastTurn = currentTurn - 1;
            Order lastOrder = threadData.GetLastOrder();

            if (lastOrder != null
                && lastOrder.GetTurn() == lastTurn
                && lastOrder.IsExecuted() == false)
            {
                lastOrder.AddCommand(Command.ATTACK, 2); //For debug
                List<ICommand> commands = lastOrder.GetCommands();

                foreach (ICommand cmd in commands)
                {
                    //If the command was executed, we'll update the dictionary
                    if (cmd.Call()) { 
                        commandsTurnDictionary.Remove(cmd.GetCommand());
                        commandsTurnDictionary.Add(cmd.GetCommand(), currentTurn);
                    }
                }
                lastOrder.Executed();
            }
        }

        private void NewTurn()
        {
            bool turnLost = !robot.StartTurn(threadData);

            if (turnLost)
                TakeDamage(10);
        }


        //              EVENT CHECKERS

        private void TurnCheck()
        {
            CheckRobotAhead();
            CheckWallHit();
            CheckDeath();
        }

        private void CheckRobotAhead()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, weapon.transform.forward, out hit))
            {
                if (hit.transform.tag == Tags.ROBOT)
                {
                    RobotController hitRobotController = hit.transform.GetComponent<RobotController>();
                    RobotInfo enemyInfo = hitRobotController.robot.info;
                    RobotDetectedEvent e = new RobotDetectedEvent(enemyInfo);
                    threadData.events.Add(e);
                }
            }
        }

        private Collision wallHit;
        private void CheckWallHit()
        {
            if (wallHit != null)
            {
                WallHitEvent e = new WallHitEvent(wallHit);
                threadData.events.Add(e);
            }

            wallHit = null;
        }

        private void CheckDeath()
        {
            if (health <= 0)
            {
                DeathEvent death = new DeathEvent(this);
                threadData.events.Add(death);
                //Add some kind of visuals
                Destroy(gameObject);
            }
        }

    }
}