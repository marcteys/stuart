using UnityEngine;
using System.Collections;

public class GetCubesAndSend : MonoBehaviour {

	public string cubesTag = "Cubes";
	public GameObject[] objList;
	//gros nombre interactors
	private UILabel numberInteractors;
	private UDPSend udpSender;

	void Start() {
		//label number interactors
		//numberInteractors = this.GetComponent<UILabel>();
		
		objList = GameObject.FindGameObjectsWithTag(cubesTag);
		numberInteractors.text = objList.Length.ToString ();

		udpSender = GameObject.Find ("_UDPSend").GetComponent<UDPSend>();


	//	InvokeRepeating("SendCubeForce", 1, 0.8f);


	}


	void Update () {

	}

	void SendCubeForce() {
		foreach(GameObject obj in objList) {
			float sendForce = obj.GetComponent<ChangeForce>().totalForce;
			
			Debug.Log ("CubeForce/"+obj.name+"/"+sendForce);
			udpSender.sendString("CubeForce/"+obj.name+"/"+sendForce);
			
		}
	}

}
