using UnityEngine;
using System.Collections;

public class GetAllInteractors : MonoBehaviour {

	public string cubesTag = "Cubes";
	public GameObject[] objList;
	//gros nombre interactors
	private UILabel numberInteractors;


	void Start() {

		numberInteractors = this.GetComponent<UILabel>();
		
		objList = GameObject.FindGameObjectsWithTag(cubesTag);
	//	numberInteractors.text = objList.Length.ToString ();

	}

	void Update () {
		//label number interactors
	


		int totalElem = 0 ;

		foreach(GameObject obj in objList) {
			if(obj.renderer.enabled == true) {
				totalElem = totalElem+1;

			}
			numberInteractors.text = totalElem.ToString ();

		}


	}

}
