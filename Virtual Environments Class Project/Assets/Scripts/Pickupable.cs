using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseOver()
	{
		GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
	}

	void OnMouseExit()
	{
		GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
	}
}
