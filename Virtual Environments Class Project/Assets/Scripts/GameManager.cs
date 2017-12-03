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

    public int currentRoom = 0;

    bool[] roomVisited = new bool[3];

    void Awake()
	{
        roomVisited[0] = roomVisited[1] = roomVisited[2] = false;
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
        if (!roomVisited[roomID])
        {
            roomManager.GetComponent<RoomManager>().moveToRoom(roomID);
        }
    }
}
