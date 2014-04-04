using UnityEngine; 
using System.Collections; 
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text; 

public class SavePositionCamera : MonoBehaviour {

	Rect _Save, _Load, _SaveMSG, _LoadMSG; 
	bool _ShouldSave, _ShouldLoad,_SwitchSave,_SwitchLoad; 
	string _FileLocation,_FileName; 
	public GameObject _Player; 
	UserData myData; 
	string _PlayerName; 
	string _data; 
	
	Vector3 VPosition; 
	Vector3 VRotation; 

	public GameObject cam;
	// Use this for initialization
	void Start () {


		_FileLocation=Application.dataPath; 
		_FileName="SaveData.xml"; 
		myData=new UserData(); 
	}
	
	// Update is called once per frame
	void Update () {


	//	Debug.Log (cam.gameObject.transform.position.x) ;



		//transform.position = new Vector3 (cam.gameObject.transform.position.x, cam.gameObject.transform.position.y, cam.gameObject.transform.position.z);


	
	
	}

	void OnGUI () {
		// Make a background box
		GUI.Box(new Rect(10,10,100,90), "Position Cam");
		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(20,40,80,20), "Save Pos.")) {

			Debug.Log(gameObject.transform.position.x);


			//GUI.Label(_SaveMSG,"Saving to: "+_FileLocation); 
			myData._iUser.x=gameObject.transform.position.x; 
			myData._iUser.y=gameObject.transform.position.y; 
			myData._iUser.z=gameObject.transform.position.z; 

			myData._iUser.rx=gameObject.transform.eulerAngles.x; 
			myData._iUser.ry=gameObject.transform.eulerAngles.y; 
			myData._iUser.rz=gameObject.transform.eulerAngles.z;     
			
			// Time to creat our XML! 
			_data = SerializeObject(myData); 
			// This is the final resulting XML from the serialization process 
			CreateXML(); 
			Debug.Log(_data); 



		}
		
		// Make the second button.
		if(GUI.Button(new Rect(20,70,80,20), "Load Pos.")) {
		


			
			
			GUI.Label(_LoadMSG,"Loading from: "+_FileLocation); 
			// Load our UserData into myData 
			LoadXML(); 
			if(_data.ToString() != "") 
			{ 
				// notice how I use a reference to type (UserData) here, you need this 
				// so that the returned object is converted into the correct type 
				myData = (UserData)DeserializeObject(_data); 
				// set the players position to the data we loaded 
				VPosition=new Vector3(myData._iUser.x,myData._iUser.y,myData._iUser.z);
				VRotation=new Vector3(myData._iUser.rx,myData._iUser.ry,myData._iUser.rz);

				Debug.Log (VPosition);
				gameObject.transform.position=VPosition; 
				gameObject.transform.eulerAngles=VRotation; 
				//Debug.Log (Quaternion.Euler(VRotation));
				// just a way to show that we loaded in ok 

			} 




		}
	}


	string UTF8ByteArrayToString(byte[] characters) 
	{      
		UTF8Encoding encoding = new UTF8Encoding(); 
		string constructedString = encoding.GetString(characters); 
		return (constructedString); 
	} 
	
	byte[] StringToUTF8ByteArray(string pXmlString) 
	{ 
		UTF8Encoding encoding = new UTF8Encoding(); 
		byte[] byteArray = encoding.GetBytes(pXmlString); 
		return byteArray; 
	} 
	
	// Here we serialize our UserData object of myData 
	string SerializeObject(object pObject) 
	{ 
		string XmlizedString = null; 
		MemoryStream memoryStream = new MemoryStream(); 
		XmlSerializer xs = new XmlSerializer(typeof(UserData)); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		xs.Serialize(xmlTextWriter, pObject); 
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream; 
		XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray()); 
		return XmlizedString; 
	} 
	
	// Here we deserialize it back into its original form 
	object DeserializeObject(string pXmlizedString) 
	{ 
		XmlSerializer xs = new XmlSerializer(typeof(UserData)); 
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		return xs.Deserialize(memoryStream); 
	} 
	
	// Finally our save and load methods for the file itself 
	void CreateXML() 
	{ 
		StreamWriter writer; 
		FileInfo t = new FileInfo(_FileLocation+"\\"+ _FileName); 
		if(!t.Exists) 
		{ 
			writer = t.CreateText(); 
		} 
		else 
		{ 
			t.Delete(); 
			writer = t.CreateText(); 
		} 
		writer.Write(_data); 
		writer.Close(); 
		//Debug.Log("File written."); 
	} 
	
	void LoadXML() 
	{ 
		StreamReader r = File.OpenText(_FileLocation+"\\"+ _FileName); 
		string _info = r.ReadToEnd(); 
		r.Close(); 
		_data=_info; 
		//Debug.Log("File Read"); 
	} 



}



// UserData is our custom class that holds our defined objects we want to store in XML format 
public class UserData 
{ 
	// We have to define a default instance of the structure 
	public DemoData _iUser; 
	// Default constructor doesn't really do anything at the moment 
	public UserData() { } 
	
	// Anything we want to store in the XML file, we define it here 
	public struct DemoData 
	{ 
		public float x; 
		public float y; 
		public float z; 


		public float rx; 
		public float ry; 
		public float rz; 
	} 
}
