using UnityEngine;

namespace BotArena
{
    public class WallHitEvent : IEvent
    {
        public Collision wallCollision;

        public WallHitEvent(Collision wallCollision) {
            this.wallCollision = wallCollision;
        }
    }
}
