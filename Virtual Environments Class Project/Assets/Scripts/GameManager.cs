using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] bool isVR;
	[SerializeField] GameObject VRCharacter;
	[SerializeField] Camera VRCam;
	[SerializeField] GameObject FPSCharacter;
	[SerializeField] Camera FPSCam;

    public GameObject roomManager;
    public GameObject lightManager;
    public GameObject soundManager;

    public int currentRoom = 0;

    void Awake()
	{
		VRCharacter.SetActive(isVR);
		FPSCharacter.SetActive(!isVR);

		if (isVR)
			RoomManager.playerCam = VRCam;
		else RoomManager.playerCam = FPSCam;
	}

	// Use this for initialization
	void Start () {
        
    }

	// Update is called once per frame
	void Update () {
        
    }

    public void trumpetPieceGoToRoom(int roomID)
    {
        if (!roomManager.GetComponent<RoomManager>().roomVisited[roomID])
        {
            roomManager.GetComponent<RoomManager>().moveToRoom(roomID);
            roomManager.GetComponent<RoomManager>().roomVisited[roomID] = true;
        }
    }
}
