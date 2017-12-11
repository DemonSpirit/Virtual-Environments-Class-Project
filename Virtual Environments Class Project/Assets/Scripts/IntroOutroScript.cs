using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroOutroScript : MonoBehaviour
{

    public AudioClip introClip;
    public AudioClip outroClip;
    public AudioClip childClip;
    public AudioClip adultClip;
    public AudioClip oldClip;
    public AudioClip applauseClip;

    bool childClipPlayed = false;
    bool adultClipPlayed = false;
    bool oldClipPlayed = false;

    bool introStarted = false;
    bool introPlaying = false;
    bool outroPlaying = false;
    bool playModePlaying = false;
    bool experienceFinished = false;
    float introTimer = 0.0f;
    float outroTimer = 0.0f;
    float playModeTimer = 0.0f;
    bool leftTrigger = false;
    bool leftPrevTrigger = false;
    bool rightTrigger = false;
    bool rightPrevTrigger = false;

    public Texture2D introTexture;
    public Texture2D outroTexture;

    // Use this for initialization
    void Start()
    {
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

            if (outroTimer > 10.0)
            {
                float dissolveVal = 1.0f - (1.0f / 5.0f * (outroTimer - 10.0f));
                GetComponent<Renderer>().material.SetFloat("_DisVal", dissolveVal);
                if (outroTimer > 15.0f)
                {
                    outroPlaying = false;
                    experienceFinished = true;
                }
            }
        }

        if (experienceFinished == true)
        {
            leftTrigger = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost)).GetHairTriggerDown();
            rightTrigger = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost)).GetHairTriggerDown();

            if (leftTrigger && leftTrigger != leftPrevTrigger)
            {
                goToPlaymode();
            }
            else if (rightTrigger && rightTrigger != rightPrevTrigger)
            {
                goToPlaymode();
            }

            leftPrevTrigger = leftTrigger;
            rightPrevTrigger = rightTrigger;
        }

        if (playModePlaying)
        {
            playModeTimer += Time.deltaTime;

            float dissolveVal = 1.0f / 5.0f * playModeTimer;
            GetComponent<Renderer>().material.SetFloat("_DisVal", dissolveVal);
            if (playModeTimer > 5.0f)
            {
                playModePlaying = false;
                GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    void playIntro()
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
        GetComponent<AudioSource>().clip = applauseClip;
        GetComponent<AudioSource>().PlayDelayed(2.0f);
    }

    public void goToPlaymode()
    {
        experienceFinished = false;
        playModePlaying = true;
        playModeTimer = 0.0f;
    }

    public void playChildSound()
    {
        if (childClipPlayed) return;
        childClipPlayed = true;
        GetComponent<AudioSource>().PlayOneShot(childClip);
    }

    public void playAdultSound()
    {
        if (adultClipPlayed) return;
        adultClipPlayed = true;
        GetComponent<AudioSource>().PlayOneShot(adultClip);
    }

    public void playOldSound()
    {
        if (oldClipPlayed) return;
        oldClipPlayed = true;
        GetComponent<AudioSource>().PlayOneShot(oldClip);
    }
}
