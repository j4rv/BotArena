using UnityEngine;
using UnityEngine.UI;

namespace BotArena { 
    class MatchTextGUIController : MonoBehaviour {

        Text matchGUIText;
        MatchManager matchManager;

        void Start () {
            matchManager = FindObjectOfType<MatchManager>();
            matchGUIText = GetComponent<Text>();
        }

        void FixedUpdate(){
            matchGUIText.text = GetMatchText();
        }

        string GetMatchText() {
            string res = "";
            DataManager.GetRobotsMatchData().ForEach( d =>
                res += d.ToString() + "\n");
            res += string.Format("Round: {0}", matchManager.round);
            return res;
        }
    }
}