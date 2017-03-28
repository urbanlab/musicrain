using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBehaviour : MonoBehaviour {

	public GameObject Wave;

	private MeshRenderer meshRenderer;
	private float startTime;
	public Color color;

	void Start () {
		meshRenderer = GetComponent<MeshRenderer> ();
		startTime = Time.time;
		color = this.meshRenderer.material.color;
		color.a = 0f;
		this.meshRenderer.material.color = color;
	}

	void Update () {
		if (Time.time - startTime < 1f)
			color.a = Time.time - startTime;
		else
			color.a = 1f;
		this.meshRenderer.material.color = color;
	}

	void OnTriggerEnter(Collider other) {
		var wave = Instantiate(Wave);
		wave.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.0001f, this.transform.position.z);
		wave.gameObject.GetComponent<SpriteRenderer>().material.color = meshRenderer.material.color;
	}

	void OnTriggerExit(Collider other) {
		Destroy (this.gameObject);
	}
}
