using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {
	public GameObject Shadow;
	public override void OnClientConnect (NetworkConnection conn)
	{
		base.OnClientConnect (conn);
		Shadow.SetActive (true);

	}
	public override void OnStartHost ()
	{
		base.OnStartHost ();
		Shadow.SetActive (true);
	}
}
