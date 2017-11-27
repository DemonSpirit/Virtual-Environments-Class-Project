using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGazeTriggerScript : MonoBehaviour {


    public GameObject hand;
    int currStrength = 0;
    int maxStrength = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Input.GetButtonDown("14"));

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        float dist = Vector2.Distance(new Vector2(screenPos.x, screenPos.y), new Vector2(0, 0));
       
        /*if (InCameraView(gameObject))
        {
            SteamVR_Controller.Input(0).TriggerHapticPulse();
        }*/
    }

    private bool InCameraView(GameObject Object)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (GeometryUtility.TestPlanesAABB(planes, Object.GetComponent<Collider>().bounds))
            return true;
        else
            return false;
    }
}
