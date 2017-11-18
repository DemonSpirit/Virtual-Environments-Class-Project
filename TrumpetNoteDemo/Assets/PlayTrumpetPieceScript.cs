using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTrumpetPieceScript : MonoBehaviour {

    public AudioClip trumpetSound;
    public GameObject mouth;
    bool isPlaying = false;

	// Use this for initialization
	void Start () {
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = trumpetSound;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == mouth)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
