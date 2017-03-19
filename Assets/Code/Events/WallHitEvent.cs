using UnityEngine;

namespace BotArena
{
    public class WallHitEvent : Event
    {
        public Collision wallCollision;

        public WallHitEvent(Collision wallCollision)
        {
            Debug.Log("Wallhit");
            this.wallCollision = wallCollision;
        }
    }
}
