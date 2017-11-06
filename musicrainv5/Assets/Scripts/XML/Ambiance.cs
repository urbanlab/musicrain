using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class Ambiance  {
    [XmlAttribute("id")]
    public string id;

	[XmlAttribute("length")]
	public int length;
    
    [XmlArray("Tracks")]
    [XmlArrayItem("Track")]
    public List<Track> Tracks = new List<Track>();
}
