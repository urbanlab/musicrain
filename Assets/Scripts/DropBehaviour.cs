using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBehaviour : MonoBehaviour {

	public GameObject Wave;
	private MeshRenderer meshRenderer;

	void Start () {
		meshRenderer = GetComponent<MeshRenderer> ();
	}
	
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		var wave = Instantiate(Wave);
		wave.transform.position = new Vector3(this.transform.position.x, 0.0001f, this.transform.position.z);
		wave.gameObject.GetComponent<SpriteRenderer>().material.color = meshRenderer.material.color;
	}

	void OnTriggerExit(Collider other) {
		Destroy (this.gameObject);
	}
}
