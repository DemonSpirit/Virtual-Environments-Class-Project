using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handscript : MonoBehaviour {

    public GameObject leftHand;
    public GameObject rightHand;

    int leftHandIndex;
    int rightHandIndex;

    public GameObject leftHandModel;
    public GameObject rightHandModel;

    public bool leftHandTriggerDown = false;
    public bool rightHandTriggerDown = false;
    bool prevLeftTrigger = false;
    bool prevRightTrigger = false;

    [HideInInspector] public static Handscript singleton;

    // Use this for initialization
    void Start () {

        if (singleton == null)
            singleton = this;
    }
	
	// Update is called once per frame
	void Update ()
    {
        leftHandIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        rightHandIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);

        bool currLeftTrigger = SteamVR_Controller.Input(leftHandIndex).GetHairTrigger();
        bool currRightTrigger = SteamVR_Controller.Input(rightHandIndex).GetHairTrigger();

        leftHandTriggerDown = (currLeftTrigger && !prevLeftTrigger);
        rightHandTriggerDown = (currRightTrigger && !prevRightTrigger);

        leftHandModel.GetComponent<Animator>().SetBool("isGrabbing", SteamVR_Controller.Input(leftHandIndex).GetHairTrigger());
        rightHandModel.GetComponent<Animator>().SetBool("isGrabbing", SteamVR_Controller.Input(rightHandIndex).GetHairTrigger());

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

        prevLeftTrigger = currLeftTrigger;
        prevRightTrigger = currRightTrigger;
    }
}
