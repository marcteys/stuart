using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

		public string levelName;
		
		void OnClick ()
		{
			if (!string.IsNullOrEmpty(levelName))
			{
				NGUITools.Broadcast("End");
				Application.LoadLevel(levelName);
			}
		}

}
