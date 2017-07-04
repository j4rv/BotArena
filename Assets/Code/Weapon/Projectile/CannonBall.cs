using System;
using UnityEngine;

namespace BotArena
{
    class CannonBall : IProjectileController
    {
        protected override float GetDamage()
        {
            return 10f;
        }
    }
}
