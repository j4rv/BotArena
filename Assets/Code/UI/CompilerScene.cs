using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace BotArena { 
	public class CompilerScene : MonoBehaviour {

		[SerializeField] private TMP_InputField input;
		[SerializeField] private TextMeshProUGUI errors;
		[SerializeField] private TextMeshProUGUI results;

		private Compiler<IRobot> compiler;

		void Start(){
			compiler = new Compiler<IRobot>(errors);
			string robot = PlayerPrefs.GetString(PlayerPrefKeys.SELECTED_ROBOT_TO_EDIT);
			if(!string.IsNullOrEmpty(robot)){
				try {
					input.text = SourceFiles.Open(robot);
					results.text = $"Loaded robot {robot} successfully.";
				} catch (Exception e) {
					errors.text = e.Message;
					Debug.LogError($"Captured exception while trying to open robot '{robot}': {e.GetBaseException()}");
				}
			}
		}

		void Update () {
			if(Input.GetKeyDown(KeyCode.F1)){
				CompileAndRun();
			}
			if(Input.GetKeyDown(KeyCode.F2)){
				Save();
			}
		}

		public void GoBack(){
			SceneManager.LoadScene(Constants.MAIN_SCENE);      
    }

		void CompileAndRun() {
			string sourceCode = input.text;
			try {
				IRobot robot = compiler.CompileAndCreateInstance(sourceCode);
				results.text = $"Compiled robot {robot} successfully.";
			} catch (CompilationException e) {
				ClearResults();
				Debug.LogWarning($"Tried to compile a robot but got compilation errors: {e.Message}");
			}
		}

		void Save() {
			string sourceCode = input.text;
			try {
				IRobot robot = compiler.CompileAndCreateInstance(sourceCode);
				SourceFiles.Save(robot.name + ".cs", sourceCode);
				results.text = $"Saved robot {robot} successfully.";
			} catch (CompilationException e) {
				ClearResults();
				Debug.LogWarning($"Tried to compile and save a robot but got compilation errors: {e.Message}");
			}
		}

		void ClearResults(){
			results.text = "";
		}

	}
}