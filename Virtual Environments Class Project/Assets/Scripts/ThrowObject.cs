using UnityEngine;
using System.Collections;

public class ThrowObject : MonoBehaviour
{
	[SerializeField] Transform player;
	[SerializeField] Transform playerCam;
	[SerializeField] float throwForce = 10;
    bool hasPlayer = false;
	bool isBeingCarried = false;
	[SerializeField] AudioClip[] soundToPlay;
	AudioSource audioSource;
    public int dmg;
	private bool isTouched = false;

    void Start()
    {
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>() as AudioSource;
		else audioSource = GetComponent<AudioSource>();

		if (playerCam == null)
			playerCam = Camera.main.transform;
    }

    void Update()
    {
        float dist = Vector3.Distance(gameObject.transform.position, player.position);
        if (dist <= 2.5f)
        {
            hasPlayer = true;
        }
        else
        {
            hasPlayer = false;
        }
		if (hasPlayer && Input.GetMouseButtonDown(0))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = playerCam;
            isBeingCarried = true;
        }
        if (isBeingCarried)
        {
            if (isTouched)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                isBeingCarried = false;
                isTouched = false;
            }
			if (Input.GetKeyDown(KeyCode.Space))
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    transform.parent = null;
                    isBeingCarried = false;
                    GetComponent<Rigidbody>().AddForce(playerCam.forward * throwForce);
                RandomAudio();
                }
                else if (Input.GetMouseButtonDown(1))
                {
                GetComponent<Rigidbody>().isKinematic = false;
                    transform.parent = null;
                isBeingCarried = false;
                }
            }
        }
    void RandomAudio()
    {
        if (audioSource.isPlaying){
            return;
                }
        audioSource.clip = soundToPlay[Random.Range(0, soundToPlay.Length)];
        audioSource.Play();

    }
   void OnTriggerEnter()
    {
        if (isBeingCarried)
        {
            isTouched = true;
        }
    }
    }