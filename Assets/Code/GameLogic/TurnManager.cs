using UnityEngine;

namespace BotArena
{
    public class TurnManager : MonoBehaviour
    {
        //A turn every two timesteps, TODO: Make it configurable
        private static readonly int TIMESTEPS_PER_TURN = 2;

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
            return Get().timeSteps % TIMESTEPS_PER_TURN == 0;
        }
    }
}