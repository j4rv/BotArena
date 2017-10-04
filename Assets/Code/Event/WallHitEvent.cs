using UnityEngine;

namespace BotArena
{
    public class WallHitEvent : IEvent
    {
        Collision wallCollision;

        public WallHitEvent(Collision wallCollision) {
            this.wallCollision = wallCollision;
        }
    }
}
