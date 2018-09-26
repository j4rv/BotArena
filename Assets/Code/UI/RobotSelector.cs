using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BotArena { 
	public class RobotSelector : MonoBehaviour {

		[SerializeField] private TMP_Dropdown robotSelectorPrefab;

		void Start () {
			robotSelectorPrefab = gameObject.GetComponent(typeof(TMP_Dropdown)) as TMP_Dropdown;
			foreach (var robotName in SourceFiles.FileNames()){
				var oneOption = new TMP_Dropdown.OptionData();
				oneOption.text = robotName;
				robotSelectorPrefab.options.Add(oneOption);				
			}
			robotSelectorPrefab.RefreshShownValue();
		}
		
	}
}