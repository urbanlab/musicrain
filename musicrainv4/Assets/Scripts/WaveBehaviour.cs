﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour {

	public float waveLifespan = 6;
	public float waveMaxScale = 3;

	private float startTime;
	private SpriteRenderer rend;

	void Start () {
		rend = GetComponent<SpriteRenderer>();
		startTime = Time.time;
	}
	
	void Update () {
		
		this.transform.localScale = new Vector3 ((Time.time - startTime)/5+0.05f, (Time.time - startTime)/5+0.05f, 1);

		rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 2-(Time.time - startTime));

		if (Time.time - startTime > waveLifespan) {
			Destroy (this.gameObject);
		}
	}
}
