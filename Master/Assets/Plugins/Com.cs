﻿using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.IO;

public class Com : MonoBehaviour {

	public bool outputToSerial=true;

	public byte m1_1=0;
	public byte m1_2=0;

	public byte m2_1=0;
	public byte m2_2=0;

	SerialPort sp = new SerialPort("COM6", 9600);
	// Use this for initialization


	float lastTime=0;


	void Awake() {

		foreach(string str in SerialPort.GetPortNames())
		{
			Debug.Log( str);
			sp = new SerialPort(str, 9600);
			//SerialPort.GetPortNames();
		}

		//Debug.Log (SerialPort.GetPortNames() );


	}

	void Start () {



		sp.Open();
		//InvokeRepeating("RepeatingSend", 0.001f, 0.1f);
	}


	// Update is called once per frame
	void Update () {



		//SendToSerial((byte) 255,(byte) 255, (byte) 255);


		if (Time.time > lastTime + 0.10f) {
				
			lastTime=Time.time;
			SendToSerial( m1_1, m1_2, m2_1, m2_2);
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
				catch (System.Exception ex) 
				{
					Debug.LogWarning(ex);
				}
			}
		}
	}

}
