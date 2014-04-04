using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


public class UDPReceive : MonoBehaviour {
	// receiving Thread
	Thread receiveThread; 
	public GameObject cible;
	Vector3 relative;

	public GameObject car;
	public Vector3 pos;
	public Quaternion rot;
	public GameObject cam;

	//voiture 
	Vector3[] moy_voit_pos= new Vector3[1];
	Vector3[] moy_voit_rot= new Vector3[1];


	Vector3[] allPos = new Vector3[10]; // tableau des cams 
	Quaternion[] allRot = new Quaternion[10]; // tableau des cams 
	//
	public bool pointerCheck=false;
	 
	//public UDPSend udpSend;
	List<Cube> myCubes = new List<Cube>();

	// udpclient object
	UdpClient client; 
	public int port; // define > init
	
	
	
	// infos
	public string lastReceivedUDPPacket="";
	public string allReceivedUDPPackets=""; // clean up this from time to time!

	public float speed;


	public bool debugMode = false;

	public AstarAI_light AIScript;
	// start from shell
	private static void Main()  {
		UDPReceive receiveObj=new UDPReceive();
		receiveObj.init();
		string text="";
		do {
			text = Console.ReadLine();
		}
		while(!text.Equals("exit"));
	}
	
	// start from unity3d
	public void Start() {
		AIScript = GameObject.FindGameObjectWithTag ("Car").GetComponent<AstarAI_light> ();

		cible= GameObject.Find("cible");
		car= GameObject.Find("Car");
		cam = GameObject.Find ("Camera");

		init(); 
	}


	// init
	private void init() { 

		// define port
	//	port = 8052; 
		// status
		if(debugMode) print("Sending to 127.0.0.1 : "+port);
		if(debugMode) print("Test-Sending to this Port: nc -u 127.0.0.1  "+port+"");
	
		
		receiveThread = new Thread(new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();
	}
	
	
	
	// receive thread 
	private void ReceiveData() {
		
		client = new UdpClient(port);
		while (true) {
			try  {
				// Bytes empfangen.
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = client.Receive(ref anyIP);
				
				// Bytes mit der UTF8-Kodierung in das Textformat kodieren.
				string text = Encoding.UTF8.GetString(data);



				// ------------------------------
				// PARSING
				// ------------------------------

				lastReceivedUDPPacket=text;
				lastReceivedUDPPacket=lastReceivedUDPPacket.Replace("(", "");
				lastReceivedUDPPacket=lastReceivedUDPPacket.Replace(")", "");

				if(debugMode) {
					Debug.Log (lastReceivedUDPPacket);
				}

				string[] strs = lastReceivedUDPPacket.Split('/');
				if(strs[0]=="Car"){ 
					refreshCar(strs[1],strs[2]);
				} 
				else if(strs[0]=="Cube"){
					refreshCube(strs[1],strs[2]);
				}
				else if(strs[0]=="Pointer"){
					pointerCheck=true;
					refreshPointer(strs[1]);

				}else if(strs[0]=="CubeForce"){
					refreshCubeForce(strs[1]+"/"+strs[2]);
				}else if(strs[0]=="emergency"){

					AIScript.emergencyStop = true;

				}
			}
			
			catch (Exception err) {
				print(err.ToString());
			}
		}
	}

	//REFRESH FORCE DATA
	void refreshCubeForce(string data){
		string []  datas =data.Split('/');
		get_Cube(int.Parse(datas[0])).force = float.Parse(datas[1]);

	

	}

	//REFRESH POINTER DATA
	void refreshPointer(string data){
			 relative=sTov3(data);
	}

	// ------------------------------
	// Refresh CUBES
	// ------------------------------
	void refreshCube(string idcam, string data){

	   string []  datas =data.Split('|');
		for (int i=0; i<datas.Length; i++) {
			string []  infos =datas[i].Split(';');
			if(int.Parse(infos[2])==1){
				if(!check_idCubes(int.Parse(infos[1]) )){
					// new cube instance
					myCubes.Add(new Cube(int.Parse(infos[1]),sTov3(infos[3]),sToQ(infos[4])));
				}else{
					Cube tempo=get_Cube(int.Parse(infos[1]));
					tempo.moy[int.Parse (idcam)-1]=sTov3(infos[3]);
					tempo.pos=tempo.get_pos();
             	}
			}
		}
	}

	//check id 
	bool check_idCubes(int id){
		bool a = false;
		foreach (Cube c in myCubes) {
			if(c.id==id) a=true;
		}
		return a;
	}

	public Cube get_Cube(int id){
		Cube v=new Cube(0,Vector3.zero,Quaternion.identity);
		foreach (Cube c in myCubes) {
			if(c.id==id) v=c;
		}
		return v;
	}


	// ------------------------------
	// Refresh CAR
	// ------------------------------

	void refreshCar(string idcam,string data){

		string []  datas =data.Split(';');


		allPos[int.Parse(idcam)] = sTov3 (datas [1]);
		allRot[int.Parse(idcam)] = sToQ (datas [2]);
		if(debugMode) Debug.Log ("cam " +int.Parse(idcam)+ " valeurs " + sTov3 (datas [1]));



	///////////////////////////	pos = sTov3 (datas [1]);

		// pos = Vector3.Lerp (car.transform.position,sTov3(datas [1]),Time.deltaTime *10);
	//////////////////////////////	rot = sToQ(datas[2]);

		/*

		moy_voit_pos[int.Parse(idcam)-1]=sTov3(datas[1]);

	//	moy_voit_rot[int.Parse(idcam)-1]=sTov3(datas[2]);
		Vector3 total_pos= Vector3.zero;
		Vector3 total_rot =Vector3.zero;

		//Average
			for(int i=0; i<moy_voit_pos.Length;i++){
				total_pos+=moy_voit_pos[i];
			}
		rot = sToQ(datas[2]);

		for(int i=0; i<moy_voit_rot.Length;i++){
			//rotation
			//Debug.Log(sToQ(datas[2]));
		}


		//Debug.Log((total_rot.x/2)%180);
		//rot=moy_voit_rot[1];//sTov3(datas[2]);
		//rot
		//Debug.Log(rot.y%360);
		pos=total_pos/moy_voit_pos.Length;
		//rot=;
		//rot.y=sTov3(datas[2]).y;
		//Debug.Log (total_pos);
		//Debug.Log (rot.y+"  ///  1:"+moy_voit_rot[0].y%360+" ////2:"+moy_voit_rot[1].y%360);
			//pos=sTov3(datas[1]);
			//rot=sTov3(datas[2]);
		//}

*/
	}

	Vector3 sTov3(string text){
		string[] pos_str=text.Split(',');
		return	new Vector3( float.Parse(pos_str[0]) ,float.Parse(pos_str[1]) , float.Parse(pos_str[2])  );
	}

	Quaternion sToQ(string text){
		string[] pos_str=text.Split(',');
		return	new Quaternion( float.Parse(pos_str[0]) ,float.Parse(pos_str[1]) , float.Parse(pos_str[2]),float.Parse(pos_str[3])  );
	}


	void Update () {
		//Update Cible
	



		//Update Car
		float min = 9999;
		int idMin = 0;
		for (int i = 0; i < allPos.Length; i++) {
			if(allPos[i] != Vector3.zero ) {
				float distance = Vector3.Distance (car.transform.position, allPos[i]);
				if(distance < min && distance != 0) {
					min = distance;
					idMin = i ;
				}
			}
		}

		car.transform.position = allPos [idMin];
		car.transform.rotation = allRot [idMin];

		//vider le tableau
		// System.Array.Clear (allPos,0,allPos.Length);

		//lock car 
		car.transform.eulerAngles=new Vector3(0,car.transform.eulerAngles.y,0);
		car.transform.position = new Vector3 (car.transform.position.x, 0, car.transform.position.z);


		//Update Cube
		foreach(Cube c in myCubes){
			if(GameObject.Find("c_"+c.id) == null ){
					GameObject instance = Instantiate(Resources.Load("c_1", typeof(GameObject))) as GameObject;
					instance.name="c_"+c.id;
					instance.transform.position=c.pos;
					//instance.transform.eulerAngles=new Vector3(0,c.rot.y,0);
					instance.transform.rotation=c.rot;
					
				//lock cubes pos
				instance.transform.eulerAngles=new Vector3(0,instance.transform.eulerAngles.y,0);
				instance.transform.position = new Vector3 (instance.transform.position.x, 0, instance.transform.position.z);

				}else{
					GameObject b =GameObject.Find("c_"+c.id);
					b.transform.position=c.pos;
					//b.transform.eulerAngles=new Vector3(0,c.rot.y,0);
				b.transform.rotation=c.rot;

				//lock cube pos
				b.transform.eulerAngles=new Vector3(0,b.transform.eulerAngles.y,0);
				b.transform.position = new Vector3 (b.transform.position.x, 0, b.transform.position.z);
			}
		}

		if(pointerCheck){
// 

			relative= car.transform.rotation*relative;
			cible.transform.position=car.transform.position+relative;


			pointerCheck=false;

		}

	}


	// end UDP
	public string getLatestUDPPacket() {
		allReceivedUDPPackets="";
		return lastReceivedUDPPacket;
	}

	void OnDisable() { 
		Debug.Log ("stop udp");
		if ( receiveThread!= null) 
			receiveThread.Abort(); 
		client.Close(); 
	} 

	float Map(this float value,  float low1,  float high1,  float low2,  float high2){
		return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
	}
}





public class Cube {
	public Vector3 pos;
	public Quaternion rot;
	public float force=0;
	public int id;
	public Vector3 total;
	public Vector3[] moy= new Vector3[1];


	public Cube(int aid ,Vector3 apos, Quaternion arot) {
		pos = apos;
		rot = arot;
		id = aid;
	}


	public Vector3 get_pos(){
		total= Vector3.zero;

		for(int i=0; i<moy.Length; i++){
			total= total+moy[i];
		}

		return total/moy.Length;
	}

}


