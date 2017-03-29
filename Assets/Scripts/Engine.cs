using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour {

	public GameObject Drop;
	public GameObject[] Families;
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

	private XMLReader _myReader;

	private float BPM;
	private float dropDelay;

	private int[][] dropRythm;

	void Start () {
		startTime = Time.time;
		drops = new List<GameObject> ();
		positions = new List<Vector3> ();

		minX = TopLeft.transform.position.x;
		maxX = TopRight.transform.position.x;
		minZ = TopLeft.transform.position.z;
		maxZ = BottomLeft.transform.position.z;

		actualFamily = 1;
		InitializeDrops (actualFamily);

		_myReader = GetComponent<XMLReader>();
		//Debug.Log ("3 " + _myReader._myPartition.BPM);

		//BPM = _myReader._myPartition.BPM;
		BPM = 120;

		dropDelay = 60/BPM;
		Debug.Log ("dropDelay " + dropDelay);

	}
	
	void Update () {
		if (startTime + dropDelay < Time.time) {
			startTime = Time.time;
			var myFamily = GameObject.Find ("Family" + actualFamily).GetComponent<Family>();
			GenerateDrops (actualFamily, myFamily);
		}
	}

	public void InitializeDrops(int familySelected) {
		Debug.Log (familySelected);
		actualFamily = familySelected;
		drops = new List<GameObject> ();
		positions = new List<Vector3> ();
		var myFamily = GameObject.Find ("Family" + familySelected).GetComponent<Family>();

		for (int i = 0; i < myFamily.drops.Length ; i++) {
			var newPosition = new Vector3(Random.Range(minX, maxX), height, Random.Range(minZ, maxZ));
			while (newPosition.x < -0.1 && newPosition.z < 0) {
				newPosition = new Vector3(Random.Range(minX, maxX), height, Random.Range(minZ, maxZ));
			}
			positions.Add(newPosition);
		}

		GenerateDrops (familySelected, myFamily);
	}

	void GenerateDrops(int familySelected, Family myFamily) {
		for (int i = 0; i < myFamily.drops.Length ; i++) {
			var newDrop = Instantiate (Drop);
			newDrop.transform.position = positions[i];
			var colorIndex = myFamily.drops [i];
			newDrop.GetComponent<MeshRenderer> ().material.color = myFamily.colors [colorIndex];
			drops.Add (newDrop);
			Debug.Log ("1 " + _myReader);
			//Debug.Log ("2 " + _myReader._myPartition);
			//Debug.Log ("Track duration : " + XMLReader.Partition.Families[familySelected].Tracks[i].Duration);
		}
		startTime = Time.time;	
	}
}
