using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


public class UDPSend : MonoBehaviour {
	
	private static int localPort;

	
	// prefs 
	public string IP;  // define in init
	public int port;  // define in init
	

	// "connection" things
	IPEndPoint remoteEndPoint;
	UdpClient client;
	

	//ben add

	string strMessage="";
	float intervalTimer=0.0f; 
	float interval=0.0f;

	// call it from shell (as program)
	
	public static void Main()  {
		
		UDPSend sendObj=new UDPSend();
		sendObj.init();
		sendObj.sendEndless(" endless infos \n");

	}
	
	// start from unity3d

	public void ChangeIP(){
	//	sendObj.init();

		
	}
	
	public void Start() {
		
		init(); 
		//GameObject obj= GameObject.Find("ImageTargetStones");
		//script = obj.GetComponent<ImageTargetTrackableEventHandler>();
		//
		//InvokeRepeating("RepeatingFunctionCube", 1, 0.8f);

		//InvokeRepeating("RepeatingFunctionCar", 1, 0.05f);

	}
	
	

	void RepeatingFunctionCar () {


		GameObject car =GameObject.FindGameObjectWithTag("car");

		if( car.renderer.enabled ){
			int fiable=0;
			string pos = car.transform.position.ToString("G4").Replace("(","").Replace(")","");
			string rot = car.transform.eulerAngles.ToString("G4").Replace("(","").Replace(")","");
			if( car.transform.eulerAngles.x< 310 && car.transform.eulerAngles.z< 310) fiable=1; 

			string elem= "Car/Cam1/"+fiable+";"+pos+";"+rot;
			sendString(elem+"\n");

		}


	}



	void RepeatingFunctionCube () {


	// parcour tout les objet avec le tag actif 
		string elems="";
		int nbobj=0;
		int toto =GameObject.FindGameObjectsWithTag("cube").Length;
		foreach(GameObject marker in GameObject.FindGameObjectsWithTag("cube")) {
			nbobj++;

			if(marker.renderer.enabled ){ // envoi data si rendu actif (detecter par vuforia)

				string pos = marker.transform.position.ToString("G4").Replace("(","").Replace(")","");

				string rot = marker.transform.eulerAngles.ToString("G4").Replace("(","").Replace(")","");
			
				int fiable=0;
			
			if( marker.transform.eulerAngles.x< 310 && marker.transform.eulerAngles.z< 310) fiable=1; // si marker de traviol = 0 

				string[] infos = marker.name.Split('_');

				string elem= infos[0]+";"+infos[1]+";"+fiable+";"+pos+";"+rot;

				if(toto==1){

					elems="Cube/Cam1/"+elem;

				}else if(nbobj==1 && toto>1 ){

					elems="Cube/Cam1/"+elem+"|";
				
				}else if(nbobj==2){

					elems+=elem;
				}else if(nbobj>2){
					elems+="|"+elem;
				}

			}

		}

		sendString(elems+"\n");


		//Will print initialDelay + repeatTime * repetitions
	}

	// init
	
	public void init() {
		
		// Endpunkt definieren, von dem die Nachrichten gesendet werden.
		print("UDPSend.init()");
	
		// ----------------------------
		
		// Senden
		// ----------------------------
		remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
		client = new UdpClient();

		// status
		print("Sending to "+IP+" : "+port);
		print("Testing: nc -lu "+IP+" : "+port);
		
		
	}
	
	
	
	// inputFromConsole
	
	public void inputFromConsole() {
		
		try  {
			string text;
			do {
				text = Console.ReadLine();
				if (text != "") {

					byte[] data = Encoding.UTF8.GetBytes(text);
					// Den Text zum Remote-Client senden.
					client.Send(data, data.Length, remoteEndPoint);
				}
			} while (text != "");
			
		} catch (Exception err) {
			print(err.ToString());
		}
	}

	// sendData
	
	public void sendString(string message) {
		try  {
			byte[] data = Encoding.UTF8.GetBytes(message);
			client.Send(data, data.Length, remoteEndPoint);
		} catch (Exception err) {
			print(err.ToString());
		}
	}
	
	
	
public void changeIP(string newIP) {


}

	private void sendEndless(string testStr) {
		do {
			 sendString(testStr);
		} while(true);

	}

	
}