using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSwapScript : MonoBehaviour
{

    public Camera playerHead;
    public GameObject football_shelf;
    public GameObject football_floor;
    private bool footballOnShelf = true;
    private bool changeEnabled = true;
    private int outOfSightCount = 0;

    private void Start()
    {

    }

    private void Update()
    {
        if (!I_Can_See(football_floor) && !I_Can_See(football_shelf))
        {
            if (changeEnabled)
            {
                outOfSightCount++;
                if (outOfSightCount > 50)
                {
                    if (!footballOnShelf)
                    {
                        football_shelf.SetActive(true);
                        football_floor.SetActive(false);
                        footballOnShelf = true;
                    }
                    else
                    {
                        football_shelf.SetActive(false);
                        football_floor.SetActive(true);
                        footballOnShelf = false;
                    }
                    outOfSightCount = 0;
                    changeEnabled = false;
                }
            }
        }
        else
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
