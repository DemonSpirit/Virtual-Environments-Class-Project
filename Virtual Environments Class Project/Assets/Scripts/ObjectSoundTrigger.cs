using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSoundTrigger : MonoBehaviour
{

    // Inspector fields
    Camera mainCamera;
    
    // Deadspots
    float deadSpot = 200.0f;
    public int roomID;
    bool soundPlayed = false;

    // Use this for initialization
    void Start()
    {
        mainCamera = RoomManager.playerCam;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!soundPlayed) {
            // Get the 2D position of the object on the screen.
            Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
            // Get the distance of the object from the center of the screen. 
            // On the Vive, this is x770 y840 for some reason.
            float dist = Vector2.Distance(new Vector2(screenPos.x, screenPos.y), new Vector2(770, 840));
            float distz = Vector3.Distance(mainCamera.transform.position, transform.position);

            // If the distance is less than the deadspot (close enough to directly looking at the object)

            if (CanSeeObjectV2(gameObject) && distz < 2.0f && roomID == GameManager.singleton.currentRoom)
            {
                if (dist < deadSpot)
                {
                    switch(roomID)
                    {
                        case 0:
                            GameManager.singleton.introOutroSphere.GetComponent<IntroOutroScript>().playChildSound();
                            break;
                        case 1:
                            GameManager.singleton.introOutroSphere.GetComponent<IntroOutroScript>().playAdultSound();
                            break;
                        case 2:
                            GameManager.singleton.introOutroSphere.GetComponent<IntroOutroScript>().playOldSound();
                            break;
                    }
                    soundPlayed = true;
            }
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
