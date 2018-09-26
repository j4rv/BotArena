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

        static readonly float TURN_LOST_PENALTY = GameConfig.instance.lostTurnPenalty;

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

        internal void ChangeHealth(float change){
            health += change;
        }

        internal void TakeDamage(float damage) {
            SoundManager.Play(transform.position, "hit", 1);
            if (damage >= 0) {
                health -= damage;
            }
        }

        void UpdateRobotInfo() {
        	Vector3 pos = transform.position;
        	Vector3 rot = transform.rotation.eulerAngles;
        	Vector3 gunRot = weapon.transform.rotation.eulerAngles;
        	robot.UpdateInfo(health, energy, pos, rot, gunRot);
        }

        readonly Dictionary<Command, int> commandToTurn = new Dictionary<Command, int>();
        public int GetLastTurnExecuted(Command cmd) {
            int result;
            commandToTurn.TryGetValue(cmd, out result);
            return result;
        }

		/// <summary>
		/// Finds all alive robot instances, except this robotController.
		/// </summary>
        LinkedList<RobotController> FindAliveEnemies() {
            LinkedList<RobotController> robots = new LinkedList<RobotController>(FindObjectsOfType<RobotController>());
            robots.Remove(this);
            return robots;
        }

        void UpdateEnemies(){
            HashSet<RobotInfo> res = new HashSet<RobotInfo>();
            foreach (RobotController r in FindAliveEnemies()){
                res.Add(r.robot.info);
            }
            robot.SetEnemies(res);
        }

        //              TURN METHODS

        internal void ExecuteTurn() {
            if (IsAlive()) {
                RecoverStats();
                UpdateRobotInfo();
                ExecuteLastOrder();

                CreateOrderForNextTurn();
                NewTurnEventChecks();
                UpdateEnemies();
                RunNewTurnOnRobotThread();
            } else {
                Death();
            }
        }

        void RecoverStats() {
            energy = Mathf.Clamp(energy + robot.energyRecoveryRate, 0, robot.maxEnergy);    //Recover some energy
            // TODO: Heal a bit over time? Stop healing for x turns after receiving damage?
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
                ChangeHealth(-TURN_LOST_PENALTY);
                SoundManager.Play(transform.position, "penalty", 1);
			}
        }


        //              EVENTS

        internal void AddEvent(IEvent e){
            robotThreadSharedData.events.Add(e);
        }

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
                    AddEvent(e);
                }
            }
        }

        Collision wallHit;
        void CheckWallHit() {
            if (wallHit != null) {
                ContactPoint contactPoint = wallHit.contacts[0];
                SoundManager.Play(transform.position, "blop", 1);
                ParticleEffectFactory.Summon("StarsHit", 1, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
                WallHitEvent e = new WallHitEvent(wallHit);
                AddEvent(e);
            }

            wallHit = null;
        }

        void Death() {
            UpdateRobotInfo();
            robot.OnDeath(robot);
            DeathEvent death = new DeathEvent(robot);
            foreach(RobotController otherRobot in FindAliveEnemies()){
                otherRobot.AddEvent(death);
            }

            SoundManager.Play(transform.position, "explosion", 2f);
            ParticleEffectFactory.Summon("Explosion", 3f, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }
}