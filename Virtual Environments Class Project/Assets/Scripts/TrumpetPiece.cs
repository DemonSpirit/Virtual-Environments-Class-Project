﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetPiece : MonoBehaviour {
	/*AudioSource audioSource;
	[SerializeField] AudioClip audioClip;*/
	[SerializeField] public int id;

	// Use this for initialization
	void Start () {
        /*if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>() as AudioSource;
		else audioSource = GetComponent<AudioSource>();*/
	}

    void FixedUpdate()
    {
        
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
