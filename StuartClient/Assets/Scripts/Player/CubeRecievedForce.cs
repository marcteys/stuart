using UnityEngine;
using System.Collections;
using System;

public class CubeRecievedForce : MonoBehaviour {


	public float force;

	ParticleSystem waveOrange;
	ParticleSystem waveBlue ;
	GameObject deg;

	private Color colorOrange = new Color(1,103.Remap(0f, 255f, 0f, 1f),0);
	private Color colorBlue = new Color(0,115.Remap(0f, 255f, 0f, 1f),1);
	
	public float multiplicateurForce = 100f;


	// Use this for initialization
	void Start () {
		 waveOrange = this.transform.Find ("WaveOrange").gameObject.GetComponent<ParticleSystem>();
		 waveBlue = this.transform.Find ("WaveBlue").gameObject.GetComponent<ParticleSystem>();
		 deg = GameObject.Find (this.transform.name+"/deg");
	}
	
	// Update is called once per frame
	void Update () {
		if(force > 0.5f) { // REPULTION 
			
			float textbas = (float)Math.Round((double)multiplicateurForce*force.Remap(0.5f, 1f, 0f, 1f),2);
			
			
			
			waveBlue.startSize = 0;
			waveOrange.startSize = textbas/60;
			
			Color colorDeg = colorOrange;
			colorDeg.a = textbas/multiplicateurForce - 0.3f;
			
			deg.renderer.material.SetColor("_TintColor",colorDeg);
			
		} else if(force < 0.5f) { // ATRACTION
			
			float textbas = (float)Math.Round((double)multiplicateurForce*force.Remap(0f, 0.5f, 1f, 0f),2);
			
			//total force
			waveOrange.startSize = 0;
			waveBlue.startSize = textbas/60;
			
			Color colorDeg = colorBlue;
			colorDeg.a = textbas/multiplicateurForce - 0.3f;
			
			deg.renderer.material.SetColor("_TintColor",colorDeg);
			
			
		} else {
			waveOrange.startSize = 0;
			waveBlue.startSize = 0;
		}
	}
}
