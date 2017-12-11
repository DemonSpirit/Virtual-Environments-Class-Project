using UnityEngine;
using System.Collections;

public class RecordPlayer : MonoBehaviour {
//--------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------

    public bool recordPlayerActive = false;

    GameObject disc;
    GameObject arm;

    int mode;
    float armAngle;
    float discAngle;
    float discSpeed;

    public GameObject lHand;
    public GameObject rHand;
    GameObject currVinyl;
    public GameObject trumpetPiece;

    float trumpetCreateTimer = 0.0f;
    bool trumpetPieceCreated = false;

//--------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------
void Awake()
{
    disc = gameObject.transform.Find("teller").gameObject;
    arm = gameObject.transform.Find("arm").gameObject;
}
//--------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------
void Start()
{
    mode = 0;
    armAngle = 0.0f;
    discAngle = 0.0f;
    discSpeed = 0.0f;
}
//--------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------
void Update()
{
        if (currVinyl != null)
        {
            if (Handscript.singleton.leftHandTriggerDown)
            {
                if (transform.Find("teller").GetComponent<Collider>().bounds.Contains(lHand.transform.position))
                {
                    removeVinyl(lHand);
                }
            } else if (Handscript.singleton.rightHandTriggerDown)
            {
                if (transform.Find("teller").GetComponent<Collider>().bounds.Contains(rHand.transform.position))
                {
                    removeVinyl(rHand);
                }
            }
        }

    //-- Mode 0: player off
    if(mode == 0)
    {   
        if(recordPlayerActive == true)
            mode = 1;
    }
    //-- Mode 1: activation
    else if(mode == 1)
    {
        if(recordPlayerActive == true)
        {
            armAngle += Time.deltaTime * 30.0f;
            if(armAngle >= 30.0f)
            {
                armAngle = 30.0f;
                mode = 2;
            }
            discAngle += Time.deltaTime * discSpeed;
            discSpeed += Time.deltaTime * 80.0f;
        }
        else
            mode = 3;
    }
    //-- Mode 2: running
    else if(mode == 2)
    {
            if (recordPlayerActive == true)
            {
                if (!GetComponent<AudioSource>().isPlaying && currVinyl != null)
                {
                    GetComponent<AudioSource>().PlayOneShot(currVinyl.GetComponent<VinylRecordScript>().song);
                    trumpetCreateTimer = 0.0f;
                } else if (GetComponent<AudioSource>().isPlaying && currVinyl != null)
                {
                    if (currVinyl.tag == "GoldenVinyl" && !trumpetPieceCreated)
                    {
                        if (trumpetCreateTimer < 9.0f)
                        {
                            trumpetCreateTimer += Time.deltaTime;
                        } else
                        {
                            trumpetPieceCreated = true;
                            Instantiate(trumpetPiece, new Vector3(transform.position.x, 2.5f, transform.position.z), Quaternion.identity, transform.parent);
                            GetComponent<AudioSource>().Stop();
                            recordPlayerActive = false;
                        }
                        
                    }
                }
                discAngle += Time.deltaTime * discSpeed;
            }
            else
            {
                mode = 3;
            }
    }
    //-- Mode 3: stopping
    else
    {
        if(recordPlayerActive == false)
        {
            if (GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Stop();
            armAngle -= Time.deltaTime * 30.0f;
            if(armAngle <= 0.0f)
                armAngle = 0.0f;

            discAngle += Time.deltaTime * discSpeed;
            discSpeed -= Time.deltaTime * 80.0f;
            if(discSpeed <= 0.0f)
                discSpeed = 0.0f;

            if((discSpeed == 0.0f) && (armAngle == 0.0f))
                mode = 0;
        }
        else
            mode = 1;
    }

    //-- update objects
    arm.transform.localEulerAngles = new Vector3(0.0f, armAngle, 0.0f);
    disc.transform.localEulerAngles = new Vector3(0.0f, discAngle, 0.0f);
    }

    public void addVinyl(GameObject vinyl)
    {
        if (currVinyl) removeVinyl();
        currVinyl = vinyl;
        vinyl.GetComponent<Collider>().enabled = false;
        vinyl.transform.SetParent(gameObject.transform.Find("teller").transform);
        vinyl.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        vinyl.transform.localPosition = new Vector3(0.0f, 0.002f, 0.0f);
        vinyl.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
        vinyl.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        recordPlayerActive = true;
    }

    public void removeVinyl(GameObject hand = null)
    {
        recordPlayerActive = false;
        currVinyl.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        currVinyl.GetComponent<Collider>().enabled = true;
        currVinyl.transform.SetParent(gameObject.transform.parent);
        if (GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Stop();
        if (hand != null)
        {
            hand.GetComponent<Valve.VR.InteractionSystem.Hand>().AttachObject(currVinyl);
        }
        currVinyl = null;
    }
    //--------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------
}
