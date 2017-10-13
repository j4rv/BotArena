using UnityEngine;

namespace BotArena
{
    class TurnManager : MonoBehaviour
    {
        //A turn every two timesteps, TODO: Make it configurable
        static readonly int TIMESTEPS_PER_TURN = GameConfig.instance.timestepsPerTurn;

        [SerializeField]
        long timeSteps;
        [SerializeField]
        int currentTurn;

        static TurnManager instance;


        void Awake(){
            instance = (TurnManager)FindObjectOfType(typeof(TurnManager));
        }

        void FixedUpdate() {
            if (IsTurnUpdate()) {
                currentTurn++;
            }
            timeSteps++;
        }


        public static void ResetTurns() {
            instance.currentTurn = 0;
        }

        public static int GetCurrentTurn() {
            return instance.currentTurn;
        }

        public static bool IsTurnUpdate() {
            return instance.timeSteps % TIMESTEPS_PER_TURN == 0;
        }
    }
}