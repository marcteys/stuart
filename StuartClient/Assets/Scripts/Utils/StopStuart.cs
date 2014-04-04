using UnityEngine;
using System.Collections;

public class StopStuart : MonoBehaviour {

	public UDPSend udpScript;


	// Use this for initialization
	public void EmercengyStop () {
		udpScript.sendString("emergency");
	}
	

}
