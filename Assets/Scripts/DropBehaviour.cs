using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBehaviour : MonoBehaviour {

	public GameObject Wave;
    public int Id = 1;

	private MeshRenderer meshRenderer;
	private float startTime;
	public Color color;
    public float Speed;
    public float CreationTime;

	void Start () {
		meshRenderer = GetComponent<MeshRenderer> ();
		startTime = Time.time;
		color = this.meshRenderer.material.color;
		color.a = 0f;
		//this.meshRenderer.material.color = color;
	}

	void Update () {
        if (Time.time - startTime < 1f)
        {
            color.a = Time.time - startTime;
            this.meshRenderer.material.color = color;
        }

       // transform.position = new Vector3(transform.position.x, transform.position .y- Speed, transform.position.z);
	}

	void OnTriggerEnter(Collider other) {
	    if (other.name == "Floor")
	    {
	        var wave = Instantiate(Wave);
	        wave.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.0001f,
	            this.transform.position.z);
	        wave.gameObject.GetComponent<SpriteRenderer>().material.color = meshRenderer.material.color;
	    }
	}

	void OnTriggerExit(Collider other) {
        if (other.name == "Floor")
            Destroy (this.gameObject);
	}
}
