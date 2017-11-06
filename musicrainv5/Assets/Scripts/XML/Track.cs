using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class Track {

    [XmlAttribute("id")]
    public int id;

    public float Duration;
    public int Color;
    public string Rythm;
}