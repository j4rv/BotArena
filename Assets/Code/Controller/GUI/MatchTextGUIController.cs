using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BotArena { 
    internal class MatchTextGUIController : MonoBehaviour {

        new Text guiText;

        void Start () {
            //MatchText only changes when a match ends, so we only need to set it at start
            guiText = GetComponent<Text>();
            guiText.text = GetMatchText();
        }

        private string GetMatchText() {
            string res = "";
            MatchManager.Instance().
                robotsMatchData.
                ForEach(
                d => res += d.ToString() + "\n");
            res += string.Format("Round: {0}", MatchManager.Instance().round);
            return res;
        }
    }
}