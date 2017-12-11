using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthPieceScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Mouth")
        {
            TrumpetManager.singleton.playCompleteTrumpet(transform.parent.GetComponent<AudioSource>());
        }
    }
}
