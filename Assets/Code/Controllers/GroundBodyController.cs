using UnityEngine;

namespace BotArena
{
    public class GroundBodyController : BodyController
    {
        private bool onTheGround;

        void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Ground")
            {
                onTheGround = true;
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (collision.transform.tag == "Ground")
            {
                onTheGround = false;
            }
        }

        public override bool CanMove()
        {
            return onTheGround;
        }
    }
}
