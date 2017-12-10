using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinylRecordScript : MonoBehaviour {

    public AudioClip song;
    public GameObject recordPlayer;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void checkIsOnPlayer()
    {
        Bounds vinylBounds = GetComponent<Collider>().bounds;
        Bounds playerBounds = recordPlayer.transform.Find("teller").GetComponent<Collider>().bounds;
        if (vinylBounds.Intersects(playerBounds))
        {
            recordPlayer.GetComponent<RecordPlayer>().addVinyl(gameObject);
        }
    }
}
