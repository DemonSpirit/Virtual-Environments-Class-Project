using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class TrumpetScript : MonoBehaviour {

    // Flags that indicate whether the trumpet piece is active and visible
    // When active the correct notes will play otherwise fail sound.
    public bool trumpet1Active = false;
    public bool trumpet2Active = false;
    public bool trumpet3Active = false;
    public bool trumpet4Active = false;

    // Gameobject attached to the VR Camera Head to represent the mouth.
    public GameObject mouth;
    public GameObject leftHand;

    // Individual pieces of the trumpet model.
    public GameObject GOTrumpet1;
    public GameObject GOTrumpet2;
    public GameObject GOTrumpet3;
    public GameObject GOTrumpet4;

    // Individual soundclips of Trumpet notes.
    public AudioClip SndTrumpet1;
    public AudioClip SndTrumpet2;
    public AudioClip SndTrumpet3;
    public AudioClip SndTrumpet4;
    public AudioClip SndTrumpetFail;
    
    // Individual audio sources for each clip.
    // TODO: Make it work with one AudioSource.
    private AudioSource SrcTrumpet1;
    private AudioSource SrcTrumpet2;
    private AudioSource SrcTrumpet3;
    private AudioSource SrcTrumpet4;
    private AudioSource SrcTrumpetFail;

    // Variables used in the note playing.
    private float TrumpetTime = 0.0f;
    private int notePos = 0;
    private bool isPlaying = false;

    // Use this for initialization
    void Start () {
	}

    // Do this when the object is first rendered in the scene.
    void Awake()
    {
        SrcTrumpet1 = gameObject.AddComponent<AudioSource>();
        SrcTrumpet2 = gameObject.AddComponent<AudioSource>();
        SrcTrumpet3 = gameObject.AddComponent<AudioSource>();
        SrcTrumpet4 = gameObject.AddComponent<AudioSource>();
        SrcTrumpetFail = gameObject.AddComponent<AudioSource>();
        SrcTrumpet1.playOnAwake = false;
        SrcTrumpet2.playOnAwake = false;
        SrcTrumpet3.playOnAwake = false;
        SrcTrumpet4.playOnAwake = false;
        SrcTrumpetFail.playOnAwake = false;
        SrcTrumpet1.clip = SndTrumpet1;
        SrcTrumpet2.clip = SndTrumpet2;
        SrcTrumpet3.clip = SndTrumpet3;
        SrcTrumpet4.clip = SndTrumpet4;
        SrcTrumpetFail.clip = SndTrumpetFail;

        rebuildTrumpet();
    }
	
	// Update is called once per frame
	void Update () {
        // When the flag isPlaying is true, play the notes on the trumpet.
		if (isPlaying)
        {
            // Check the passing of time in seconds.
            TrumpetTime += Time.deltaTime;
            // If a second has passed, play a trumpet note.
            if (TrumpetTime > 1.0f)
            {
                TrumpetTime = 0.0f;
                playTrumpetNote();
            }
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        // If the trumpet has collided with the mouth, play all the notes.
        if (collision.gameObject == mouth)
        {
            playTrumpet();
        } else if (collision.gameObject.tag == "TrumpetPiece") // If a trumpet piece has collided with another trumpet piece, merge trumpet pieces.
        {
            mergeTrumpets(collision.gameObject);
        }
    }

    void rebuildTrumpet()
    {
        // Make visible the trumpet pieces that are active. Hide those that are inactive. Call every time those flags change.
        GOTrumpet1.SetActive(trumpet1Active);
        GOTrumpet2.SetActive(trumpet2Active);
        GOTrumpet3.SetActive(trumpet3Active);
        GOTrumpet4.SetActive(trumpet4Active);
        GOTrumpet1.GetComponent<Collider>().enabled = trumpet1Active;
        GOTrumpet2.GetComponent<Collider>().enabled = trumpet2Active;
        GOTrumpet3.GetComponent<Collider>().enabled = trumpet3Active;
        GOTrumpet4.GetComponent<Collider>().enabled = trumpet4Active;
    }

    void mergeTrumpets(GameObject piece)
    {
        // If this piece is not being held, do not run the script. The other piece will handle it.
        if (gameObject.GetComponent<NewtonVR.NVRInteractableItem>().AttachedHands.Count == 0) return;

        // TODO: If both pieces are being held, prioritise the piece being held in the left hand.
        Debug.Log(gameObject.GetComponent<NewtonVR.NVRInteractableItem>().AttachedHands[0].name);
        if (piece.GetComponent<NewtonVR.NVRInteractableItem>().AttachedHands.Count > 0)
        {
            if (gameObject.GetComponent<NewtonVR.NVRInteractableItem>().AttachedHands[0].name == "LeftHand") return;
        }

        // Add the active pieces of the other trumpet piece holder to this one.
        if (piece.GetComponent<TrumpetScript>().trumpet1Active) trumpet1Active = true;
        if (piece.GetComponent<TrumpetScript>().trumpet2Active) trumpet2Active = true;
        if (piece.GetComponent<TrumpetScript>().trumpet3Active) trumpet3Active = true;
        if (piece.GetComponent<TrumpetScript>().trumpet4Active) trumpet4Active = true;

        // One merged, destroy the other piece and rebuild this one.
        Destroy(piece);
        rebuildTrumpet();
    }

    void playTrumpet()
    {
        Debug.Log("PLAY TRUMPET");
        // Do not play if the trumpet is already playing.
        if (isPlaying) return;
        notePos = 0;
        TrumpetTime = 0.0f;
        isPlaying = true;
        playTrumpetNote();
    }

    void playTrumpetNote()
    {
        Debug.Log("PLAY TRUMPET "+notePos);
        if (notePos == 4) // If all notes played, stop playing.
        {
            isPlaying = false;
        }
        // Otherwise, play either the correct note if the piece is active, or a fail note if not active.
        switch (notePos)
        {
            case 0:
                if (trumpet1Active)
                {
                    SrcTrumpet1.Play();
                } else
                {
                    SrcTrumpetFail.Play();
                }
                break;
            case 1:
                if (trumpet2Active)
                {
                    SrcTrumpet2.Play();
                }
                else
                {
                    SrcTrumpetFail.Play();
                }
                break;
            case 2:
                if (trumpet3Active)
                {
                    SrcTrumpet3.Play();
                }
                else
                {
                    SrcTrumpetFail.Play();
                }
                break;
            case 3:
                if (trumpet4Active)
                {
                    SrcTrumpet4.Play();
                }
                else
                {
                    SrcTrumpetFail.Play();
                }
                break;
        }
        notePos++;
    }

    public void playTrumpetWithoutFail()
    {
        Debug.Log("PLAY TRUMPET");
        if (isPlaying) return;
        notePos = 0;
        TrumpetTime = 0.0f;
        isPlaying = true;
        playTrumpetNoteWithoutFail();
    }

    void playTrumpetNoteWithoutFail()
    {
        Debug.Log("PLAY TRUMPET " + notePos);
        if (notePos == 4)
        {
            isPlaying = false;
        }
        switch (notePos)
        {
            case 0:
                if (trumpet1Active)
                {
                    SrcTrumpet1.Play();
                }
                else
                {
                    playTrumpetNoteWithoutFail();
                }
                break;
            case 1:
                if (trumpet2Active)
                {
                    SrcTrumpet2.Play();
                }
                else
                {
                    playTrumpetNoteWithoutFail();
                }
                break;
            case 2:
                if (trumpet3Active)
                {
                    SrcTrumpet3.Play();
                }
                else
                {
                    playTrumpetNoteWithoutFail();
                }
                break;
            case 3:
                if (trumpet4Active)
                {
                    SrcTrumpet4.Play();
                }
                else
                {
                    playTrumpetNoteWithoutFail();
                }
                break;
        }
        notePos++;
    }
}
