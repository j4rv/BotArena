using BotArena;
using UnityEngine;

namespace DefaultBots {
    public class Searcher : ITank{
        public Searcher() {
            name = "Searcher";
        }

        public override void Think() {
            order.AddCommand(Command.ROTATEGUN, 6);
        }

        public override void OnRobotDetected(RobotInfo robotInfo) {
						var desiredRot = info.position - robotInfo.position;
						var gunForward = Quaternion.Euler(info.gunRotation) * Vector3.forward;
						var angle = Vector3.Angle(gunForward, desiredRot);
						
						Debug.Log(gunForward.ToString());
						Debug.Log(desiredRot.ToString());
						Debug.Log(angle.ToString());
						Debug.Log("-----------------------------");
            Attack(info, robotInfo, order);
            order.AddCommand(Command.ROTATEGUN, angle);
        }
	
		public void Attack(RobotInfo attacker, RobotInfo robot, Order order){
			float power = Vector3.Distance(attacker.position, robot.position) * 0.21f;
			if (attacker.energy >= power* 20) {
				order.AddCommand(Command.ATTACK, power);
			}
		}
    }
}
