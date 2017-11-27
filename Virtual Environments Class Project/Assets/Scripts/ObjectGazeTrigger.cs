using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGazeTrigger : MonoBehaviour {

    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject roomController;

    float deadSpot = 200.0f;
    float selectionDeadSpot = 100.0f;
    float currPulse = 0.0f;
    float pulseTimer = 0.0f;
    float pulseInterval = 1.0f;

    float currSelection = 0.0f;

    bool selecting = false;
    bool selected = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        float dist = Vector2.Distance(new Vector2(screenPos.x, screenPos.y), new Vector2(770, 840));

            if (dist < selectionDeadSpot)
            {
                if (selected) return;
                if (!selecting)
                {
                    currSelection = 0.0f;

                }
                if (currSelection > 3.0f && !selected)
                {
                    roomController.GetComponent<HideScript>().moveToNextRoom();
                    selected = true;
                }
                selecting = true;
                float range = 1.0f / 100.0f * (100.0f - dist);
                currPulse = 3000.0f;
                pulseInterval = 0.3f;
                GetComponent<Renderer>().material.color = new Color(1.0f, currSelection/3.0f, currSelection/3.0f);
                currSelection += Time.deltaTime;
                pulseInterval = 0.3f - (currSelection / 1.0f);
            }
            else if (dist < deadSpot)
            {
                float range = 1.0f / 100.0f * (100.0f - (dist - 100.0f));
                currPulse = 3000.0f * range;
                float r = 255.0f * range;
                pulseInterval = 0.5f;
                GetComponent<Renderer>().material.color = new Color(range, 0, 0);
                selecting = false;
            }
            else
            {
                currPulse = 0.0f;
                pulseInterval = 1.0f;
                GetComponent<Renderer>().material.color = Color.black;
                selecting = false;
                selected = false;
            }

        if (currPulse > 0.0f) {
            pulseTimer += Time.deltaTime;
            if (pulseTimer > pulseInterval) {
                Debug.Log("PULSE");
                SteamVR_Controller.Input(1).TriggerHapticPulse((ushort)currPulse);
                SteamVR_Controller.Input(4).TriggerHapticPulse((ushort)currPulse);
                pulseTimer = 0.0f;
            }
        }   
    }
}
