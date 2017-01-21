using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class CameraGrabber : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			Camera.main.GetComponent<FollowCam> ().SetTarget (transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
