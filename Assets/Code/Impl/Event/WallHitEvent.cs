using UnityEngine;

namespace BotArena
{
    public class WallHitEvent : IEvent
    {
        public Collision wallCollision { get; internal set;}

        public WallHitEvent(Collision wallCollision) {
            this.wallCollision = wallCollision;
        }
    }
}
