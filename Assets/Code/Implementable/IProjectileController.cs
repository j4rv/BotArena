using UnityEngine;

namespace BotArena
{
    abstract class IProjectileController : MonoBehaviour
    {
        static readonly float MINIMUM_DAMAGE = 1;
        static readonly float MAXIMUM_DAMAGE = 80; //Let's limit one hit kills

        protected IWeaponController weapon;
        protected bool hasHitted;

        public void SetWeapon(IWeaponController weapon) {
            if (weapon == null) {
                this.weapon = weapon;
            }
        }

        protected abstract float GetDamage();
        protected abstract void OnHit();

        public virtual void RobotHit(RobotController hit, float impactVelocity) {
            float damage = Mathf.Clamp(impactVelocity * GetDamage(), MINIMUM_DAMAGE, MAXIMUM_DAMAGE);
            hit.TakeDamage(damage);
            // Debug.Log(string.Format("Dealing {0} damage to {1}.", damage, hit.robot.name));
        }

        void OnCollisionEnter(Collision collision) {
            if (!hasHitted && collision.transform.tag == Tags.ROBOT) {
                RobotController robot = collision.gameObject.GetComponent<RobotController>();
                RobotHit(robot, collision.relativeVelocity.magnitude);
            }
            OnHit();
            hasHitted = true;
        }

    }
}
