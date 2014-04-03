/*

 

    -----------------------

    UDP-Send

    -----------------------

    // [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]

    

    // > gesendetes unter 

    // 127.0.0.1 : 8050 empfangen

    

    // nc -lu 127.0.0.1 8050

 

        // todo: shutdown thread at the end

*/

using UnityEngine;

using System.Collections;



using System;

using System.Text;

using System.Net;

using System.Net.Sockets;

using System.Threading;



public class UDPSend : MonoBehaviour
	
{
	
	private static int localPort;
	
	
	
	// prefs 
	
	public string IP;  // define in init
	
	public int port;  // define in init


	public GameObject car; 

	public GameObject cible;


	// "connection" things
	
	IPEndPoint remoteEndPoint;
	
	UdpClient client;
	

	
	//ben add


	string strMessage="";
	
	float intervalTimer=0.0f; 
	float interval=0.0f;
	
	
	
	// call it from shell (as program)
	
	public static void Main() 
		
	{
		
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
	
	public void Start()
		
	{
		
		init(); 

		cible=GameObject.Find("cible");
		car=GameObject.Find("Car");

		InvokeRepeating("refreshVector",1,0.1f);
	
	}
	

	public void refreshVector(){


		Vector3 cibletempo = new Vector3 (cible.transform.position.x, 0, cible.transform.position.z);
		Vector3 cartempo = new Vector3 (car.transform.position.x, 0, car.transform.position.z);

		Vector3 vector= cibletempo-cartempo;
		//vector=Quaternion.AngleAxis(180,Vector3.up);
	

		sendString("Pointer/"+vector.ToString("G4").Replace("(","").Replace(")",""));


	}



	// init
	
	public void init()
		
	{
		
		// Endpunkt definieren, von dem die Nachrichten gesendet werden.
		
		print("UDPSend.init()");
		
		

		// define
		
	//	IP="192.168.0.17";
		
	//	port=8053;
		
		
		
		// ----------------------------
		
		// Senden
		
		// ----------------------------
		
		remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
		
		client = new UdpClient();
		
		
		
		// status
		
		print("Sending to "+IP+" : "+port);
		
		print("Testing: nc -lu "+IP+" : "+port);
		
		
		
	}
	
	
	

	
	
	
	// sendData
	
	public void sendString(string message)
		
	{
		
		try 
			
		{
			
			//if (message != "") 
			
			//{
			
			
			
			// Daten mit der UTF8-Kodierung in das Binärformat kodieren.
			
			byte[] data = Encoding.UTF8.GetBytes(message);
			
			
			
			// Den message zum Remote-Client senden.
			
			client.Send(data, data.Length, remoteEndPoint);
			
			//}
			
		}
		
		catch (Exception err)
			
		{
			
			print(err.ToString());
			
		}
		
	}
	
	
	
	
	
	// endless test
	
	private void sendEndless(string testStr)
		
	{
		
		do
			
		{
			
			sendString(testStr);
			
			
			
			
			
		}
		
		while(true);
		
		
		
	}
	


	
}