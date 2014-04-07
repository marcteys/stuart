using UnityEngine;
using System.Collections;
using System;

public class Com : MonoBehaviour {




	// OSC========
	public string UDPHost  = "127.0.0.1";
	private int listenerPort = 8000;
	public int broadcastPort  = 57131;
	private Osc oscHandler ;
	
	private string eventName = "";
	private string eventData  = "";
//	private int counter  = 0;

	public string messageData ;
	public UDPPacketIO udp;
	// OSC========



	public bool outputToSerial=true;

	public byte m1_1=0;
	public byte m1_2=0;

	public byte m2_1=0;
	public byte m2_2=0;


	// Use this for initialization
	private string message="";

//	float lastTime=0;


	void Start () {

		udp  = this.GetComponent<UDPPacketIO>();
		udp.init(UDPHost, broadcastPort, listenerPort);
		oscHandler = this.GetComponent<Osc>();
		oscHandler.init(udp);


	}



	void Update () {	
	//	counter++;
		messageData=m1_1+"/"+m1_2+"/"+m2_1+"/"+m2_2;


		sendMessage(messageData);
	}	




	void sendMessage(string message )
	{	
		oscHandler.Send(Osc.StringToOscMessage(message)); 
		
	} 
	
	
	void OnApplicationQuit () {
		sendMessage("##end##");
		udp.Close();
	}







}
