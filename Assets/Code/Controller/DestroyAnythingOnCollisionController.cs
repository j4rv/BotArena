
using UnityEngine;
namespace BotArena
{
    internal class DestroyAnythingOnCollisionController : MonoBehaviour
    {

        private void OnCollisionEnter(Collision collision) {
            if (collision.transform.tag == Tags.ROBOT)
                collision.transform.GetComponent<RobotController>().TakeDamage(10000);
            else Destroy(collision.gameObject);
        }

    }
}