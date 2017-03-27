using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBehaviour : MonoBehaviour {

	public GameObject Wave;
	private MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		//Debug.Log (this.name + " : " + transform.position);

		var wave = Instantiate(Wave);
		wave.transform.position = new Vector3(this.transform.position.x, 0.0001f, this.transform.position.z);
		wave.gameObject.GetComponent<SpriteRenderer>().material.color = meshRenderer.material.color;
	}

	void OnTriggerExit(Collider other) {
		Destroy (this.gameObject);
	}
}
