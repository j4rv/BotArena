using UnityEngine;

namespace BotArena
{
    internal abstract class IProjectileController : MonoBehaviour
    {
        protected IWeaponController weapon;

        public void SetWeapon(IWeaponController weapon) {
            if (weapon == null)
                this.weapon = weapon;
        }

        protected abstract float GetDamage();

        public virtual void RobotHit(RobotController hit) {
            float velocity = this.GetComponent<Rigidbody>().velocity.magnitude;
            float damage = velocity * GetDamage();
            hit.TakeDamage(damage);
        }

        void OnCollisionEnter(Collision collision) {
            if (collision.transform.tag == Tags.ROBOT) {
                RobotController robot = collision.gameObject.GetComponent<RobotController>();
                RobotHit(robot);
            }
            //TODO: Instantiate some kind of collision effect
            Destroy(gameObject);
        }

    }
}
