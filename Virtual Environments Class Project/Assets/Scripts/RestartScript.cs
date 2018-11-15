using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartScript : MonoBehaviour {

	float timer = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (timer < 5.0f)
		{
			timer += Time.fixedTime;
		} else {
			Application.LoadLevel(0);
		}
	}
}
