public var UDPHost : String = "127.0.0.1";
private var listenerPort : int = 8000;
public var broadcastPort : int = 57131;
private var oscHandler : Osc;

private var eventName : String = "";
private var eventData : String = "";
private var counter : int = 0;
public var output_txt : GUIText;

public var messageData : String;
var udp : UDPPacketIO;

public function Start ()
{	
	udp  = GetComponent("UDPPacketIO");
	udp.init(UDPHost, broadcastPort, listenerPort);
	oscHandler = GetComponent("Osc");
	oscHandler.init(udp);

}

function Update () {	
counter++;
messageData=counter.ToString("G4");
    sendMessage(messageData);
}	

public function sendMessage(message : String) : void
{	
    oscHandler.Send(Osc.StringToOscMessage(message)); 

} 


function OnApplicationQuit () {
	sendMessage("##end##");
	udp.Close();
}