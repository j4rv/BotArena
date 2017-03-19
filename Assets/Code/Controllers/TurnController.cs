using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BotArena
{
    public class TurnController : MonoBehaviour
    {

        [SerializeField]
        private long timeSteps;
        [SerializeField]
        private int currentTurn;

        private static TurnController instance;

        private static TurnController Get()
        {
            if (instance == null)
                instance = (TurnController)FindObjectOfType(typeof(TurnController));
            return instance;
        }

        void FixedUpdate()
        {
            if (IsTurnUpdate())
            {
                currentTurn++;
            }
            timeSteps++;
        }

        public static int GetCurrentTurn()
        {
            return Get().currentTurn;
        }

        public static bool IsTurnUpdate()
        {
            //A turn every two timesteps
            return Get().timeSteps % 2 == 0;
        }
    }
}