using UnityEngine;
using System.Collections;

public class ChangeIP : MonoBehaviour {

	private UDPSend sendScript;
	public UILabel uiLabel;

	// Use this for initialization
	void Start () {
		sendScript = GameObject.Find ("_UDPSend").GetComponent<UDPSend> ();
		uiLabel.text = sendScript.IP;
	}
	
	// Update is called once per frame
	public void ChangeIPAddress () {
		sendScript.IP = uiLabel.text;
		sendScript.init();
	}
}
