using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class OSCSender : MonoBehaviour {

	private OSCClient _sender;

	void Start () {
		_sender = new OSCClient (System.Net.IPAddress.Parse ("127.0.0.1"), 7000);
		_sender.Connect ();
	}
	
	void Update () {
		var packet = new OSCMessage ("/test/");
		packet.Append (10);
		_sender.Send (packet);
	}
}
