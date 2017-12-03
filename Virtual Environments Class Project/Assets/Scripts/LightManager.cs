using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    public float intensity = 1.0f;
    private float prevIntensity = 1.0f;
    List<LightObject> lights;

    // Use this for initialization
    void Start ()
    {
        lights = new List<LightObject>();

        Object[] allLightObjects = Resources.FindObjectsOfTypeAll(typeof(Light));
        for (int i = 0; i < allLightObjects.Length; i++)
        {
            Light l = (Light)allLightObjects[i];
            if (l.tag != "ObjectSpotlight")
            {
                LightObject lo = new LightObject();
                lo.light = l;
                lo.origIntensity = lo.light.intensity;
                lights.Add(lo);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (prevIntensity == intensity) return;

        for (int i = 0; i < lights.Count; i++)
        {
            LightObject lo = lights[i];
            lo.light.intensity = lo.origIntensity / 1.0f * intensity;
        }

        prevIntensity = intensity;
    }
}

class LightObject
{
    public Light light;
    public float origIntensity;
}
