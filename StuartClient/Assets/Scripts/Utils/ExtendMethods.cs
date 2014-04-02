﻿using UnityEngine;
using System.Collections;




public static class ExtensionMethods {
	
	
	
	public static float Remap (this float value, float from1, float to1, float from2, float to2) {
		
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		
	}
	

	

	
//	public static float Round (float value, int digits) {
		
	//	float mult = Mathf.pow(10.0f, (float)digits);
	//	return Mathf.Round(value * mult) / mult;
		
//	}
	
	
	
}


