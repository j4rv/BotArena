using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace BotArena { 
	public class MatchConfigUI : MonoBehaviour {

		[SerializeField] private Slider botsAmount;
		[SerializeField] private TMP_Dropdown robotSelectorPrefab;
		[SerializeField] private GameObject dropdownsParent;

		private TMP_Dropdown[] robotSelectors;

		// Use this for initialization
		void Start () {
			robotSelectors = new TMP_Dropdown[Constants.MAX_BOTS_PER_MATCH];
			for(int i = 0; i < Constants.MAX_BOTS_PER_MATCH; i++){
				TMP_Dropdown aRobotSelector = Instantiate(robotSelectorPrefab);
				aRobotSelector.transform.SetParent(dropdownsParent.transform);
				robotSelectors[i] = aRobotSelector;
			}
			UpdateDropdownsParent();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void UpdateDropdownsParent(){
			for(int i = 0; i < Constants.MAX_BOTS_PER_MATCH; i++){
				bool enableSelector = i < botsAmount.value;
				robotSelectors[i].gameObject.SetActive(enableSelector);
			}
		}

	}
}
