using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BotArena { 
	public class RobotSelectorController : MonoBehaviour {

		private TMP_Dropdown dropdown;
		private static readonly TMP_Dropdown.OptionData NO_OPTIONS = new TMP_Dropdown.OptionData();

		void Awake () {
			NO_OPTIONS.text = "No bots found on bots folder";

			dropdown = gameObject.GetComponent(typeof(TMP_Dropdown)) as TMP_Dropdown;
			foreach (var robotName in SourceFiles.FileNames()){
				var oneOption = new TMP_Dropdown.OptionData();
				oneOption.text = robotName;
				dropdown.options.Add(oneOption);
			}
			if(HasNoOptions()){
				dropdown.interactable = false;
				dropdown.options.Add(NO_OPTIONS);
			}
			dropdown.RefreshShownValue();
		}

		public bool HasNoOptions(){
			return dropdown.options.Count == 0 || 
				(dropdown.options.Count == 1 && dropdown.options[0] == NO_OPTIONS);
		}
		
		public string GetSelectedRobot () {
			return dropdown.options[dropdown.value].text;
		}
		
	}
}