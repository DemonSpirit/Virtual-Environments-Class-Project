using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static Camera playerCam;

    bool prevLeftTrigger = false;
    bool prevRightTrigger = false;

    Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    // Player camera

    [SerializeField] List<Transform> roomOrigins;
    [SerializeField] List<Transform> objectFolders;
    List<GameObject> objectsToDisappear;


    int previousRoomNumber = -1;
    public int currentRoomNumber = 0;
    bool shouldEmptyRoom;

    public bool[] roomVisited = new bool[3];

    // 0 - Childs Room
    // 1 - Adults Room
    // 2 - Carehome Room

    void Awake()
    {
        // In case the camera is not assigned
        //if (playerCam == null)
        //{
        //	playerCam = Camera.main;
        //}

    }

    void Start()
    {
        roomVisited[0] = true;
        roomVisited[1] = roomVisited[2] = false;
        objectsToDisappear = new List<GameObject>();

        //Puts every timeline object in the main room
        for (int i = 1; i < roomOrigins.Count; i++)
        {
            for (int j = 0; j < objectFolders[i].childCount; j++)
                ObjectAppearV2(objectFolders[i].GetChild(j).gameObject, false, true);

            PositionObjectFolder(i, 0);
        }
    }

    void Update()
    {

        // TODO: this is is example code, roomState will be determined by events.
        if (Input.GetKeyDown(KeyCode.K))
        {
            moveToNextRoom();
        }

        // Old objects should progressively disappear
        // New room objects progressively appear
        if (shouldEmptyRoom)
        {
            EmptyingRoom();
            FillingRoom(currentRoomNumber);
        }

    }

    public void moveToNextRoom()
    {
        int nextRoom = (currentRoomNumber + 1) % roomOrigins.Count;
        ChangeRoomState(nextRoom);
        shouldEmptyRoom = true;
    }

    public void moveToRoom(int roomID)
    {
        ChangeRoomState(roomID);
        shouldEmptyRoom = true;
    }

    // Makes sure certain new objects are not directly visible
    void ChangeRoomState(int newRoomNumber)
    {

        previousRoomNumber = currentRoomNumber;
        currentRoomNumber = newRoomNumber;

        NewRoomStart(currentRoomNumber);

    }

    // Old objects should progressively disappear
    void EmptyingRoom()
    {
        int i = 0;
        int count = objectsToDisappear.Count;

        while (i < count)
        {
            GameObject obj = objectsToDisappear[i];

            if (!CanSeeObjectV2(obj))
            {
                ObjectAppearV2(obj, false);
                objectsToDisappear.Remove(obj);
                i--;
                count--;
            }

            i++;
        }
    }

    // Called once. What happens what you enter a new room state.
    void NewRoomStart(int newRoom)
    {
        for (int i = 0; i < objectFolders[newRoom].childCount; i++)
        {
            GameObject obj = objectFolders[newRoom].GetChild(i).gameObject;



            // Makes sure certain new objects are not directly visible, but should become
            if (CanSeeObjectV2(obj))
            {
                // If we meet the same room objects again. TODO: make sure it works well with ECE and DORUK stuff
                if (ObjectIsRendered(obj))
                    continue;
                ObjectAppearV2(obj, false);
                //obj.SetActive(false);
            }

            // New objects that are not directly visible can already appear
            else
            {
                ObjectAppearV2(obj, true);
            }

        }


        for (int i = 0; i < objectFolders[previousRoomNumber].childCount; i++)
        {
            GameObject obj = objectFolders[previousRoomNumber].GetChild(i).gameObject;

            // Adds objects from previous room that you still can see but still need to be removed, to objectsToDisappear list
            if (CanSeeObjectV2(obj))
            {
                objectsToDisappear.Add(obj);
            }

            // Previous objects that you already don't see can already disappear.
            else
            {
                ObjectAppearV2(obj, false);
            }
        }

    }

    // New Roomstate progressively appear when camera is not looking directly at it
    void FillingRoom(int newRoom)
    {
        for (int i = 0; i < objectFolders[newRoom].childCount; i++)
        {
            GameObject obj = objectFolders[newRoom].GetChild(i).gameObject;

            if (!CanSeeObjectV2(obj))
            {
                ObjectAppearV2(obj, true);
                //obj.SetActive(true);
            }
        }

    }



    // Positions specific objectFolders related to a room, to it's destination.
    void PositionObjectFolder(int startRoom, int destination)
    {
        // Distance between the 2 rooms
        Vector3 dist = roomOrigins[destination].position - roomOrigins[startRoom].position;

        for (int i = 0; i < objectFolders[startRoom].childCount; i++)
        {
            GameObject obj = objectFolders[startRoom].GetChild(i).gameObject;
            //print(obj.name + " - add distance: " + dist);
            obj.transform.position += dist;
        }
    }


    // Returns true if obj is visible to the camera, false if not
    bool CanSeeObject(GameObject obj)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);
        if (GeometryUtility.TestPlanesAABB(planes, obj.GetComponent<Collider>().bounds))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    void ObjectAppear(GameObject obj, bool shouldAppear)
    {


        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
            obj.GetComponent<Renderer>().enabled = shouldAppear;

        Collider coll = obj.GetComponent<Collider>();
        if (coll != null)
            obj.GetComponent<Collider>().isTrigger = !shouldAppear;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
            obj.GetComponent<Rigidbody>().isKinematic = !shouldAppear;



        // TODO: once Ece and Doruk did fix (parent have full trigger collider)
        /*for (int i = 0; i < obj.transform.childCount; i++)
		{
			obj.transform.GetChild(i).gameObject.SetActive(shouldAppear);
		}*/
    }


    // TODO: make this work
    bool CanSeeObjectV2(GameObject obj)
    {
        Collider[] colls = obj.GetComponents<Collider>();
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);

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

    void ObjectAppearV2(GameObject obj, bool shouldAppear, bool force = false)
    {
        if (!shouldAppear && obj.tag == "Unremovable" && !force) return;
        // All colliders should be either trigger or not
        Collider[] colls = obj.GetComponents<Collider>();
        if (colls.Length > 0)
        {
            foreach (Collider coll in colls)
            {
                coll.isTrigger = !shouldAppear;
            }
        }

        // Rigidbody should be either kinematic or not
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = !shouldAppear;


        // Renderer should be either enabled or not
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
           rend.enabled = shouldAppear;
        }

		// Can enable/disable light if wanted
		Light light = obj.GetComponent<Light>();
		if (light != null)
		{
			light.enabled = shouldAppear;
		}

        if (obj.transform.childCount == 0)
            return;


        // Do the same for all children
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            ObjectAppearV2(obj.transform.GetChild(i).gameObject, shouldAppear);
        }
    }


    // Check if current obj or children objects are currently rendered
    bool ObjectIsRendered(GameObject obj)
    {
        Renderer rend = obj.GetComponent<Renderer>();

        // If obj has no renderer
        if (rend == null)
        {
            // Check if atleast one of the children is rendered
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                if (ObjectIsRendered(obj.transform.GetChild(i).gameObject))
                    return true;
            }

            // If it has no children
            // or if none of the children are rendered
            return false;
        }

        // If obj has a renderer
        else
        {
            return rend.enabled;
        }
    }
}
