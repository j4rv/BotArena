using UnityEngine;

namespace BotArena
{
    abstract class IProjectileController : MonoBehaviour
    {
        static readonly float MINIMUM_DAMAGE = 1;
        static readonly float MAXIMUM_DAMAGE = 80; //Let's limit one hit kills

        protected IWeaponController weapon;

        public void SetWeapon(IWeaponController weapon) {
            if (weapon == null) {
                this.weapon = weapon;
            }
        }

        protected abstract float GetDamage();

        public virtual void RobotHit(RobotController hit, float impactVelocity) {
            float damage = Mathf.Clamp(impactVelocity * GetDamage(), MINIMUM_DAMAGE, MAXIMUM_DAMAGE);
            hit.TakeDamage(damage);
        }

        void OnCollisionEnter(Collision collision) {
            if (collision.transform.tag == Tags.ROBOT) {
                RobotController robot = collision.gameObject.GetComponent<RobotController>();
                RobotHit(robot, collision.relativeVelocity.magnitude);
            }
            //TODO: Instantiate some kind of collision effect
            Destroy(gameObject);
        }

    }
}
