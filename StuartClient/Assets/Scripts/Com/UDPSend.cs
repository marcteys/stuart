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
	public bool debugMode = false;

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
		if(debugMode) Debug.Log (message);
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