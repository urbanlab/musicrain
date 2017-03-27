using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.VR;

public class Raycast : MonoBehaviour {

	public float sightlength;

	void Start () {

	}

	void Update () {
		RaycastHit seen;

		Ray raydirection = new Ray (transform.position, transform.forward);
		
		if (Physics.Raycast(raydirection, out seen, sightlength)) {
			if (seen.collider.name == "Target1") {
				Debug.Log ("This is target number 1");
			} else if (seen.collider.name == "Target2") {
				Debug.Log ("This is target number 2");
			}

		}
	}
}