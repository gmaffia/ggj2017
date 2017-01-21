using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {
	public GameObject Shadow;
	public override void OnClientConnect (NetworkConnection conn)
	{
		base.OnClientConnect (conn);
		Instantiate (Shadow);
	}
	public override void OnStartHost ()
	{
		base.OnStartHost ();
		Instantiate (Shadow);
	}
}
