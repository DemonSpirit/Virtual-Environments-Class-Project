using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroOutroScript : MonoBehaviour {

    public AudioClip introClip;

    bool introStarted = false;
    bool introPlaying = false;
    bool outroPlaying = false;
    float introTimer = 0.0f;
    float outroTimer = 0.0f;
    bool leftTrigger = false;
    bool leftPrevTrigger = false;
    bool rightTrigger = false;
    bool rightPrevTrigger = false;

    public Texture2D introTexture;
    public Texture2D outroTexture;

    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().material.SetTexture("_MainTex", introTexture);
	}

    // Update is called once per frame
    void Update()
    {
        if (!introStarted)
        { 

            leftTrigger = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost)).GetHairTriggerDown();
            rightTrigger = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost)).GetHairTriggerDown();

            if (leftTrigger && leftTrigger != leftPrevTrigger)
            {
                playIntro();
            }
            else if (rightTrigger && rightTrigger != rightPrevTrigger)
            {
                playIntro();
            }

            leftPrevTrigger = leftTrigger;
            rightPrevTrigger = rightTrigger;

        }

        if (introPlaying)
        {
            introTimer += Time.deltaTime;

            float dissolveVal = 1.0f / 9.0f * introTimer;
            GetComponent<Renderer>().material.SetFloat("_DisVal", dissolveVal);
            if (introTimer > 9.0f)
            {
                introPlaying = false;
                GetComponent<MeshRenderer>().enabled = false;
            }
        }

        if (outroPlaying)
        {
            outroTimer += Time.deltaTime;

            float dissolveVal = 1.0f - (1.0f / 9.0f * outroTimer);
            GetComponent<Renderer>().material.SetFloat("_DisVal", dissolveVal);
            if (outroTimer > 9.0f)
            {
                outroPlaying = false;
            }
        }
    }

    void  playIntro()
    {
        introStarted = true;
        introPlaying = true;
        introTimer = 0.0f;
        GetComponent<AudioSource>().PlayOneShot(introClip);
    }

    public void playOutro()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Renderer>().material.SetTexture("_MainTex", outroTexture);
        outroTimer = 0.0f;
        outroPlaying = true;
    }
}
