using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour {

	public GameObject Drop;

	public GameObject[] Families;

	public float dropDelay = 1;

	public float height = 1;

	public GameObject TopLeft;
	public GameObject TopRight;
	public GameObject BottomLeft;
	public GameObject BottomRight;

	private float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		//var ambiance = 1;
	}
	
	// Update is called once per frame
	void Update () {

		if (startTime + dropDelay < Time.time) {
			startTime = Time.time;
			var drop = Instantiate(Drop);
			drop.transform.position = new Vector3(Random.Range(TopLeft.transform.position.x, BottomLeft.transform.position.x), height, Random.Range(TopLeft.transform.position.z, TopRight.transform.position.z));
		}
	}
}
