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
	public int camSlaveId;

	// "connection" things
	IPEndPoint remoteEndPoint;
	UdpClient client;
		
	//ben add
	string strMessage="";
	float intervalTimer=0.0f; 
	float interval=0.0f;

	//MARC ADD
	GameObject car;
	int nbrCubes;
	// call it from shell (as program)
	private static void Main() {
		UDPSend sendObj=new UDPSend();
		sendObj.init();
		sendObj.sendEndless(" endless infos \n");
	}

	
	public void Start() {

		//global detection
		nbrCubes =GameObject.FindGameObjectsWithTag("cube").Length;
		car =GameObject.FindGameObjectWithTag("car");


		init(); 

		InvokeRepeating("RepeatingFunctionCube", 1, 0.6f);
		InvokeRepeating("RepeatingFunctionCar", 1, 0.2f);
	}


	void RepeatingFunctionCar () {

		if( car.renderer.enabled ){
			int fiable=0;
			string pos = car.transform.position.ToString("G4").Replace("(","").Replace(")","");
			string rot = car.transform.rotation.ToString("G4").Replace("(","").Replace(")","");
			if( car.transform.eulerAngles.x< 310 && car.transform.eulerAngles.z< 310) fiable=1; 

			string elem= "Car/"+camSlaveId+"/"+fiable+";"+pos+";"+rot;
			sendString(elem+"\n");
		}

	}

	void RepeatingFunctionCube () {

	// parcour tout les objet avec le tag actif 
		string elems="";
		string elem="";
		int nbobj=0;
		foreach(GameObject marker in GameObject.FindGameObjectsWithTag("cube")) {
			if(marker.renderer.enabled ){ // envoi data si rendu actif (detecter par vuforia)

				nbobj++;

				string pos = marker.transform.position.ToString("G4").Replace("(","").Replace(")","");
				string rot = marker.transform.rotation.ToString("G4").Replace("(","").Replace(")","");
				string[] infos = marker.name.Split('_');

				elem= infos[0]+";"+infos[1]+";"+1+";"+pos+";"+rot;
				Debug.Log (nbobj +" - "+nbrCubes);
				
				if(nbobj==1){
					elems="Cube/"+camSlaveId+"/"+elem;
				}else if(nbobj>1){
					elems+="|"+elem;
				}
			}
		}

		sendString(elems+"\n");
		//Debug.Log(elems);
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
	private void inputFromConsole() {
		try  {
			string text;
			do {
				text = Console.ReadLine();
				// Den Text zum Remote-Client senden.
				if (text != "") {		
					// Daten mit der UTF8-Kodierung in das Binärformat kodieren.
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
	private void sendString(string message) {
		Debug.Log (message);
		try {
			byte[] data = Encoding.UTF8.GetBytes(message);
			client.Send(data, data.Length, remoteEndPoint);

		} catch (Exception err) {
			print(err.ToString());
		}
	}
	

	// endless test
	private void sendEndless(string testStr) {
		
		do {
			sendString(testStr);
		}
		while(true);
	}
}