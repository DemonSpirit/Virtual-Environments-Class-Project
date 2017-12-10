using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void Update()
    {

    }

    void onCollisionEnter(Collider col)
    {
        Debug.Log(col.gameObject);
        if (col.gameObject.tag == "TrumpetTrigger")
        {
            Debug.Log("TRIGGER");
            //transform.parent.gameObject.GetComponent<TrumpetPiece>().combineTrumpetPieces(col.transform.parent.gameObject);
        }
    }
}
