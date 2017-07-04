﻿using System;
using UnityEngine;

namespace BotArena
{
    public abstract class IProjectileController : MonoBehaviour
    {
        protected IWeaponController weapon;

        public void SetWeapon(IWeaponController weapon)
        {
            if(weapon == null)
                this.weapon = weapon;
        }

        protected abstract float GetDamage();
        
        public virtual void RobotHit(RobotController hit)
        {
            float velocity = GetComponent<Rigidbody>().velocity.magnitude;
            hit.TakeDamage(velocity * GetDamage());
        }
        
        void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == Tags.ROBOT)
            {
                RobotController robot = collision.gameObject.GetComponent<RobotController>();
                RobotHit(robot);
            }
        }

    }
}
