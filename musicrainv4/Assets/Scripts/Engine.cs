
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Engine : MonoBehaviour {

	public GameObject Drop;
	public GameObject[] Families;
    public int ActualFamily;

    public float dropDelay;
	public float height;

	public GameObject TopLeft;
	public GameObject TopRight;
	public GameObject BottomLeft;
	public GameObject BottomRight;
    public GameObject PointUnderStandband;

	private float startTime;
	public List<GameObject> Drops;
	private List<Vector3> positions;

	private float minX;
	private float maxX;
	private float minZ;
	private float maxZ;

    private Partition _partition;
    private int index;
	private int indexMesure;
	private List<int> playingTracks;
    
	private float oldTime;

	public int Base;
	public int Step;

	public int CurrentStep = 1;
	public int CurrentMeasure;

	private float interval;
	private float nextTime;
    private List<Vector3> availablesPositions;
	private bool started;
	void Start () {
		started = false;
		startTime = Time.time;
		Drops = new List<GameObject> ();
		positions = new List<Vector3> ();
        availablesPositions = new List<Vector3>();

	    InitializeGrid();

		minX = TopLeft.transform.position.x;
		maxX = TopRight.transform.position.x;
		minZ = TopLeft.transform.position.z;
		maxZ = BottomLeft.transform.position.z;

		ActualFamily = 1;


        _partition = GetComponent<XMLReader>().ReadXML();
        Debug.Log("BPM : " + _partition.BPM);

		Base = 4;




	}
	public void VasYFaisLe() {
		if(!started) {
			//InitializeDrops (ActualFamily);
			StopCoroutine("DoTick"); // stop any existing coroutine of the metronome
			CurrentStep = 1; // start at first step of new measure
			var multiplier = Base / 4f; // base time division in music is the quarter note, which is signature base 4, so we get a multiplier based on that
			var tmpInterval = 60f / _partition.BPM; // this is a basic inverse proportion operation where 60BPM at signature base 4 is 1 second/beat so x BPM is ((60 * 1 ) / x) seconds/beat
			interval = tmpInterval / multiplier; // final interval is modified by multiplier

			nextTime = Time.time;

			playingTracks = new List<int>();

			for (var i = 0; i < _partition.Ambiances [ActualFamily-1].Tracks.Count; i++)
				playingTracks.Add(0);
			Debug.Log ("Track count : " + _partition.Ambiances [ActualFamily-1].Tracks.Count);
			var firstNote = Random.Range (0, _partition.Ambiances [ActualFamily-1].Tracks.Count-1);
			playingTracks[firstNote] = 1;

			StartCoroutine("DoTick"); // start the fun
			started = true;
		}
	}

	void GiveMeADrop () {
		var myFamily = GameObject.Find ("Family" + ActualFamily).GetComponent<Family> ();
		var i = 0;
		//Debug.Log ("Playingtrack : " + playingTracks);
		//playingTracks [i] = 1;
		foreach (var track in _partition.Ambiances[ActualFamily-1].Tracks) {
			if (playingTracks [i] == 1) {
				if (track.Rythm [index] == '1') {
					var newDrop = Instantiate (Drop);
					newDrop.transform.position = positions [i];
					newDrop.GetComponent<MeshRenderer> ().material.color = myFamily.colors [track.Color];
					//newDrop.GetComponent<Rigidbody>().velocity = new Vector3(0f, -height*(60 / _partition.BPM), 0f);//(60 / _partition.BPM);
					//newDrop.transform.localScale = new Vector3(0.05f, 0.05f*track.Duration/2, 0.05f); 
					newDrop.GetComponent<DropBehaviour> ().Id = track.id;
					newDrop.GetComponent<DropBehaviour> ().Speed = 1f;//-height * (60 / _partition.BPM);
					newDrop.GetComponent<AudioSource> ().clip = Resources.Load<AudioClip> ("Sounds/"+ _partition.Ambiances[ActualFamily - 1].id + track.id);
                    
					Drops.Add (newDrop);
				}
			}
			i++;
		}

		index++;
		if (index > 7) {
			index = 0;
			indexMesure++;
            var newNote = Random.Range(0, _partition.Ambiances[ActualFamily - 1].Tracks.Count - 1);

            if (playingTracks[newNote] == 0)
            {
                playingTracks[newNote] = 1;
            }
        }
		startTime = Time.time;
	}

	IEnumerator DoTick() // yield methods return IEnumerator
	{
		for (; ; )
		{
			//+Debug.Log("bop");
			GiveMeADrop ();
			//Debug.Log (oldTime - Time.time + 0.5f);
			
			oldTime = Time.time;
			// do something with this beat
			nextTime += interval; // add interval to our relative time
			yield return new WaitForSeconds(nextTime - Time.time); // wait for the difference delta between now and expected next time of hit
			CurrentStep++;
			if (CurrentStep > Step)
			{
				CurrentStep = 1;
				CurrentMeasure++;
			}
		}
	}

	public void InitializeDrops(int familySelected) {
		ActualFamily = familySelected;
		Drops = new List<GameObject> ();
		positions = new List<Vector3> ();
		var myFamily = GameObject.Find ("Family" + familySelected).GetComponent<Family>();
        InitializeGrid();


        for (int i = 0; i < myFamily.drops.Length ; i++) {
            //var newPosition = new Vector3(Random.Range(minX, maxX), height, Random.Range(minZ, maxZ));
            //while (newPosition.x < -0.1 && newPosition.z < 0.5) {
            //	newPosition = new Vector3(Random.Range(minX, maxX), height, Random.Range(minZ, maxZ));
            //}

            //New version
		    var rand = Random.Range(0, availablesPositions.Count);
		    var newPosition = availablesPositions[rand];
            availablesPositions.RemoveAt(rand);
			newPosition.y = height;
            positions.Add(newPosition);
		}

		playingTracks = new List<int>();
		_partition = GetComponent<XMLReader>().ReadXML();
		for (var i = 0; i < _partition.Ambiances [ActualFamily-1].Tracks.Count; i++)
			playingTracks.Add(0);

		var firstNote = Random.Range (0, _partition.Ambiances [ActualFamily-1].Tracks.Count-1);
		playingTracks [firstNote] = 1;
	}

    public void InitializeGrid()
    {
		availablesPositions = new List<Vector3> ();
        //  x  x  x  x  x
        //  x  o  o  o  x
        //  x  o  o  o  x
        //  x  o  o  o  x
        //  x  x  x  x  x

        var abscisse = TopRight.transform.position - TopLeft.transform.position;
        var ordonnee = BottomLeft.transform.position - TopLeft.transform.position;

        var entreDeuxPointsAbscisse = abscisse / 4;
        var entreDeuxPointsOrdonnee = ordonnee / 4;

        for (var i = 1; i < 4; i++)
        {
            for (var j = 1; j < 4; j++)
            {
                var realComputedPoint = TopLeft.transform.position + entreDeuxPointsOrdonnee * j +
                                        entreDeuxPointsAbscisse * i;
                var randomOnCircle = Random.onUnitSphere;
                randomOnCircle.x *= (entreDeuxPointsAbscisse.magnitude / 4); 
                randomOnCircle.y = 0;
                randomOnCircle.z *= (entreDeuxPointsOrdonnee.magnitude / 4);

                availablesPositions.Add(realComputedPoint + randomOnCircle);
                
            }
        }
        //remove under standband:
		List<int> toRemove = new List<int>();
		var l = 0;
		foreach (var point in availablesPositions) {
			if ((point - BottomRight.transform.position).magnitude < 1.25f)
				toRemove.Add (l);
			l++;
		}
		foreach(var trash in toRemove)
        	availablesPositions.RemoveAt(trash);

        //just to see how it looks like
        var k = 0;
        foreach (var obj in availablesPositions)
        {
            var Obj = new GameObject();
            Obj.transform.position = obj;
            Obj.name = "PointOnGrid" + k;
            k++;
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
