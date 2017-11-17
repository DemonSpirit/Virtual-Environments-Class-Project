using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetPiece : MonoBehaviour {
	AudioSource audioSource;
	[SerializeField] AudioClip audioClip;
	[SerializeField] GameObject combinedObject;


	// Use this for initialization
	void Start () {
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>() as AudioSource;
		else audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CombineObjects(Transform other)
	{
		if (TrumpetManager.isCombiningPieces)
			return;

		if (audioClip != null)
		{
			audioSource.PlayOneShot(audioClip);
		}

		TrumpetManager.isCombiningPieces = true;


		Instantiate(combinedObject, other.position, other.rotation);
		Destroy(other.gameObject);
		Destroy(gameObject);

	}

	void OnCollisionEnter(Collision col)
	{
		if (col.transform.tag == "TrumpetPiece")
		{
			CombineObjects(col.transform);
		}
	}

	void OnDestroy()
	{
		TrumpetManager.isCombiningPieces = false;
		print(TrumpetManager.isCombiningPieces);
	}
}
