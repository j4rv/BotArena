using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

namespace BotArena { 
	public class MatchConfigScene : MonoBehaviour {

		[SerializeField] private Slider botsAmount;
		[SerializeField] private RobotSelectorController robotSelectorPrefab;
		[SerializeField] private TMP_Text robotSelectorAmount;
		[SerializeField] private TMP_InputField roundsInput;
		[SerializeField] private GameObject dropdownsParent;

		private RobotSelectorController[] robotSelectors;

		void Start () {
			robotSelectors = new RobotSelectorController[Constants.MAX_BOTS_PER_MATCH];
			for(int i = 0; i < Constants.MAX_BOTS_PER_MATCH; i++){
				RobotSelectorController aRobotSelector = Instantiate(robotSelectorPrefab);
				aRobotSelector.transform.SetParent(dropdownsParent.transform, false);
				robotSelectors[i] = aRobotSelector;
			}
			UpdateDropdownsParent();
		}

		public void StartMatch(){
			List<string> selectedRobots = robotSelectors
				.Where(selector => selector.gameObject.activeInHierarchy)
				.Select(selector => selector.GetSelectedRobot())
				.ToList();
			DataManager.SetRobots(selectedRobots);
			DataManager.SetRounds(Int32.Parse(roundsInput.text));
			SceneManager.LoadScene(Constants.MATCH_SCENE);
		}

		public void UpdateDropdownsParent(){
			for(int i = 0; i < Constants.MAX_BOTS_PER_MATCH; i++){
				bool enableSelector = i < botsAmount.value;
				robotSelectors[i].gameObject.SetActive(enableSelector);
			}
			robotSelectorAmount.text = botsAmount.value.ToString();
		}

	}
}
