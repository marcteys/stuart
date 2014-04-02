using UnityEngine;
using System.Collections;

public class ClickCubes : MonoBehaviour {



	public Transform ground;
	
	public string tagCubes  = "Cubes" ;
	public float limit  = 250f ;


	private ChangeForce tmpCubeScript;
		

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			DetectCube();
		}	
	}


	void DetectCube() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		//desactivate if click somewhere
		//if(tmpCubeScript) tmpCubeScript.Desactivate();
		//tmpCubeScript = null;

		if (Physics.Raycast(ray, out hit, limit)) {

			if(hit.transform.CompareTag( tagCubes )){
				Debug.Log("click on Cube " + hit.transform.name);
				if(tmpCubeScript) tmpCubeScript.Desactivate();
				tmpCubeScript = hit.transform.gameObject.GetComponent<ChangeForce>();
				tmpCubeScript.Activate();
			} else {
				Debug.Log ("desactivate");
				if(tmpCubeScript) tmpCubeScript.Desactivate();
			}

		}
	}

	void DetectObject() {

	}
}
