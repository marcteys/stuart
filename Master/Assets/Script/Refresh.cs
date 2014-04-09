using UnityEngine;
using System.Collections;

public class Refresh : MonoBehaviour {
	float lastMillis=0;

	// Use this for initialization
	void Start () {


		InvokeRepeating ("reload", 1, 4f);

	}
	
	// Update is called once per frame
	void Update () {





	}


	void reload(){

		/*
		foreach(GameObject c in GameObject.FindGameObjectsWithTag("Cube")) {
			c.transform.position= new Vector3(30,30,30);
			
		}
		*/


		AstarPath.active.Scan();

}
}
