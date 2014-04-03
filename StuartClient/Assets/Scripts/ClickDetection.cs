using UnityEngine;
using System.Collections;

public class ClickDetection : MonoBehaviour {

	public bool debugMode = false;

	public Transform ground;
	
	public string tagGround  = "Ground" ;
	public string tagCar  = "Car1" ;
	public float limit  = 250f ;
	//public float sphereRayon  = 0.5;
	
	public bool objectClicked  = false;

	//prefabs
	
	public GameObject dotPrefab ;
	private GameObject tmpSelect ;

		
	private Transform car;


	//com
	public UDPSend udpSend;


	private Vector3 pointerVector;


	// Use this for initialization
	void Start () {
		car = GameObject.FindGameObjectWithTag("Car").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			PlacePoint();
		}	
		Debug.DrawRay (Vector3.zero,pointerVector,Color.green);
		Debug.Log (pointerVector);
	}


	void PlacePoint() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, limit)) {

			if(hit.transform.CompareTag( tagGround )){
				Debug.Log(hit.point);
				if(debugMode){
					if(tmpSelect) Destroy (tmpSelect);
					tmpSelect = Instantiate(dotPrefab, hit.point, Quaternion.identity) as GameObject;
				}

				CalculatePos(hit.point);
			}
		}
	}

	void DetectObject() {

	}

	void CalculatePos(Vector3 pointPos) {
			
		pointerVector =   pointPos - car.transform.position;

		pointerVector = new Vector3(pointerVector.x,0,pointerVector.z);

		string pointerString = pointerVector.ToString("G4").Replace("(","").Replace(")","");

		for(int i = 0; i < 3 ; i++) {
			udpSend.sendString("Pointer/"+pointerString+"\n");
		}

	}
}
