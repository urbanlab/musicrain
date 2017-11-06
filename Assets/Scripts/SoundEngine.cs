using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class SoundEngine : MonoBehaviour
{
    public GameObject Speaker1;
    public GameObject Speaker2;
    public GameObject Player;

    public float RoomDiagonal;
    public float Volume;
    public float Pourcentage;

    private GameObject[] _audioSources;
    private Vector3 _speakerBase;
    private GameObject _playerOnBaseGameObject, _sourceOnBaseGameObject;
    private Engine _engine;

    private OSCClient myClient;
    // Use this for initialization
    void Start () {
        _audioSources = GameObject.FindGameObjectsWithTag("Audio");
        _speakerBase = Speaker1.transform.position - Speaker2.transform.position;

        _playerOnBaseGameObject = new GameObject {name = "PlayerOnSpeakerBase"};
        _sourceOnBaseGameObject = new GameObject {name = "AudioSourceOnSpeakerBase"};
        _engine = this.GetComponent<Engine>();

        myClient = new OSCClient(System.Net.IPAddress.Parse("127.0.0.1"), 9000);
        var packet = new OSCMessage("/live/name/track");
        myClient.Send(packet);
    }

    private void ComputeSpeakerValues(GameObject source)
    {
        if (source == null) return;
        
        //Compute volume
        var distancePlayerSource = (source.transform.position - Player.transform.position).magnitude;
        Volume = distancePlayerSource / RoomDiagonal;

        //Compute speaker percentage
        var sourceOnBase = Vector3.Project(source.transform.position, _speakerBase);
        _sourceOnBaseGameObject.transform.position = sourceOnBase;
        //_sourceOnBaseGameObject.transform.rotation = source.transform.rotation;
        _sourceOnBaseGameObject.transform.Rotate(new Vector3(0f, 45f, 0f));

        var distancePlayerSpeaker1 = (Speaker1.transform.position - Player.transform.position).magnitude;
        var distanceSourceSpeaker1 = (Speaker1.transform.position - source.transform.position).magnitude;

        if (distancePlayerSource > 0.2f)
            Pourcentage = distanceSourceSpeaker1 < distancePlayerSpeaker1 ? -1f : 1f;
        else
            Pourcentage = 0f;
    }

	// Update is called once per frame
	void Update ()
    {
        var playerOnBase = Vector3.Project(Player.transform.position, _speakerBase);
        _playerOnBaseGameObject.transform.position = playerOnBase;
        _playerOnBaseGameObject.transform.Rotate(new Vector3(0f, 45f, 0f));
        var i = 0;

        foreach (var source in _engine.Drops)
        {
            if (source == null) return;
            ComputeSpeakerValues(source);

         //   Debug.Log("Senfind OSC ...");

            //Start clip
            var packet = new OSCMessage("/live/play/clip");
            packet.Append(_engine.ActualFamily);
            packet.Append(source.GetComponent<DropBehaviour>().Id);
            myClient.Send(packet);

            //   /live/volume (int track, float volume(0.0 to 1.0))
            packet = new OSCMessage("/live/volume");
            packet.Append(source.GetComponent<DropBehaviour>().Id);
            packet.Append(Volume);
            myClient.Send(packet);

            // /live/master/pan        (int track, float pan(-1.0 to 1.0))             Sets master track's pan to pan
            packet = new OSCMessage("/live/master/pan");
            packet.Append(source.GetComponent<DropBehaviour>().Id);
            packet.Append(Pourcentage);
            myClient.Send(packet);
            i++;
            //var mouseDir = mousePos - charPos;
            //if (Vector3.Dot(right, mouseDir) < 0)
            //{
            //    //do right hand stuff
            //}
            //else
            //{
            //    //do left hand stuff
            //}
            //Player.transform.forward;
        }
        
      //  Debug.Log(" Closest point : " + Vector3.Project(Player.transform.position, Speaker1.transform.position - Speaker2.transform.position));
    }

    void OnApplicationQuit()
    {
        //var packet = new OSCMessage("/live/stop");
        //myClient.Send(packet);
    }
}
