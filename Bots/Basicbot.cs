using BotArena;
using UnityEngine;

namespace DefaultBots 
{
	public class Basicbot : ITank
	{
		public Basicbot() {
			name = "Basicbot";
		}

		public override void Think() {
			order.AddCommand(Command.ROTATE, 6);
		}

		//No energy management. Oops!
		public override void OnRobotDetected(RobotInfo robotInfo) {
			Attack(info, robotInfo, order);
			order.RemoveCommand(Command.ROTATE);
			order.AddCommand(Command.GOFORWARD, 6);
			order.AddCommand(Command.PROPEL, "this does nothing");
		}
		
		public void Attack(RobotInfo attacker, RobotInfo robot, Order order){
			float power = Vector3.Distance(attacker.position, robot.position) * 0.21f;
			if (attacker.energy >= power* 20) {
				order.AddCommand(Command.ATTACK, power);
			}
		}

	}
}