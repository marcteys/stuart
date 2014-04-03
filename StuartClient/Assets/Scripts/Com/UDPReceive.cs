
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;



public class UDPReceive : MonoBehaviour {


	public bool debugMode = false;

	// receiving Thread
	Thread receiveThread; 

	// udpclient object
	UdpClient client; 

	// public
	
	// public string IP = "127.0.0.1"; default local
	public int port; // define > init

	// infos
	public string lastReceivedUDPPacket="";
	public string allReceivedUDPPackets=""; // clean up this from time to time!
	

	// start from shell
	public Transform car;
	public Transform pointer;


	public Vector3 newPos;

	private static void Main() {
		
		UDPReceive receiveObj=new UDPReceive();
		
		receiveObj.init();

		string text="";

		do {
			text = Console.ReadLine();
		} while(!text.Equals("exit"));
		
	}
	
	// start from unity3d
	
	public void Start()
		
	{
		car = GameObject.FindGameObjectWithTag("Car").transform;
		pointer = GameObject.FindGameObjectWithTag("Pointer").transform;
		
		
		init(); 
		
	}
	
	

	
	private void init() {
		
		print("UDPRecieve.init()");
	
		
	//	print("Sending to 127.0.0.1 : "+port);
	
//		print("Test-Sending to this Port: nc -u 127.0.0.1  "+port+"");
		

		receiveThread = new Thread(
			new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();

	}
	
	
	
	// receive thread 
	
	private  void ReceiveData() {
		
		
		
		client = new UdpClient(port);
		while (true) {
			

			try {
				
				// Bytes empfangen.
				
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = client.Receive(ref anyIP);
				
			
				// Bytes mit der UTF8-Kodierung in das Textformat kodieren.
				string text = Encoding.UTF8.GetString(data);

				// Den abgerufenen Text anzeigen.
				//print(">> " + text);

				if(debugMode) Debug.Log(text);

				// MISE A JIUR GAME OBJECT
				string[] datas= text.Split('/');

				if(datas[0] == "Pointer") {
					newPos = sTov3(datas[1]);
					if(debugMode) Debug.Log("New pos ! : " + newPos);
				}










				// latest UDPpacket
				
				lastReceivedUDPPacket=text;
		
				
			} catch (Exception err) {
				print(err.ToString());
			}
		}
	}
	


	Vector3 sTov3(string text){
		
		string[] pos_str=text.Split(',');
		return	new Vector3( float.Parse(pos_str[0]) ,float.Parse(pos_str[1]) , float.Parse(pos_str[2])  );
		
	}


	void Update() {
		pointer.position = newPos + car.position ;
		pointer.position = new Vector3(pointer.position.x,-0.8f,pointer.position.z);

		Debug.DrawRay(Vector3.zero,newPos,Color.blue);



	}









	void OnDisable() 
	{ 
		if ( receiveThread!= null) 
			receiveThread.Abort(); 
		
		client.Close(); 
	}

	public string getLatestUDPPacket(){
		
		allReceivedUDPPackets="";
		
		return lastReceivedUDPPacket;
		
	}
	
}