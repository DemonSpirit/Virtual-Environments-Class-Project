using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetManager : MonoBehaviour {

    [SerializeField] GameObject lHand;
    [SerializeField] GameObject rHand;

    [SerializeField] GameObject combTrump1x2;
	[SerializeField] GameObject combTrump1x3;
	[SerializeField] GameObject combTrump2x3;
	[SerializeField] GameObject combTrumpComplete;

    AudioSource trumpetAudioSource;

	// Singleton
	[HideInInspector] public static TrumpetManager singleton;
	[HideInInspector] public bool isInstantiating;

	void Start()
	{
		if (singleton == null)
			singleton = this;


	}
		
	public void InstantiateCombinedObject(GameObject obj1, GameObject obj2)
	{
		int obj1ID = obj1.GetComponent<TrumpetPiece>().id;
		int obj2ID = obj2.GetComponent<TrumpetPiece>().id;

        bool obj1Held = (obj1.transform.parent == lHand.transform || obj1.transform.parent == rHand.transform);
        bool obj2Held = (obj2.transform.parent == lHand.transform || obj2.transform.parent == rHand.transform);

        Debug.Log(obj1Held + " " + obj2Held);

        GameObject newObject;

        // 1x2
        if ( (obj1ID == 1 && obj2ID == 2) || (obj1ID == 2 && obj2ID == 1) )
		{
            newObject = combTrump1x2;
            GameManager.singleton.trumpetPieceGoToRoom(2);
		}

		// 1x3
		else if ( (obj1ID == 1 && obj2ID == 3) || (obj1ID == 3 && obj2ID == 1) )
		{
            newObject = combTrump1x3;
		}

		// 2x3
		else if ( (obj1ID == 2 && obj2ID == 3) || (obj1ID == 3 && obj2ID == 2) )
		{
            newObject = combTrump2x3;
		}

		// 1x2x3
		else 
		{
            newObject = combTrumpComplete;
        }
        if (newObject != null) newObject.GetComponent<TrumpetPiece>().hasBeenPickedUp = true;
        if (obj1Held && obj2Held)
        {
            GameObject objInLeftHand = (obj1.transform.parent == lHand.transform) ? obj1 : obj2;

            Vector2 pos = objInLeftHand.transform.position;
            Quaternion rot = objInLeftHand.transform.rotation;

            detachFromHands(obj1);
            detachFromHands(obj2);

            GameObject newPiece = Instantiate(newObject, pos, rot, objInLeftHand.transform.parent);

            lHand.GetComponent<Valve.VR.InteractionSystem.Hand>().AttachObject(newPiece);

        } else if (obj1Held)
        {
            GameObject hand = getHandHoldingObject(obj1);

            Vector2 pos = obj1.transform.position;
            Quaternion rot = obj1.transform.rotation;

            detachFromHands(obj1);

            GameObject newPiece = Instantiate(newObject, pos, rot, obj1.transform.parent);

            hand.GetComponent<Valve.VR.InteractionSystem.Hand>().AttachObject(newPiece);
        }
        else if (obj2Held)
        {
            GameObject hand = getHandHoldingObject(obj2);

            Vector2 pos = obj2.transform.position;
            Quaternion rot = obj2.transform.rotation;

            detachFromHands(obj2);

            GameObject newPiece = Instantiate(newObject, pos, rot, obj2.transform.parent);

            hand.GetComponent<Valve.VR.InteractionSystem.Hand>().AttachObject(newPiece);
        } else
        {
            Instantiate(newObject, obj1.transform.localPosition, obj1.transform.localRotation, obj1.transform.parent);
        }

        Destroy(obj1);
        Destroy(obj2);

    }

    void detachFromHands(GameObject obj)
    {
        if (obj.transform.parent == lHand.transform)
        {
            lHand.GetComponent<Valve.VR.InteractionSystem.Hand>().DetachObject(obj, true);
        }
        else if(obj.transform.parent == rHand.transform)
        {
            rHand.GetComponent<Valve.VR.InteractionSystem.Hand>().DetachObject(obj, true);
        }
    }

    GameObject getHandHoldingObject(GameObject obj)
    {
        GameObject hand = null;
        if (obj.transform.parent == lHand.transform) hand = lHand;
        if (obj.transform.parent == rHand.transform) hand = rHand;
        return hand;
    }

    public void playCompleteTrumpet(AudioSource audios)
    {
        if (!audios.isPlaying) audios.Play();
        GameManager.singleton.playOutro();
    }

    public void playTrumpetSound(AudioClip clip, AudioSource source)
    {
        stopTrumpetSound();
        trumpetAudioSource = source;
        trumpetAudioSource.PlayOneShot(clip);
    }

    public void stopTrumpetSound()
    {
        if (trumpetAudioSource != null)
        {
            if (trumpetAudioSource.isPlaying) trumpetAudioSource.Stop();
            trumpetAudioSource = null;
        }
    }

}
