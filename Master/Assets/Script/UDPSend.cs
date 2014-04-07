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

	public GameObject car; 
	public GameObject cible;

	// "connection" things
	IPEndPoint remoteEndPoint;
	UdpClient client;


	// Vector pointer 
	private Vector3 afterRotation;

	
	//ben add
	string strMessage="";
	
	float intervalTimer=0.0f; 
	float interval=0.0f;

	// call it from shell (as program)
	
	public static void Main() {
		
		UDPSend sendObj=new UDPSend();
		sendObj.init();
		// testing via console
		// sendObj.inputFromConsole();

		// as server sending endless
		sendObj.sendEndless(" endless infos \n");
	}
	
	// start from unity3d

	public void ChangeIP(){

	}
	
	public void Start() {
		init(); 

		cible=GameObject.Find("cible");
		car=GameObject.Find("Car");

		InvokeRepeating("refreshVector",1,0.1f);
	
	}


	public void Update(){


		Vector3 cibletempo = new Vector3 (cible.transform.localPosition.x, 0, cible.transform.localPosition.z);
		Vector3 cartempo = new Vector3 (car.transform.localPosition.x, 0, car.transform.localPosition.z);
		Vector3 vecteurCible = cibletempo-cartempo;
		
		Quaternion rotateVectorAboutY = Quaternion.AngleAxis(-car.transform.eulerAngles.y, Vector3.up);
		afterRotation = rotateVectorAboutY * vecteurCible;
		
		
		Debug.DrawRay (Vector3.zero, afterRotation, Color.yellow);


	}

	public void refreshVector(){

		sendString("Pointer/"+afterRotation.ToString("G4").Replace("(","").Replace(")",""));

	}

	


	// init
	public void init() {
		
		// Endpunkt definieren, von dem die Nachrichten gesendet werden.
		print("UDPSend.init()");
	
		remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
		client = new UdpClient();

		print("Sending to "+IP+" : "+port);
		print("Testing: nc -lu "+IP+" : "+port);

	}

	
	// sendData
	
	public void sendString(string message) {
		try {
			// Daten mit der UTF8-Kodierung in das Binärformat kodieren.
			byte[] data = Encoding.UTF8.GetBytes(message);
			
			// Den message zum Remote-Client senden.
			client.Send(data, data.Length, remoteEndPoint);
			
			//}
		} catch (Exception err) {
			print(err.ToString());
		}
		
	}


	// endless test
	
	private void sendEndless(string testStr) {
		 do {
			 sendString(testStr);
		 } while(true);
	}

}