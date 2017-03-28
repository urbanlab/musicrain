using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.VR;

public class Raycast : MonoBehaviour {

	public float selectDelay;

	private float startTime;
	private string lookingAt;

	private bool isPressed;

	public GameObject Core;

	void Start () {
		isPressed = false;
	}

	void Update () {
		RaycastHit seen;

		Ray raydirection = new Ray (transform.position, transform.forward);
		
		if (Physics.Raycast(raydirection, out seen, Mathf.Infinity)) {
			if (seen.collider.tag == "button") {
				if (lookingAt == seen.collider.name) {
					Debug.Log ("Keep looking at " + seen.collider.name + "...");
					if (Time.time > startTime + selectDelay) {
						
						Debug.Log ("Selected " + seen.collider.name);
						int familySelected = Int32.Parse(seen.collider.name.Substring((seen.collider.name.Length - 1), 1));
						Debug.Log (familySelected);

						if (!isPressed) {
							Core.GetComponent<Engine>().InitializeDrops (familySelected);
							isPressed = true;
						}
					}
				} else {
					startTime = Time.time;
					lookingAt = seen.collider.name;
					Debug.Log ("Started to look at " + seen.collider.name);
					isPressed = false;
				}
			} else {
				Debug.Log ("Looking at nothing...");
				lookingAt = "";
			}
		}
	}
}