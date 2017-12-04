using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGazeTrigger : MonoBehaviour {

    // Inspector fields
    [SerializeField] int roomID = 0;
    [SerializeField] Camera mainCamera;

    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject roomManager;
    [SerializeField] GameObject lightManager;
    [SerializeField] GameObject ambientManager;
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
	void FixedUpdate ()
    {
        if (gameManager.GetComponent<GameManager>().currentRoom == roomID) return;
        // Get the 2D position of the object on the screen.
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        // Get the distance of the object from the center of the screen. 
        // On the Vive, this is x770 y840 for some reason.
        float dist = Vector2.Distance(new Vector2(screenPos.x, screenPos.y), new Vector2(770, 840));
        float distz = Vector3.Distance(mainCamera.transform.position, transform.position);

        // If the distance is less than the deadspot (close enough to directly looking at the object)

        if (CanSeeObjectV2(gameObject) && distz < 2.0f && gameManager.GetComponent<GameManager>().hasRoomBeenVisisted(roomID))
        {
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
                if (currTriggerTime > totalTriggerTime && !selected)
                {
                    gameManager.GetComponent<GameManager>().memoryGoToRoom(roomID);
                    lightManager.GetComponent<LightManager>().intensity = 1.0f;
                    spotlight.intensity = 0.0f;

                    selected = true;
                }
                else
                {
                    //spotlight.intensity += 0.1f;
                }
                selecting = true;
                float range = 1.0f / 100.0f * (100.0f - dist); // Range between 0.0f and 1.0f for the focus distance
                currPulse = 3000.0f; // Set the pulse to the 
                pulseInterval = 0.3f;
                // THIS IS WHERE THE EFFECT IS SET
                lightManager.GetComponent<LightManager>().intensity = 0.0f;
                currTriggerTime += Time.deltaTime;
                pulseInterval = 0.3f - (currTriggerTime / 1.0f);
            }
            // If the distance is less
            else if (dist < deadSpot)
            {
                float range = 1.0f / 100.0f * (100.0f - (dist - 100.0f));
                currPulse = 3000.0f * range;
                lightManager.GetComponent<LightManager>().intensity = 1.0f - range;
                ambientManager.GetComponent<AmbientSoundManager>().masterVolume = 1.0f - range;
                spotlight.intensity = range;
                pulseInterval = 0.5f;
                selecting = false;
            }
            else
            {
                //lightManager.GetComponent<LightManager>().intensity = 1.0f;
                spotlight.intensity = 0.0f;
                currPulse = 0.0f;
                pulseInterval = 1.0f;
                selecting = false;
                selected = false;
            }
        }

        if (currPulse > 0.0f) {
            pulseTimer += Time.deltaTime;
            if (pulseTimer > pulseInterval) {
                // TODO: Check that this captures both controllers
                SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost)).TriggerHapticPulse((ushort)currPulse);
                SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost)).TriggerHapticPulse((ushort)currPulse);
                pulseTimer = 0.0f;
            }
        }
    }


    // TODO: make this work
    bool CanSeeObjectV2(GameObject obj)
    {
        Collider[] colls = obj.GetComponents<Collider>();
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        // Check if we can see atleast one collider from current object
        if (colls.Length > 0)
        {
            foreach (Collider coll in colls)
            {
                if (GeometryUtility.TestPlanesAABB(planes, coll.bounds))
                    return true;
            }
        }

        // If current object has children, do the step above recursively to each child
        if (obj.transform.childCount > 0)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                if (CanSeeObjectV2(obj.transform.GetChild(i).gameObject))
                    return true;
            }
        }

        // Can't see any colliders from current and children objects
        return false;
    }
}
