using UnityEngine;

namespace BotArena
{
    public class TurnManager : MonoBehaviour
    {

        [SerializeField]
        private long timeSteps;
        [SerializeField]
        private int currentTurn;

        private static TurnManager instance;

        private static TurnManager Get() {
            if (instance == null)
                instance = (TurnManager) FindObjectOfType(typeof(TurnManager));
            return instance;
        }

        void FixedUpdate() {
            if (IsTurnUpdate()) {
                currentTurn++;
            }
            timeSteps++;
        }

        public static int GetCurrentTurn() {
            return Get().currentTurn;
        }

        public static bool IsTurnUpdate() {
            //A turn every two timesteps
            return Get().timeSteps % 2 == 0;
        }
    }
}