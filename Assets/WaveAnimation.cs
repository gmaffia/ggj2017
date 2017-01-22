using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAnimation : MonoBehaviour {
	public float duration = 1;
	public float counter = 0;
	public float speed = 1;
	SpriteRenderer render;
	// Use this for initialization
	void Start () {
		counter = duration;
		render = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		counter -= Time.deltaTime;
		float percent = counter / duration;
		transform.localScale = transform.localScale + Vector3.one*speed* Time.deltaTime;
		if (percent > 0.5) {
			render.color = new Color (1, 1, 1, 2 - percent*2);
		} else {
			render.color = new Color (1, 1, 1, percent * 2);
		}
		if (counter < 0) {
			Destroy (gameObject);
		}
	}
}
