using BotArena;

namespace DefaultBots {
    public class Roambot : ITank{
        
        float rotateAngles;
        float speed = 5;
        
        public Roambot() {
            name = "Roambot";
        }

        public override void Think() {
            // high speed cleaning!
            order.AddCommand(Command.GOFORWARD, speed);

            // rotate if you need to!
            if(rotateAngles > 0){
                float rotation;
                if(rotateAngles >= 4){
                    rotation = 4;
                } else {
                    rotation = rotateAngles;
                }
                order.RemoveCommand(Command.GOFORWARD);
                order.AddCommand(Command.ROTATE, rotation);
                rotateAngles -= rotation;
            }
        }

        public override void OnWallHit(UnityEngine.Collision wallCollision) {
            rotateAngles = 40;
        }

        public override void OnDeath(IRobot deadRobot) {
            base.OnDeath(deadRobot);
            if (deadRobot == this){
                // Go faster next time!
                speed += 1;
            }
        }
    }
}
