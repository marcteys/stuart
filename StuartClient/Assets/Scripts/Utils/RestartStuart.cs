using UnityEngine;
using System.Collections;

public class RestartStuart : MonoBehaviour {

	public UDPSend udpScript;


	// Use this for initialization
	public void Restart () {
		udpScript.sendString("restart");
	}
	

}
