using UnityEngine;
using System.Collections;
using System;

public class ChangeForce : MonoBehaviour {

	//general proprieties
	public bool ActiveCube = false;
	public float force = 0f;
	public string carTag = "Car";

	//right elements
	private UISlider forceSlider;
	private UILabel labelCubes;

	// slider bas gauche
	private UISlider sliderBas;
	private UISprite sliderBasForeground;
		//color
	private Color colorOrange = new Color(1,103.Remap(0f, 255f, 0f, 1f),0);
	private Color colorBlue = new Color(0,115.Remap(0f, 255f, 0f, 1f),1);
		//label bas
	private UILabel labelBas;
	public float multiplicateurForce = 100f;
	private UILabel cubeNameLabel;
		//cube transform
	private Transform cubeWire;
		// label positioncube
	private UILabel positionCubeLabel;
		
	// verif pour prendre la valeur du slider une seule fois
	private bool verifOnce = false;

	//the two particle systems
	private ParticleSystem waveOrange;
	private ParticleSystem waveBlue;

	//returnet force
	public float totalForce;

	//Sender
	private UDPSend udpSender;
	public bool verifEnvoie = true;

	//visual effect
	private GameObject deg;

	//active state
	private GameObject activeCube;
	public float animSpeed = 1.0f;
	public bool animOnce = false;

	void Start () {

		forceSlider = GameObject.Find ("MainSlider").GetComponent<UISlider> ();
		//labelCubes = GameObject.Find ("LabelCubes").GetComponent<UILabel> ();
		cubeWire =  GameObject.Find ("CubeWire").transform;

		sliderBas = GameObject.Find ("ForceDiameterSlider").GetComponent<UISlider>();
		labelBas = GameObject.Find ("ForceDiameterLabel").GetComponent<UILabel>();
		sliderBasForeground = GameObject.Find ("ForceDiameterSlider/Foreground").GetComponent<UISprite>();
		cubeNameLabel = GameObject.Find ("CubeNameLabel").GetComponent<UILabel>();

		//label cube Position
		positionCubeLabel = GameObject.Find ("PositionCubeLabel").GetComponent<UILabel>();

		//getParticle
		waveOrange = this.transform.Find ("WaveOrange").gameObject.GetComponent<ParticleSystem>();
		waveBlue = this.transform.Find ("WaveBlue").gameObject.GetComponent<ParticleSystem>();

		//udp sender

		udpSender = GameObject.Find("_UDPSend").GetComponent<UDPSend>();

		deg = GameObject.Find (this.transform.name+"/deg");

		activeCube = GameObject.Find(this.transform.name+"/active");

	}
	
	void Update () {

		if (ActiveCube) {

			if(!animOnce) {

				activeCube.animation["Click"].speed = animSpeed;
				//activeCube.animation["Click"].time = activeCube.animation["Click"].length;
				activeCube.animation.CrossFade("Click");
				animOnce = true;

			}


			force = forceSlider.value;

			//multiplicateurForce
			sliderBas.value =  force;

		
			
			if(force > 0.5f) { // REPULTION 
				sliderBas.value = force.Remap(0.5f, 1f, 0f, 1f);
				sliderBasForeground.color = colorOrange;

				float textbas = (float)Math.Round((double)multiplicateurForce*force.Remap(0.5f, 1f, 0f, 1f),2);
				labelBas.text = textbas.ToString();


				//total force
				waveBlue.startSize = 0;
				waveOrange.startSize = textbas/60;

				Color colorDeg = colorOrange;
				colorDeg.a = textbas/multiplicateurForce - 0.3f;

				deg.renderer.material.SetColor("_TintColor",colorDeg);

			} else if(force < 0.5f) { // ATRACTION
				sliderBas.value = force.Remap(0f, 0.5f, 1f, 0f);
				sliderBasForeground.color = colorBlue;
						
				float textbas = (float)Math.Round((double)multiplicateurForce*force.Remap(0f, 0.5f, 1f, 0f),2);
				labelBas.text = textbas.ToString();

				
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



			//ici envoyer le UDP
		
		} //end active force 
		else {
				//fin anim
			if(animOnce) {
				activeCube.animation["Click"].time = activeCube.animation["Click"].length;

				activeCube.animation["Click"].speed = -animSpeed;
				activeCube.animation.Play("Click");
				animOnce = false;
			}

		}

		totalForce = (float)Math.Round((double)force.Remap(0f, 1f, -1f, 1f),2);


		if(ActiveCube && verifEnvoie && Input.GetMouseButton(0)) {
			Invoke("SendData",0.4f);
			verifEnvoie = false;
		}


		if(Input.GetMouseButtonUp(0)) {
			SendData();
		}

			

	
	
	}
	/*
	void OnTriggerEnter(Collider other) {
		if(other.CompareTag(carTag)){
			Debug.Log ("Car is enter !");

			//here apply force vector
			if(force >= 0.5f) {
				float newforce = (float)Math.Round((double)force.Remap(0.5f, 1f, 0f, 1f),2);
			} else if(force < 0.5f) {
				float newforce = (float)Math.Round((double)multiplicateurForce*force.Remap(0f, 0.5f, 1f, 0f),2);
			}




		}
	}
*/

	public void SendData() {

		Debug.Log ("senddata");
	//	if(verifEnvoie) {
			//for(int i = 0; i < 3 ; i++) {
				udpSender.sendString("CubeForce/"+this.transform.name+"/"+totalForce+"\n");
				Debug.Log ("Envoi "+this.transform.name+" / " + totalForce );
			//}
			verifEnvoie = true;
	//	} else {

	//		verifEnvoie = true;

	//	}

	}

	public void Activate() {
		forceSlider.value = force;
		ActiveCube = true;
		cubeWire.eulerAngles = new Vector3(350, 10, 0);


		//change labels

		cubeNameLabel.text = "CUBE "+this.transform.parent.name;

		positionCubeLabel.text = "POS\nx : "+
					(float)Math.Round((double)transform.position.x,2)+"\ny : "+
					(float)Math.Round((double)transform.position.y,2)+"\nz : "+
					(float)Math.Round((double)transform.position.z,2);


	}

	public void Desactivate() {
		ActiveCube = false;
	}
}
