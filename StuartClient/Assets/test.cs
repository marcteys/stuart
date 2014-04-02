using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {


	public Transform[] cubes = new Transform[2];
	public Transform  mainCube;

	public Quaternion rot;
	public float speed = 5f;
	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;

		foreach( Transform cube in cubes) {
/*
			rot.w += cube.rotation.w;
			rot.x += cube.rotation.x;
			rot.y += cube.rotation.y;
			rot.z += cube.rotation.y;
*/
		//	mainCube.transform.eulerAngles = Vector3.Lerp (mainCube.transform.eulerAngles ,  cube.eulerAngles,step);
			mainCube.rotation = Quaternion.Slerp(mainCube.rotation, cube.rotation, step);
		}
	


	//	rot = rot/cubes.Length;
	//	mainCube.rotation = rot;


	}

	
	Vector3 eeulervector( Vector3 euler)//convert euler to vector3
		
	{
		
		float elevation =  Mathf.Deg2Rad  *euler.x;
		
		float heading =  Mathf.Deg2Rad*euler.y;
		
		return new Vector3(Mathf.Cos(elevation) * Mathf.Sin(heading), Mathf.Sin(elevation), Mathf.Cos(elevation) * Mathf.Cos(heading));
		
	}




}
