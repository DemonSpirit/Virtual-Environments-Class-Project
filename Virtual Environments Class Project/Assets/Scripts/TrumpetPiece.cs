using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetPiece : MonoBehaviour {
	[SerializeField] public int id;
    public bool hasBeenPickedUp = false;

	// Use this for initialization
	void Start () {

	}

    void FixedUpdate()
    {
        
    }

    public void trumpetPickedUp()
    {
        if (hasBeenPickedUp) return;
        AudioSource audios = GetComponent<AudioSource>();
        if (audios.clip != null) audios.Play();
        hasBeenPickedUp = true;
    }

	void OnCollisionEnter(Collision col)
	{
		if (col.transform.tag == "TrumpetPiece" && !TrumpetManager.singleton.isInstantiating)
		{
            Debug.Log("JOIN TRUMPETS");
			TrumpetManager.singleton.isInstantiating = true;
			TrumpetManager.singleton.InstantiateCombinedObject(gameObject, col.gameObject);
		}
	}
		

	void OnDestroy()
	{
		TrumpetManager.singleton.isInstantiating = false;
	}
}
