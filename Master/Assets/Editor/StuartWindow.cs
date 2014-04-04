using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

class StuartWindow : EditorWindow
{
	private string[] options = new string[] {"0.1", "0.2", "0.3"};
	private int index = 0;

	private AstarAI_light AIScript;

	[MenuItem ("Stuart/Tools")]
	
	static void Init ()
	{
		StuartWindow window = (StuartWindow)EditorWindow.GetWindow (typeof (StuartWindow));
	}
	
	void OnGUI() {
		//index = EditorGUILayout.Popup(index, options);
/*
		if (GUILayout.Button ("Apply")) {
			Apply();

		}
*/
		if (GUILayout.Button ("Stop Stuart !!")) {
			StopStuart();
			
		}
		if (GUILayout.Button ("Restart Stuart")) {
			RestartStuart();
			
		}
	}


	void Start() {
		AIScript = GameObject.FindGameObjectWithTag ("Car").GetComponent<AstarAI_light> ();
	}


	void Apply()
	{

		float newZ = float.Parse(options[index]);
		
		if (Selection.transforms.Length == 0) {
			EditorUtility.DisplayDialog("No Selected Transforms", "To use ApplyZ you have to select one or more Transform", "Ok");

				}

		foreach (Transform oneTrans in Selection.transforms) {
			Vector3 pos = oneTrans.position;
			oneTrans.position = new Vector3(pos.x, pos.y, newZ);
		}
	}


	void StopStuart() {
		GameObject.FindGameObjectWithTag ("Car").GetComponent<AstarAI_light> ().emergencyStop = true;
		Debug.Log ("Emergency stop !");
	}
	void RestartStuart() {
		GameObject.FindGameObjectWithTag ("Car").GetComponent<AstarAI_light> ().emergencyStop = false;
		Debug.Log ("Emergency stop !");
	}
}