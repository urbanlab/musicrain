using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLReader : MonoBehaviour
{
    public Partition MyPartition;

    public Partition ReadXML() {
        MyPartition = Partition.Load(Path.Combine(Application.dataPath, "Resources/config.xml"));
        Debug.Log("XML read !");
        //Debug.Log("Nb famille : " + MyPartition.Ambiances.Count);
        //foreach (var family in MyPartition.Ambiances)
        //    foreach (var track in family.Tracks)
        //        Debug.Log("Track rythm : " + track.Rythm);

        return MyPartition;
    }
}