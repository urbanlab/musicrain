using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour {

	public GameObject Drop;
	public GameObject[] Families;
    public int ActualFamily;

    public float dropDelay = 1;
	public float height = 1;

	public GameObject TopLeft;
	public GameObject TopRight;
	public GameObject BottomLeft;
	public GameObject BottomRight;

	private float startTime;
	public List<GameObject> Drops;
	private List<Vector3> positions;

	private float minX;
	private float maxX;
	private float minZ;
	private float maxZ;

    private Partition _partition;
    private int index;
    
	void Start () {
		startTime = Time.time;
		Drops = new List<GameObject> ();
		positions = new List<Vector3> ();

		minX = TopLeft.transform.position.x;
		maxX = TopRight.transform.position.x;
		minZ = TopLeft.transform.position.z;
		maxZ = BottomLeft.transform.position.z;

		ActualFamily = 1;
		InitializeDrops (ActualFamily);

        _partition = GetComponent<XMLReader>().ReadXML();
        Debug.Log("BPM : " + _partition.BPM);
	}
	
	void Update () {
		if (startTime + (60/_partition.BPM) < Time.time) { //With that we have a tempo
           
            var myFamily = GameObject.Find ("Family" + ActualFamily).GetComponent<Family>();
		    var i = 0;
		    foreach (var track in _partition.Ambiances[ActualFamily-1].Tracks)
		    {
		        if (track.Rythm[index] == '1')
		        {
                    var newDrop = Instantiate(Drop);
                    newDrop.transform.position = positions[i];
                    newDrop.GetComponent<MeshRenderer>().material.color = myFamily.colors[track.Color];
		            newDrop.GetComponent<Rigidbody>().velocity = new Vector3(0f, -height*(60 / _partition.BPM), 0f);//(60 / _partition.BPM);
                    newDrop.transform.localScale = new Vector3(0.05f, 0.05f*track.Duration/2, 0.05f);
		            newDrop.GetComponent<DropBehaviour>().Id = track.id;
                    Drops.Add(newDrop);
                }
		        i++;
		    }
		    index++;
		    if (index > 7)
		        index = 0;
            startTime = Time.time;
        }
	}

	public void InitializeDrops(int familySelected) {
		ActualFamily = familySelected;
		Drops = new List<GameObject> ();
		positions = new List<Vector3> ();
		var myFamily = GameObject.Find ("Family" + familySelected).GetComponent<Family>();

		for (int i = 0; i < myFamily.drops.Length ; i++) {
			var newPosition = new Vector3(Random.Range(minX, maxX), height, Random.Range(minZ, maxZ));
			while (newPosition.x < -0.1 && newPosition.z < 0.5) {
				newPosition = new Vector3(Random.Range(minX, maxX), height, Random.Range(minZ, maxZ));
			}
			positions.Add(newPosition);
		}
	}

	//void GenerateDrops(Family myFamily) {
	//	for (int i = 0; i < myFamily.drops.Length ; i++) {
	//		var newDrop = Instantiate (Drop);
	//		newDrop.transform.position = positions[i];
	//		var colorIndex = myFamily.drops [i];
	//		newDrop.GetComponent<MeshRenderer> ().material.color = myFamily.colors [colorIndex];
	//		Drops.Add (newDrop);
	//	}
	//	startTime = Time.time;	
	//}
}
