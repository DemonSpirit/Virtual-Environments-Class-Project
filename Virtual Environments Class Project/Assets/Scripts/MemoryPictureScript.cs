using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPictureScript : MonoBehaviour
{

    // Inspector fields
    [SerializeField] int roomID = 0;
    [SerializeField] Camera mainCamera;

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
    void Start()
    {
        mainCamera = RoomManager.playerCam;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool shouldPictureAppear = (GameManager.singleton.currentRoom != roomID && GameManager.singleton.hasRoomBeenVisisted(roomID));
        GetComponent<Renderer>().enabled = shouldPictureAppear;
        transform.Find("Picture").GetComponent<Renderer>().enabled = shouldPictureAppear;
        GetComponent<Collider>().isTrigger = !shouldPictureAppear;

        if (!shouldPictureAppear) return;
        // Get the 2D position of the object on the screen.
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        // Get the distance of the object from the center of the screen. 
        // On the Vive, this is x770 y840 for some reason.
        float dist = Vector2.Distance(new Vector2(screenPos.x, screenPos.y), new Vector2(770, 840));
        float distz = Vector3.Distance(mainCamera.transform.position, transform.position);

        // If the distance is less than the deadspot (close enough to directly looking at the object)

        if (CanSeeObjectV2(gameObject) && distz < 2.0f)
        {
            if (dist < selectionDeadSpot)
            {
                // If the object has already been selected, ignore.
                if (selected) return;
                // If the object was not previously being selected, reset the currTriggerTime parameter.
                if (!selecting)
                {
                    currTriggerTime = 0.0f;
                    // TODO: Check that this captures both controllers
                    SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost)).TriggerHapticPulse(1000);
                    SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost)).TriggerHapticPulse(1000);
                    LightManager.singleton.GetComponent<LightManager>().setIntensity(1.0f, 1.0f);
                }
                // If the object has been directly looked at for 3 seconds, trigger the timeline change.
                if (currTriggerTime > totalTriggerTime && !selected)
                {
                    GameManager.singleton.GetComponent<GameManager>().memoryGoToRoom(roomID);
                    LightManager.singleton.GetComponent<LightManager>().setIntensity(0.0f, 1.0f);
                    //spotlight.intensity = 0.0f;

                    selected = true;
                }
                currTriggerTime += Time.deltaTime;
                pulseInterval = 0.3f - (currTriggerTime / 1.0f);
            }
            else
            {
                //lightManager.GetComponent<LightManager>().intensity = 1.0f;
                if (selecting)
                {
                    LightManager.singleton.GetComponent<LightManager>().setIntensity(1.0f, 1.0f);
                }
                //spotlight.intensity = 0.0f;
                selecting = false;
                selected = false;
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
