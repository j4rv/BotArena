using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BotArena { 
	public class RobotSelector : MonoBehaviour {

		private TMP_Dropdown dropdown;

		void Start () {
			dropdown = gameObject.GetComponent(typeof(TMP_Dropdown)) as TMP_Dropdown;
			foreach (var robotName in SourceFiles.FileNames()){
				var oneOption = new TMP_Dropdown.OptionData();
				oneOption.text = robotName;
				dropdown.options.Add(oneOption);				
			}
			dropdown.RefreshShownValue();
		}
		
		public string GetSelectedRobot () {
			return dropdown.options[dropdown.value].text;
		}
		
	}
}