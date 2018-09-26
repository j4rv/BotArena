using BotArena;

namespace DefaultBots
{
    public class Searcher : ITank
    {
        public Searcher() {
            name = "Searcher";
        }

        public override void Think() {
            order.AddCommand(Command.ROTATEGUN, 6);
        }

        public override void OnRobotDetected(RobotInfo robotInfo) {
            Attack(info, robotInfo, order);
            order.AddCommand(Command.ROTATEGUN, 0);
        }
	
		public void Attack(RobotInfo attacker, RobotInfo robot, Order order){
			float power = Vector3.Distance(attacker.position, robot.position) * 0.21f;
			if (attacker.energy >= power* 20) {
				order.AddCommand(Command.ATTACK, power);
			}
		}

    }
}
