using UnityEngine;
using System.Collections;

public class ReScan : MonoBehaviour {

	public UDPSend udpScript;


	// Use this for initialization
	public void Rescan () {
		udpScript.sendString("rescan");
	}
	

}
