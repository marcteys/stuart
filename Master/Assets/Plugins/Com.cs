using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.IO;
using System;
using System.Threading;

public class Com : MonoBehaviour {

	public bool outputToSerial=true;

	public byte m1_1=0;
	public byte m1_2=0;

	public byte m2_1=0;
	public byte m2_2=0;

	SerialPort sp = new SerialPort("COM3", 9600, Parity.None,8, StopBits.One);
	// Use this for initialization
	private string message="";

	float lastTime=0;

	Thread myThread;


	/* 
	void Awake() {

		foreach(string str in SerialPort.GetPortNames())
		{
		//	Debug.Log( str);
		//	sp = new SerialPort(str, 9600, Parity.None, 8, StopBits.One);
			//SerialPort.GetPortNames();
		}

		//Debug.Log (SerialPort.GetPortNames() );


	}
*/

	void Start () {



		OpenConnection ();


		//StartCoroutine("Arduino");
		//InvokeRepeating("RepeatingSend", 0.001f, 0.1f);


		myThread = new Thread(new ThreadStart(ThreadArduino));
		myThread.Start();


	}
	/*
	IEnumerator Arduino()
	{
		//actions dans ta coroutine (faisant office d' Update)
		//(exemple ici comptage des frames)

		if (Time.time > lastTime + 0.10f) {
			
			lastTime=Time.time;
			SendToSerial( m1_1, m1_2, m2_1, m2_2);
		}

		
		yield return null;
		RepeatArduino();
	}


	//Method qui répète la coroutine
	void RepeatArduino()
	{
		StartCoroutine("Arduino");
	}

*/



	private void ThreadArduino(){
		
		while(myThread.IsAlive)
		{
			SendToSerial( m1_1, m1_2, m2_1, m2_2);
			Debug.Log ("sendarduino");
			Thread.Sleep(100);
			
		}
	}



	public void sendArduino(byte m1_1,byte m2_2,byte m1_2,byte m2_1){




		SendToSerial( m1_1, m1_2, m2_1, m2_2);


	}




	void SendToSerial(params byte[] cmd)
	{



		if (outputToSerial) 
		{
			if (sp.IsOpen){
				try { 
					sp.Write( cmd, 0, cmd.Length );
				}
				catch (Exception e) 
				{
					Debug.LogWarning(e);
				}
			}
		}

	}

	void OnApplicationQuit() 
	{

		sp.Close();
		myThread.Abort ();
		Debug.Log ("stop serial");


	}


	public void OpenConnection() 
	{  
		if (sp != null) 
		{
			if (sp.IsOpen) 
			{
				sp.Close();
				message = "Closing port, because it was already open!";
			}
			else 
			{
				sp.Open();  // opens the connection
				Debug.Log ( "Port Opened!");
				sp.ReadTimeout = 50;  // sets the timeout value before reporting error
				sp.WriteTimeout=100;
				sp.DtrEnable=true;
				sp.RtsEnable=true;


			}
		}
		else 
		{
			if (sp.IsOpen)
			{
				print("Port is already open");
			}
			else 
			{
				print("Port == null");
			}
		}
	}




}
