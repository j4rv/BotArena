using BotArena;

namespace DefaultBots {
	public class Randombot : ITank
	{
		int rot = 6;
		System.Random random = new System.Random();

		public Randombot() {
			name = "Randombot";
		}

		public override void Think() {
			double rng = random.NextDouble();
			if (rng <= 0.01) {
				rot *= -1;
			}

			order.AddCommand(Command.GOFORWARD, 2);
			order.AddCommand(Command.ROTATE, rot);
			order.AddCommand(Command.ROTATEGUN, rot);
		}

		public override void OnRobotDetected(RobotInfo robotInfo) {
			order.Reset();
			order.AddCommand(Command.ATTACK, random.NextDouble() * 5); //Even attacks are random! Praise Rngesus.
		}
	}
}