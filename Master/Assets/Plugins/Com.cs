using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.IO;
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
		StartCoroutine("Arduino");
		//InvokeRepeating("RepeatingSend", 0.001f, 0.1f);
	}

	IEnumerator Arduino()
	{
		//actions dans ta coroutine (faisant office d' Update)
		//(exemple ici comptage des frames)

		if (Time.time > lastTime + 0.20f) {
			
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
				catch (System.Exception ex) 
				{
					Debug.LogWarning(ex);
				}
			}
		}
	}

	void OnApplicationQuit() 
	{
		sp.Close();
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
				sp.ReadTimeout = 50;  // sets the timeout value before reporting error
				message = "Port Opened!";
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
