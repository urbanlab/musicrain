using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLReader : MonoBehaviour
{
    public Partition _myPartition;

    void Start() {
		_myPartition = Partition.Load(Path.Combine(Application.dataPath, "Resources/config.xml"));
        Debug.Log("XML read !");
        Debug.Log("Nb famille : " + _myPartition.Families.Count);
		foreach (var family in _myPartition.Families)
			foreach (var track in family.Tracks) {
				Debug.Log ("Track duration : " + track.Duration);
				Debug.Log ("Track rythm : " + track.Rythm);
			}
    }
}