using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundManager : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    public float masterVolume = 1.0f;
    private float prevMasterVolume = 1.0f;

    // Adult
    [SerializeField] AudioSource adultInterior;
    [SerializeField] AudioSource adultExterior;
    // Child
    [SerializeField] AudioSource childInterior;
    [SerializeField] AudioSource childExterior;
    // Carehome
    [SerializeField] AudioSource carehomeInterior;
    [SerializeField] AudioSource carehomeExterior;

    private float interiorVolume = 0.6f;
    private float exteriorVolume = 0.05f;

    private float currAdultInteriorVol = 0.6f;
    private float currAdultExteriorVol = 0.05f;
    private float currChildInteriorVol = 0.0f;
    private float currChildExteriorVol = 0.0f;
    private float currCarehomeInteriorVol = 0.0f;
    private float currCarehomeExteriorVol = 0.0f;

    private float transitionTimer = 0.0f;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (prevMasterVolume == masterVolume) return;
        adultInterior.volume = currAdultInteriorVol / 1.0f * masterVolume;
        adultExterior.volume = currAdultExteriorVol / 1.0f * masterVolume;
        childInterior.volume = currChildInteriorVol / 1.0f * masterVolume;
        childExterior.volume = currChildExteriorVol / 1.0f * masterVolume;
        carehomeInterior.volume = currCarehomeInteriorVol / 1.0f * masterVolume;
        carehomeExterior.volume = currCarehomeExteriorVol / 1.0f * masterVolume;
        prevMasterVolume = masterVolume;
    }
}
