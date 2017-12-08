using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handscript : MonoBehaviour {

    public GameObject leftHand;
    public GameObject rightHand;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update ()
    {
        leftHand.GetComponent<Animator>().SetBool("isGrabbing", SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost)).GetHairTrigger());
        rightHand.GetComponent<Animator>().SetBool("isGrabbing", SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost)).GetHairTrigger());
    }
}
