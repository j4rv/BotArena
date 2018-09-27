using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BotArena {
	public class CompilerComponent : MonoBehaviour {

		[SerializeField]
		private TMP_InputField input;
		[SerializeField]
		private TextMeshProUGUI errors;

		private Compiler<IRobot> compiler;

		void Start () {
			compiler = new Compiler<IRobot>(errors);
		}
		
		void Update () {
			if(Input.GetKeyDown(KeyCode.F1)){
				CompileAndRun();
			}
			if(Input.GetKeyDown(KeyCode.F2)){
				Save();
			}
		}

		void CompileAndRun() {
			string sourceCode = input.text;
			IRobot robot = compiler.CompileAndCreateInstance(sourceCode);
		}

		void Save() {
			string sourceCode = input.text;
			IRobot robot = compiler.CompileAndCreateInstance(sourceCode);
			if(robot != null){
				SourceFiles.Save(robot.name + ".cs", sourceCode);
			}
		}

	}
}
