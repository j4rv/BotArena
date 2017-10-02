using System.Collections.Generic;
using UnityEngine;


namespace BotArena
{
    class RobotController : MonoBehaviour
    {
        public IRobot robot;
        RobotThreadSharedData robotThreadSharedData;

        [SerializeField]
        public IWeaponController weapon;
        [SerializeField]
        public BodyController body;
        [SerializeField]
        Transform radar;

        public float health;
        public float energy;

        public void Init(IRobot robot) {
            this.robot = robot;
            health = robot.maxHealth;
            energy = robot.maxEnergy;

            robotThreadSharedData = new RobotThreadSharedData();
        }

        //              UNITY METHODS

        readonly LinkedList<Collision> collisions = new LinkedList<Collision>();
        void OnCollisionEnter(Collision collision) {
            collisions.AddLast(collision);

            if (collision.transform.tag == Tags.WALL) {
                wallHit = collision;
            }
        }

        //              ROBOT METHODS

        public bool IsAlive() {
            return health > 0;
        }

        public float GetEnergy() {
            return energy;
        }

        internal void ConsumeEnergy(float consumption) {
            if (consumption >= 0) {
                energy -= consumption;
            }
        }

        public float GetAgility() {
            return robot.agility;
        }

        internal void TakeDamage(float damage) {
            if (damage >= 0) {
                health -= damage;
            }
        }

        readonly Dictionary<Command, int> commandToTurn = new Dictionary<Command, int>();
        public int GetLastTurnExecuted(Command cmd) {
            int result;
            commandToTurn.TryGetValue(cmd, out result);
            return result;
        }

		/// <summary>
		/// Finds all robot instances, except this robotController.
		/// </summary>
        void FindEnemies() {
            HashSet<RobotInfo> res = new HashSet<RobotInfo>();
            GameObject[] robots = GameObject.FindGameObjectsWithTag(Tags.ROBOT);

            foreach (GameObject r in robots) {
				if (r != gameObject){
                	res.Add(r.GetComponent<RobotController>().robot.info);
				}
            }

			robot.SetEnemies(res);
        }

        //              TURN METHODS

        internal void TurnUpdate() {
            if (IsAlive()) {
                TurnUpdateStats();
                UpdateRobotInfo();
                ExecuteLastOrder();

                CreateOrderForNextTurn();
                NewTurnEventChecks();
				FindEnemies();
                RunNewTurnOnRobotThread();
            } else {
                CheckDeath();
            }
        }

        void TurnUpdateStats() {
            energy = Mathf.Clamp(energy + robot.energyRecoveryRate, 0, robot.maxEnergy);    //Recover some energy
            // TODO: Heal a bit over time? Stop healing for x turns after receiving damage?
        }

        void UpdateRobotInfo() {
            Vector3 pos = transform.position;
            Vector3 rot = transform.rotation.eulerAngles;
            Vector3 gunRot = weapon.transform.rotation.eulerAngles;
            robot.UpdateInfo(health, energy, GetAgility(), pos, rot, gunRot);
        }

        void ExecuteLastOrder() {
            int currentTurn = TurnManager.GetCurrentTurn();
            int lastTurn = currentTurn - 1;
            Order lastOrder = robotThreadSharedData.GetLastOrder();

            if (lastOrder != null
                && lastOrder.GetTurn() == lastTurn      //If the robot missed the last turn, this would return false
                && lastOrder.IsExecuted() == false) {
                List<ICommand> commands = lastOrder.GetCommands();

                foreach (ICommand cmd in commands) {
                    //If the command was executed, we'll update the command-turn dictionary
                    if (cmd.Call()) {
                        commandToTurn.Remove(cmd.GetCommand());
                        commandToTurn.Add(cmd.GetCommand(), currentTurn);
                    }
                }
                lastOrder.Executed();
            }
        }

        void CreateOrderForNextTurn() {
            int turn = TurnManager.GetCurrentTurn();
            Order order = new Order(this, turn);
            robotThreadSharedData.orders.Add(order);
        }

        void RunNewTurnOnRobotThread() {
            //We try to start a new turn. If we can't, we'll apply a damage penalty
            bool turnLost = !robot.StartTurn(robotThreadSharedData);

			if (turnLost) {
				TakeDamage(5);
			}
        }


        //              EVENT CHECKERS

        void NewTurnEventChecks() {
            //Clear the previous turn's events and collisions
            robotThreadSharedData.events.Clear();
            collisions.Clear();

            CheckRobotAhead();
            CheckWallHit();
        }

        void CheckRobotAhead() {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, radar.transform.forward, out hit)) {
                if (hit.transform.tag == Tags.ROBOT) {
                    RobotController hitRobotController = hit.transform.GetComponent<RobotController>();
                    RobotInfo enemyInfo = hitRobotController.robot.info;
                    RobotDetectedEvent e = new RobotDetectedEvent(enemyInfo);
                    robotThreadSharedData.events.Add(e);
                }
            }
        }

        Collision wallHit;
        void CheckWallHit() {
            if (wallHit != null) {
                WallHitEvent e = new WallHitEvent(wallHit);
                robotThreadSharedData.events.Add(e);
            }

            wallHit = null;
        }

        void CheckDeath() {
            DeathEvent death = new DeathEvent(robot);
            robotThreadSharedData.events.Add(death);
            UpdateRobotInfo();
            //TODO: Add some kind of visuals
            Destroy(gameObject);
            Destroy(this);
        }

    }
}