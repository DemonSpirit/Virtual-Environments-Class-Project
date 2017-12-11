using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrumScript : MonoBehaviour {

    AudioClip strumClip;
    public GameObject lhand;
    public GameObject rhand;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter (Collider col)
    {
        Debug.Log("STRUM");
        if (col.gameObject == rhand || col.gameObject == lhand)
        {
            Debug.Log("STRUM R HAND");
            GetComponent<AudioSource>().Play();
        }
    }
}
