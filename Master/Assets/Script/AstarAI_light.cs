using UnityEngine;
using System.Collections;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use %Pathfinding
using Pathfinding;

[RequireComponent (typeof(Seeker))]



public class AstarAI_light : MonoBehaviour {


	// Variable init

	public Transform target;
	public float repathRate = 0.1F;

	public bool canSearch = true;
	public bool canMove = true;

	public float coefForceField=1;
	protected float lastPathSearch = -9999;

	protected Seeker seeker;

	public Com com;
	public UDPReceive receive;


	private float lastTimeCheck;
	private float timeIntervalFlag = 3000;


	protected Vector3[] path;
	protected int pathIndex = 0;
	protected Transform tr;


	public float pickNextWaypointDistance = 1F;
	public float targetReached = 0.2F;


	private GameObject car;
	private GameObject cible;


	public bool emergencyStop = false;

	public void Start () {
		//Get a reference to the Seeker component we added earlier
		seeker = GetComponent<Seeker>();

		com = gameObject.GetComponent<Com>();
		receive = GameObject.Find("Camera").GetComponent<UDPReceive>();
		car= GameObject.Find("Car");
		cible= GameObject.Find("cible");
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		seeker.StartPath (transform.position,target.position, OnPathComplete);

		tr=transform;
		Repath ();


	}


	
	public void OnPathComplete (Path p) {
		
		/*if (Time.time-lastPathSearch >= repathRate) {
			Repath ();
		} else {*/
		StartCoroutine (WaitToRepath ());
		//}
		
		//If the path didn't succeed, don't proceed
		if (p.error) {
			return;
		}
		
		//Get the calculated path as a Vector3 array
		path = p.vectorPath.ToArray();
		
		//Find the segment in the path which is closest to the AI
		//If a closer segment hasn't been found in '6' iterations, break because it is unlikely to find any closer ones then
		float minDist = Mathf.Infinity;
		int notCloserHits = 0;
		
		for (int i=0;i<path.Length-1;i++) {
			float dist = AstarMath.DistancePointSegmentStrict (path[i],path[i+1],tr.position);
			if (dist < minDist) {
				notCloserHits = 0;
				minDist = dist;
				pathIndex = i+1;
			} else if (notCloserHits > 6) {
				break;
			}
		}
	}



	public IEnumerator WaitToRepath () {
		float timeLeft = repathRate - (Time.time-lastPathSearch);
		
		yield return new WaitForSeconds (timeLeft);
		Repath ();

	}



	public virtual void Repath () {

		lastPathSearch = Time.time;


		if (seeker == null || target == null || !canSearch || !seeker.IsDone ()) {
			StartCoroutine (WaitToRepath ());
			return;
		}

		Path p = ABPath.Construct(transform.position,target.position,null);
		seeker.StartPath (p,OnPathComplete);

	}




	public void PathToTarget (Vector3 targetPoint) {
		lastPathSearch = Time.time;
		
		if (seeker == null) {
			return;
		}
		
		//Start a new path from transform.positon to target.position, return the result to OnPathComplete
		seeker.StartPath (transform.position,targetPoint,OnPathComplete);
	}



public void Update () {// Start Update
		
//	___________ Pas touche Astar

		if (path == null || pathIndex >= path.Length || pathIndex < 0 || !canMove) {
			return;
		}

		//Change target to the next waypoint if the current one is close enough
		Vector3 currentWaypoint = path[pathIndex];
		currentWaypoint.y = tr.position.y;
		while ((currentWaypoint - tr.position).sqrMagnitude < pickNextWaypointDistance*pickNextWaypointDistance) {
			pathIndex++;
			if (pathIndex >= path.Length) {
				//Use a lower pickNextWaypointDistance for the last point. If it isn't that close, then decrement the pathIndex to the previous value and break the loop
				if ((currentWaypoint - tr.position).sqrMagnitude < (pickNextWaypointDistance*targetReached)*(pickNextWaypointDistance*targetReached)) {
					ReachedEndOfPath ();
					return;
				} else {
					pathIndex--;
					//Break the loop, otherwise it will try to check for the last point in an infinite loop
					break;
				}
			}
			currentWaypoint = path[pathIndex];
			currentWaypoint.y = tr.position.y;
		}
		
//	___________ Pas touche Astar


//	___________ Vector direction
		Vector3 dir = currentWaypoint - tr.position;
		Vector3 forwardDir = transform.forward;
		Debug.DrawRay(transform.position,forwardDir.normalized*2,Color.red);// Direction real car
//	___________ Vector direction		 



//	___________ Application force cube

		foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube")) {    
				
		
		
			float dist_cube=Vector3.Distance(transform.position,cube.transform.position );

			Vector3 field = cube.transform.position-transform.position;
			field=field.normalized/(dist_cube*4f);
			int nameCube= int.Parse(cube.gameObject.name.Split('_')[1]);

		
    
	      //  int id= (int) nameCube[1];
			float forceField=receive.get_Cube(nameCube).force;


		    dir=dir+(field*(forceField*-coefForceField));

			Debug.DrawRay(transform.position,field*(forceField*-coefForceField),Color.yellow);
//			Debug.Log( dist_cube); 


		
		
		}
	
//	___________ Application force cube


//	___________ Angle direction
		Debug.DrawRay(transform.position,dir.normalized*2,Color.blue);
		float angle = Vector3.Angle(dir, forwardDir);
		//Debug.Log (angle);

		Vector3 cross= Vector3.Cross(dir, forwardDir);
		if (cross.y < 0) angle = -angle;
//	___________ Angle direction		
	


float dist=Vector3.Distance(car.transform.position,cible.transform.position);


//	___________ Arduino Car AI

	/*

		if(dist>1){
		
			if(angle>-10 && angle<10 ){

				com.m1_1=100;
				com.m2_1=100;


			}else if(angle>10){

				com.m1_1=0;
				com.m2_1=100;



			}else if(angle<-10){

				com.m2_1=0;
				com.m1_1=100;

			}


		 }else{


			stopCar();

		}
*/


	

		if(dist>1 && !emergencyStop){



			if(angle<50 && angle> -50){

				com.m1_2=0;
				com.m1_1=(byte) Map(angle,-50,50,255,0);

				com.m2_2=0;
				com.m2_1=(byte) Map(angle,-50,50,0,255);


			}else{


				if( angle<50 && angle>-180){

				

					com.m1_2= 0;
					com.m1_1=(byte) Map(angle,-50,-180,100,255);
					
					com.m2_2=(byte) Map(angle,-50,-180,100,255);
					com.m2_1=0;


				}else{


					//Debug.Log (angle);

					
					com.m1_2= (byte) Map(angle,50,180,100,255);
					com.m1_1=0;
					
					com.m2_2=0;
					com.m2_1=(byte) Map(angle,50,180,100,255);
					

				}


			







			}



			
		}else{
			
			
			stopCar();
			
		}






//	___________ Arduino Car AI





	}// End Update


	
	void OnGUI () {// UI pour SCAN



		if (GUI.Button (new Rect (10,10,150,100), "Scan")) {

			AstarPath.active.Scan();
			//Repath();

		}

	}


	public virtual void ReachedEndOfPath () {
		//The AI has reached the end of the path
	}



	 float Remap (this float value, float from1, float to1, float from2, float to2) {
		
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		
	}

	float Map(this float value,  float low1,  float high1,  float low2,  float high2){


		return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
	}


	void stopCar(){

		com.m2_1= 0;
		com.m2_2= 0;
		com.m1_1=0;
		com.m1_2=0;

	}



	void OnDisable(){

		stopCar();
	}

} 