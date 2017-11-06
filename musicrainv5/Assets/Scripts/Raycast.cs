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

	private GameObject MaybeSelected;
	private GameObject ActuallySelected;

	void Start () {
		isPressed = false;
	}

	void Update () {
		RaycastHit seen;

		Ray raydirection = new Ray (transform.position, transform.forward);
		
		if (Physics.Raycast(raydirection, out seen, Mathf.Infinity)) {
			if (seen.collider.tag == "button") {
				if (lookingAt == seen.collider.name) {

					Debug.Log ("Looking at" + seen.collider.name + "...");
					// augmenter alpha zone regardée
					// les autres reviennent à 50
					if(MaybeSelected == null)
						MaybeSelected = seen.collider.gameObject;
					
					if (MaybeSelected.name != seen.collider.name) {
						MaybeSelected = seen.collider.gameObject;
						var newNormalColor = MaybeSelected.GetComponent<MeshRenderer> ().material.color;
						newNormalColor.a = 0.2f;
						MaybeSelected.GetComponent<MeshRenderer> ().material.color = newNormalColor;
					}

					var newColor = seen.collider.gameObject.GetComponent<MeshRenderer> ().material.color;
					newColor.a = 0.2f + ((Time.time - startTime) / selectDelay);
					if (newColor.a > 1) {
						newColor.a = 1; //wow
					}
					seen.collider.gameObject.GetComponent<MeshRenderer> ().material.color = newColor;

					//ActuallySelected.GetComponent<SpriteRenderer> ().color.a = 50;

					if (Time.time > startTime + selectDelay) {
						
						Debug.Log ("Selected " + seen.collider.name);
						int familySelected = Int32.Parse(seen.collider.name.Substring((seen.collider.name.Length - 1), 1));
						Debug.Log (familySelected);

						if (!isPressed) {
							Core.GetComponent<Engine>().InitializeDrops (familySelected);
							var go = GameObject.Find ("Tuto");
							if(go != null)
								go.SetActive (false);
							isPressed = true;

							// remplir Selected de la couleur
							ActuallySelected = seen.collider.gameObject;
							GameObject.Find ("Selected").GetComponent<SpriteRenderer> ().color = ActuallySelected.GetComponent<MeshRenderer> ().material.color;
							MaybeSelected = null;
							GameObject.Find ("Core").GetComponent<Engine> ().VasYFaisLe ();
						}
					}
				} else {
					startTime = Time.time;
					lookingAt = seen.collider.name;
					Debug.Log ("Started to look at " + seen.collider.name);
					isPressed = false;

				}
			} 
		}
		else {
			Debug.Log ("Looking at nothing...");
			lookingAt = "";
			if(MaybeSelected != null) {
			var newNormalColor = MaybeSelected.GetComponent<MeshRenderer> ().material.color;
			newNormalColor.a = 0.2f;
			MaybeSelected.GetComponent<MeshRenderer> ().material.color = newNormalColor;
			//GameObject.Find ("Selected").GetComponent<SpriteRenderer> ().color = Color.black;//ActuallySelected.GetComponent<MeshRenderer> ().material.color;
			}
			ActuallySelected = null;
		}
	}
}