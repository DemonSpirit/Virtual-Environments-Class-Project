using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideScript : MonoBehaviour {

    public Camera playerHead;
    public GameObject plant;
    private bool plantVisible = true;
    private bool changeEnabled = true;
    private int outOfSightCount = 0;

    private void Start()
    {
		if (playerHead == null)
			playerHead = Camera.main;
    }

    private void Update()
    {
		

        if (!I_Can_See(plant))
        {
            if (changeEnabled)
            {
                outOfSightCount++;
                if (outOfSightCount > 50)
                {
                    if (!plantVisible)
                    {
                        plant.SetActive(true);
                        plantVisible = true;
                    }
                    else
                    {
                        plant.SetActive(false);
                        plantVisible = false;
                    }
                    outOfSightCount = 0;
                    changeEnabled = false;
                }
            }
        } else
        {
            outOfSightCount = 0;
            changeEnabled = true;
        }
    }

    private bool I_Can_See(GameObject Object)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerHead);
        if (GeometryUtility.TestPlanesAABB(planes, Object.GetComponent<Collider>().bounds))
            return true;
        else
            return false;
    }
}
