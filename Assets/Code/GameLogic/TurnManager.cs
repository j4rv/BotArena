using UnityEngine;

namespace BotArena
{
    class TurnManager : MonoBehaviour
    {
        //A turn every two timesteps, TODO: Make it configurable
        static readonly int TIMESTEPS_PER_TURN = 2;

        [SerializeField]
        long timeSteps;
        [SerializeField]
        int currentTurn;

        static TurnManager instance;

        static TurnManager Get() {
            if (instance == null) {
                instance = (TurnManager)FindObjectOfType(typeof(TurnManager));
            }
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