using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundManager : MonoBehaviour {

    // Adult
    [SerializeField] AudioSource adultInterior;
    [SerializeField] AudioSource adultExterior;
    // Child
    [SerializeField] AudioSource childInterior;
    [SerializeField] AudioSource childExterior;
    // Carehome
    [SerializeField] AudioSource carehomeInterior;
    [SerializeField] AudioSource carehomeExterior;

    private float indoorVolume = 0.6f;
    private float outdoorVolume = 0.05f;

    private float currAdultIndoorVol = 0.0f;
    private float currAdultOutdoorVol = 0.0f;
    private float currChildIndoorVol = 0.0f;
    private float currChildOutdoorVol = 0.0f;
    private float currCarehomeIndoorVol = 0.0f;
    private float currCarehomeOutdoorVol = 0.0f;

    private float currAdultIndoorVolDiff = 0.0f;
    private float currAdultOutdoorVolDiff = 0.0f;
    private float currChildIndoorVolDiff = 0.0f;
    private float currChildOutdoorVolDiff = 0.0f;
    private float currCarehomeIndoorVolDiff = 0.0f;
    private float currCarehomeOutdoorVolDiff = 0.0f;

    private float transitionTimer = 0.0f;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		//if (adultInterior.volume < currAdultIndoorVol) adultInterior.volume 
	}

    public void transitionVolume(int roomID, float time = 3.0f)
    {

    }

    public void setCurrentVolume(float vol)
    {

    }

    public void transitionToRoom(int roomID, float time = 3.0f)
    {
        switch (roomID)
        {
            case 0:
                //currAdultIndoorVol
                break;
        }
    }
}
