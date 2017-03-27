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
	private List<GameObject> drops;
	private List<Vector3> positions;

	private float minX;
	private float maxX;
	private float minZ;
	private float maxZ;
	private int actualFamily;

	void Start () {
		startTime = Time.time;
		drops = new List<GameObject> ();
		positions = new List<Vector3> ();

		minX = TopLeft.transform.position.x;
		maxX = BottomLeft.transform.position.x;
		minZ = TopLeft.transform.position.z;
		maxZ = TopRight.transform.position.z;

		actualFamily = 1;
		InitializeDrops (actualFamily);

	}
	
	void Update () {
		if (startTime + dropDelay < Time.time) {
			startTime = Time.time;
			var myFamily = GameObject.Find ("Family" + actualFamily).GetComponent<Family>();
			GenerateDrops (myFamily);
		}
	}

	public void InitializeDrops(int familySelected) {
		Debug.Log (familySelected);
		actualFamily = familySelected;
		drops = new List<GameObject> ();
		positions = new List<Vector3> ();
		var myFamily = GameObject.Find ("Family" + familySelected).GetComponent<Family>();

		GenerateDrops (myFamily);
	}

	void GenerateDrops(Family myFamily) {
		for (int i = 0; i < myFamily.drops.Length ; i++) {
			var newDrop = Instantiate (Drop);
			var newPosition = new Vector3(Random.Range(minX, maxX), height, Random.Range(minZ, maxZ));
			positions.Add(newPosition);
			newDrop.transform.position = newPosition;
			var colorIndex = myFamily.drops [i];
			newDrop.GetComponent<MeshRenderer> ().material.color = myFamily.colors [colorIndex];
			drops.Add (newDrop);
		}
		startTime = Time.time;	
	}
}
