using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] bool isVR;
	[SerializeField] GameObject VRCharacter;
	[SerializeField] GameObject FPSCharacter;

    public GameObject roomManager;
    public GameObject lightManager;
    public GameObject soundManager;

    void Awake()
	{
		VRCharacter.SetActive(isVR);
		FPSCharacter.SetActive(!isVR);
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
            soundManager.GetComponent<AmbientSoundManager>().setRoom(roomID);
        }
    }

    public void memoryGoToRoom(int roomID)
    {
        roomManager.GetComponent<RoomManager>().moveToRoom(roomID);
        roomManager.GetComponent<RoomManager>().roomVisited[roomID] = true;
        soundManager.GetComponent<AmbientSoundManager>().setRoom(roomID);
    }

    public bool hasRoomBeenVisisted(int roomID)
    {
        return roomManager.GetComponent<RoomManager>().roomVisited[roomID];
    }

    public void setRoomAsVisited(int roomID)
    {
        roomManager.GetComponent<RoomManager>().roomVisited[roomID] = true;
    }

    public int currentRoom
    {
        get
        {
            return roomManager.GetComponent<RoomManager>().currentRoomNumber;
        }
    }
}
