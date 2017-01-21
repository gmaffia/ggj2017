using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
	
	Transform target = null;

	public void SetTarget(Transform target){
		this.target = target;
	}

	void LateUpdate () {
		if (target != null) {
			transform.position = new Vector3 (target.position.x, target.position.y, transform.position.z);
		}
	}
}
