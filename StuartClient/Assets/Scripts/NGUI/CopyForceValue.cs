using UnityEngine;
using System.Collections;

public class CopyForceValue : MonoBehaviour {

	public ChangeForce changeForceScript;
	private UISlider thisSlider;

	// Use this for initialization
	void Start () {
		thisSlider = this.gameObject.GetComponent<UISlider>();
	}
	
	// Update is called once per frame
	void Update () {
		thisSlider.value = changeForceScript.force;
	}
}
