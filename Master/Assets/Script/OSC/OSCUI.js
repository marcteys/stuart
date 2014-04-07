private var sender  : OSCSender;

public var displayUI : boolean = true;
function Awake() {
	sender = this.GetComponent(OSCSender);
}
function OnGUI () {
 
	 if(displayUI) {
	    
	    if (GUI.Button (Rect (180,30,150,150), "Up")) {
	 		sender.sendMessage("up");
	    }
	     if (GUI.Button (Rect (30,180,150,150), "Left")) {
	 		sender.sendMessage("left");
	    }
	    if (GUI.Button (Rect (330,180,150,150), "Right")) {
	 		sender.sendMessage("right");
	    }
	   	if (GUI.Button (Rect (180,330,150,150), "Down")) {
	 		sender.sendMessage("down");
	    }
	}
}