using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Partition")]
public class Partition {

    public int BPM;

    [XmlArray("Ambiances")]
    [XmlArrayItem("Ambiance")]
    public List<Ambiance> Ambiances = new List<Ambiance>();

    public static Partition Load(string path)
    {
        XmlRootAttribute xRoot = new XmlRootAttribute();
        xRoot.ElementName = "Partition";
        xRoot.IsNullable = true;

        var serializer = new XmlSerializer(typeof(Partition), xRoot);
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Partition;
        }
    }
}
