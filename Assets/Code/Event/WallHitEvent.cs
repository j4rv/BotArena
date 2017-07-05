using UnityEngine;

namespace BotArena
{
    public class WallHitEvent : Event
    {
        public Collision wallCollision;

        public WallHitEvent(Collision wallCollision)
        {
            this.wallCollision = wallCollision;
        }
    }
}
