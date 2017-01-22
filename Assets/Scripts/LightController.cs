using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class LightController : NetworkBehaviour
{
	
	public Transform light;
	public float SmallRadius = 125f;
	public float BigRadius = 250f;
	public float BigRadiusDuration = 2f;
	private float BigRadiusCounter = 0f;
	public float RadiusFlickerRange = 15f;
	public float RadiusFlickerSpeed = 1f;

	// Use this for initialization
	void Start () {
		if(isLocalPlayer)
		{
			light.gameObject.SetActive (true);
		}
	}
	public void grow(){
		BigRadiusCounter = BigRadiusDuration;
	}
	// Update is called once per frame
	void Update () {
		if(isLocalPlayer)
		{
			light.localScale +=  new Vector3(1,1,0.1f) * Mathf.Sin ( Time.time * RadiusFlickerSpeed ) * RadiusFlickerRange;


			if (BigRadiusCounter > 0) {
				BigRadiusCounter -= Time.deltaTime;
				light.localScale = Vector3.Lerp( light.localScale, BigRadius * new Vector3(1,1,0.1f), 0.03f);
			} else {
				light.localScale = Vector3.Lerp( light.localScale, SmallRadius * new Vector3(1,1,0.1f), 0.03f);
			}

		}

	}
}
