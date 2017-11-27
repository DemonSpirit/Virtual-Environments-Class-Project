using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideScript : MonoBehaviour {

	public GameObject left_hand;
    public GameObject right_hand;

    bool prevLeftTrigger = false;
    bool prevRightTrigger = false;

    Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	// Player camera
	[SerializeField] Camera playerCam;

	[SerializeField] List<Transform> roomOrigins;
	[SerializeField] List<Transform> objectFolders;
	bool[] alreadyPositioned;

	int previousRoomNumber = -1;
	int currentRoomNumber = 0;
	bool shouldEmptyRoom;

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
		alreadyPositioned = new bool[roomOrigins.Count];
		alreadyPositioned[0] = true;
    }

    void Update()
    {
        //bool currLeftTrigger = left_hand.GetComponent<SteamVR_TrackedController>().triggerPressed;
        //bool currRightTrigger = right_hand.GetComponent<SteamVR_TrackedController>().triggerPressed;

        //bool triggerPressed = false;// ((currLeftTrigger && !prevLeftTrigger) || (currRightTrigger && !prevRightTrigger));

        // TODO: this is is example code, roomState will be determined by events.
        if (Input.GetKeyDown(KeyCode.Space))// || triggerPressed)
		{
            moveToNextRoom();
		}

		// Old room should progressively disappear
		// New one progressively appear
		if (shouldEmptyRoom)
		{
			EmptyingRoom(previousRoomNumber);
			FillingRoom(currentRoomNumber);
		}

        //prevLeftTrigger = currLeftTrigger;
        //prevRightTrigger = currRightTrigger;
		
    }

    public void moveToNextRoom()
    {
        int nextRoom = (currentRoomNumber + 1) % roomOrigins.Count;
        ChangeRoomState(nextRoom);
        shouldEmptyRoom = true;
    }

	// Positions new Object folder to the main room (= 0)
	// Makes sure certain new objects are not directly visible
	void ChangeRoomState(int newRoomNumber)
	{
        if (newRoomNumber == currentRoomNumber)
            return;

		if (!alreadyPositioned[newRoomNumber])
			PositionObjectFolder(newRoomNumber, 0);
		previousRoomNumber = currentRoomNumber;
		currentRoomNumber = newRoomNumber;
		NewRoomStart(currentRoomNumber);

	}
		
	// Desactivates Objects of old Roomstate that are not visible to the camera
	void EmptyingRoom(int roomNumber)
	{
		for (int i = 0; i < objectFolders[roomNumber].childCount; i++)
		{
			GameObject obj = objectFolders[roomNumber].GetChild(i).gameObject;

			if (!CanSeeObject(obj))
			{
				//PositionObject(obj, 0, currentRoomNumber);
				ObjectAppear(obj, false);
				//obj.SetActive(false);
			}

			/*else if (CanSeeObject(obj))
			{
				print(obj.name + "'s collider is on screen");
			}*/
		}
	}

	// Makes sure certain new objects are not directly visible
	void NewRoomStart(int newRoom)
	{
		for (int i = 0; i < objectFolders[newRoom].childCount; i++)
		{
			GameObject obj = objectFolders[newRoom].GetChild(i).gameObject;

			if (CanSeeObject(obj))
			{
				ObjectAppear(obj, false);
				//obj.SetActive(false);
			}
		}
	}

	// New Roomstate progressively appear when camera is not looking directly at it
	void FillingRoom(int newRoom)
	{
		for (int i = 0; i < objectFolders[newRoom].childCount; i++)
		{
			GameObject obj = objectFolders[newRoom].GetChild(i).gameObject;

			if (!CanSeeObject(obj))
			{
				ObjectAppear(obj, true);
				//obj.SetActive(true);
			}
		}
				
	}
		
	// Returns true if obj is visible to the camera, false if not
    bool CanSeeObject(GameObject obj)
    {
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);
        if (GeometryUtility.TestPlanesAABB(planes, obj.GetComponent<Collider>().bounds))
            return true;
        else
            return false;
    }

	// Positions specific objectFolders related to a room, to it's destination.
	void PositionObjectFolder(int startRoom, int destination)
	{
		// Distance between the 2 rooms
		Vector3 dist = roomOrigins[destination].position - roomOrigins[startRoom].position; 

		for (int i = 0; i < objectFolders[startRoom].childCount; i++)
		{
			GameObject obj = objectFolders[startRoom].GetChild(i).gameObject;
			obj.transform.position += dist;
		}

		alreadyPositioned[startRoom] = true;
	}
		
	void PositionObject(GameObject obj, int startRoom, int destination){
		// Distance between the 2 rooms
		Vector3 dist = roomOrigins[destination].position - roomOrigins[startRoom].position; 
		obj.transform.position += dist;
	}
		
	void ObjectAppear(GameObject obj, bool shouldAppear)
	{
		/*string isApearring = "disappearing";
		if (shouldAppear)
			isApearring = "appearing";
		print(obj.name + " is " + isApearring);*/

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











	bool CanSeeObjectV2(GameObject obj)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);
		Collider[] colls = obj.GetComponents<Collider>();
		if (colls.Length > 0)
		{
			foreach(Collider coll in colls)
			{
				if (GeometryUtility.TestPlanesAABB(planes, obj.GetComponent<Collider>().bounds))
					return true;
			}
		}

		return false;
	}

	void ObjectAppearV2(GameObject obj, bool shouldAppear)
	{
		// All colliders should be either trigger or not
		Collider[] colls = obj.GetComponents<Collider>();
		if (colls.Length > 0)
		{
			foreach(Collider coll in colls)
			{
				coll.isTrigger = !shouldAppear;
			}
		}

		// Rigidbody should be either kinematic or not
		Rigidbody rb = obj.GetComponent<Rigidbody>();
		if (rb != null)
			obj.GetComponent<Rigidbody>().isKinematic = !shouldAppear;

		Renderer rend = obj.GetComponent<Renderer>();

		// Renderer should be either enabled or not
		if (rend != null)
		{
			obj.GetComponent<Renderer>().enabled = shouldAppear;
			// it must be the last child I guess
			return;
		}

		if (obj.transform.childCount == 0)
			return;

		// Do the same for all childs
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			ObjectAppearV2(obj.transform.GetChild(i).gameObject, shouldAppear);
		}
	}
}
