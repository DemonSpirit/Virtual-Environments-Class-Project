using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handscript : MonoBehaviour {

    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftHandModel;
    public GameObject rightHandModel;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update ()
    {
        leftHandModel.GetComponent<Animator>().SetBool("isGrabbing", SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost)).GetHairTrigger());
        rightHandModel.GetComponent<Animator>().SetBool("isGrabbing", SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost)).GetHairTrigger());

        GameObject leftHeldObject = leftHand.GetComponent<Valve.VR.InteractionSystem.Hand>().currentAttachedObject;
        GameObject rightHeldObject = rightHand.GetComponent<Valve.VR.InteractionSystem.Hand>().currentAttachedObject;
        if (leftHeldObject != null && rightHeldObject != null)
        {
            if (leftHeldObject.tag == "TrumpetPiece" && rightHeldObject.tag == "TrumpetPiece")
            {
                float dist = Vector3.Distance(leftHeldObject.transform.position, rightHeldObject.transform.position);
                if (dist < 0.1f)
                {
                    TrumpetManager.singleton.InstantiateCombinedObject(leftHeldObject, rightHeldObject);
                }
            }
        }
    }
}
