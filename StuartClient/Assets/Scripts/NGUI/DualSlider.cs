using UnityEngine;
using System.Collections;

public class DualSlider : MonoBehaviour {

	public UISlider DistantSlider;
	public UISlider ThisSlider;

	private float sliderVal = 0;
	// Use this for initialization

	public bool positive;
	public bool negative;

	// Update is called once per frame
	void Update () {
		sliderVal = DistantSlider.value;

		if(positive) {
			if(DistantSlider.value >= 0.51f) {
				ThisSlider.value = DistantSlider.value.Remap(0.5f, 1f, 0f, 1f);
			} else {
				ThisSlider.value  = 0f;
			}
		} else if(negative) {
			if(DistantSlider.value <= 0.49f) {
				ThisSlider.value = DistantSlider.value.Remap(0f, 0.5f, 1f, 0f);
			} else {
				ThisSlider.value  = 0f;
			}
		} else {
			Debug.Log ("Problem !! ");
		}

	}






}





/*
 * 
 * decimal res = 2.Map(1, 3, 0, 10);
// res will be 5

*/

