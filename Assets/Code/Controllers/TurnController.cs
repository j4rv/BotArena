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
        private int turnSteps;
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

        public static bool IsTurnUpdate()
        {
            bool res = false;

            if(Get().turnSteps <= 0)
            {
                res = true;
            } else {
                res = (Get().timeSteps % Get().turnSteps) == 0;
            }

            return res;
        }
    }
}