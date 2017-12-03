using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGazeTrigger : MonoBehaviour {

    // Inspector fields
    [SerializeField] int roomID = 0;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject roomController;
    [SerializeField] GameObject lightManager;
    [SerializeField] Light spotlight;
    //[SerializeField] Light 

    // Deadspots
    float deadSpot = 200.0f;
    float selectionDeadSpot = 100.0f;
    
    // Haptic Pulse Values
    private float currPulse = 0.0f;
    private float pulseTimer = 0.0f;
    private float pulseInterval = 1.0f;

    // Trigger Values
    private float currTriggerTime = 0.0f; // Length of time that the object has currently been directly looked at uninterrupeted.
    [SerializeField] float totalTriggerTime = 3.0f; // Total number of seconds that an object must be look at to trigger the change.

    // Selection booleans
    private bool selecting = false;
    private bool selected = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Get the 2D position of the object on the screen.
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        // Get the distance of the object from the center of the screen. 
        // On the Vive, this is x770 y840 for some reason.
        float dist = Vector2.Distance(new Vector2(screenPos.x, screenPos.y), new Vector2(770, 840));

        // If the distance is less than the deadspot (close enough to directly looking at the object)
        if (dist < selectionDeadSpot)
        {
            // If the object has already been selected, ignore.
            if (selected) return;
            // If the object was not previously being selected, reset the currTriggerTime parameter.
            if (!selecting)
            {
                currTriggerTime = 0.0f;

            }
            // If the object has been directly looked at for 3 seconds, trigger the timeline change.
            if (currTriggerTime > 3.0f && !selected)
            {
                roomController.GetComponent<HideScript>().moveToRoom(roomID);
                selected = true;
            }
            selecting = true;
            float range = 1.0f / 100.0f * (100.0f - dist); // Range between 0.0f and 1.0f for the focus distance
            currPulse = 3000.0f; // Set the pulse to the 
            pulseInterval = 0.3f;
            // THIS IS WHERE THE EFFECT IS SET
            lightManager.GetComponent<LightManager>().intensity = 0.0f;
            spotlight.intensity = 1.0f;
            currTriggerTime += Time.deltaTime;
            pulseInterval = 0.3f - (currTriggerTime / 1.0f);
        }
        // If the distance is less
        else if (dist < deadSpot)
        {
            float range = 1.0f / 100.0f * (100.0f - (dist - 100.0f));
            currPulse = 3000.0f * range;
            lightManager.GetComponent<LightManager>().intensity = 1.0f - range;
            spotlight.intensity = range;
            pulseInterval = 0.5f;
            selecting = false;
        }
        else
        {
            currPulse = 0.0f;
            pulseInterval = 1.0f;
            //GetComponent<Renderer>().material.color = Color.black;
            selecting = false;
            selected = false;
        }

        if (currPulse > 0.0f) {
            pulseTimer += Time.deltaTime;
            if (pulseTimer > pulseInterval) {
                Debug.Log("PULSE");
                SteamVR_Controller.Input(1).TriggerHapticPulse((ushort)currPulse);
                SteamVR_Controller.Input(3).TriggerHapticPulse((ushort)currPulse);
                pulseTimer = 0.0f;
            }
        }   
    }
}
