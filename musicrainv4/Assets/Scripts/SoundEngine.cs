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
		RoomDiagonal = (GameObject.Find ("TopLeft").transform.position - GameObject.Find ("BottomRight").transform.position).magnitude;
    }

    private void ComputeSpeakerValues(GameObject source)
    {
        var SourceOnGroundPosition = source.transform.position;
        SourceOnGroundPosition.y = 0f;
        //Compute volume
        var distancePlayerSource = (SourceOnGroundPosition - Player.transform.position).magnitude;
        Volume = 1-(distancePlayerSource / RoomDiagonal) - 0.2f;

        //Compute speaker percentage
        var sourceOnBase = Vector3.Project(source.transform.position, _speakerBase);
        _sourceOnBaseGameObject.transform.position = sourceOnBase;
        
        var distancePlayerSpeaker1 = (Speaker1.transform.position - Player.transform.position).magnitude;
        var distanceSourceSpeaker1 = (Speaker1.transform.position - SourceOnGroundPosition).magnitude;
        //Debug.Log("Distance to source : " + distancePlayerSource);
        if (distancePlayerSource > 0.2f)
        {
            Pourcentage = distanceSourceSpeaker1 < distancePlayerSpeaker1 ? -1f : 1f;
            Volume += 0.2f;
        }
        else
            Pourcentage = 0f;

        source.GetComponent<AudioSource>().volume = Volume;
        source.GetComponent<AudioSource>().panStereo = Pourcentage;
    }

	// Update is called once per frame
	void Update ()
    {
        var playerOnBase = Vector3.Project(Player.transform.position, _speakerBase);
        _playerOnBaseGameObject.transform.position = playerOnBase;
        var i = 0;

        foreach (var source in _engine.Drops)
        {
            if (source == null) return;
            ComputeSpeakerValues(source);
        }
    }
}
