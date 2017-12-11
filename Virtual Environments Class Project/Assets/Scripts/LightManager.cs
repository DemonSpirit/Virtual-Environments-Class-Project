using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    public float intensity = 1.0f;
    private float prevIntensity = 1.0f;
    List<LightObject> lights;

    float currIntensity = 1.0f;
    float newIntensity = 1.0f;

    float intensityTimer = 0.0f;
    float intensityChangeLength = 2.0f;
    bool intensityChangeDone = true;

    // Singleton
    [HideInInspector] public static LightManager singleton;

    // Use this for initialization
    void Start ()
    {
        if (singleton == null)
            singleton = this;

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

    public void setIntensity(float ni, float il)
    {
        prevIntensity = intensity;
        newIntensity = ni;
        intensityTimer = 0.0f;
        intensityChangeLength = il;
        intensityChangeDone = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!intensityChangeDone)
        {
            intensity = Mathf.Lerp(prevIntensity, newIntensity, intensityTimer);
            if (intensityTimer <= 1)
            {
                intensityTimer += Time.deltaTime / intensityChangeLength;
                updateLights(intensity);
            } else
            {
                intensity = newIntensity;
                intensityChangeDone = true;
                updateLights(intensity);
            }
        }
    }

    void updateLights(float dointensity)
    {
        for (int i = 0; i < lights.Count; i++)
        {
            LightObject lo = lights[i];
            lo.light.intensity = lo.origIntensity / 1.0f * dointensity;
        }
    }
}

class LightObject
{
    public Light light;
    public float origIntensity;
}
