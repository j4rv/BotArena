using UnityEngine;

namespace BotArena {
    class DestroyAnythingOnCollisionController : MonoBehaviour {

        void OnCollisionEnter(Collision collision) {
            if (collision.transform.tag == Tags.ROBOT) {
                // TODO This should get replaced with a Destroy function.
                collision.transform.GetComponent<RobotController>().ChangeHealth(-10000);
            } else {
                Destroy(collision.gameObject);
            }
        }

    }
}