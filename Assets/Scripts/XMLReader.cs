using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLReader : MonoBehaviour
{
    public Partition Partition;

    void Start() {
        Partition = Partition.Load(Path.Combine(Application.dataPath, "Resources/config.xml"));
        Debug.Log("XML read !");
        //Debug.Log("Nb famille : " + Partition.Families.Count);
        //foreach (var family in Partition.Families)
        //    foreach (var track in family.Tracks)
        //        Debug.Log("Track rythm : " + track.Rythm);
    }
}