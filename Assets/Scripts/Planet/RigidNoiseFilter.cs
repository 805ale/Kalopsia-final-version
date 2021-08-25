using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RigidNoiseFilter class
public class RidgidNoiseFilter : INoiseFilter
{

    PlanetNoiseSettings.RidgidNoiseSettings ridgidNoiseSettings;
    PlanetNoise planetNoise = new PlanetNoise();

    // RigidNoiseFilter constructor
    public RidgidNoiseFilter(PlanetNoiseSettings.RidgidNoiseSettings ridgidNoiseSettings)
    {
        this.ridgidNoiseSettings = ridgidNoiseSettings;
    }

    // Evaluate function
    public float Evaluate(Vector3 point)
    {
        float planetNoiseValue = 0;                                                 // noise value
        float planetFrequency = ridgidNoiseSettings.planetBaseRoughness;            // frequency
        float planetAmplitude = 1;                                                  // amplitude
        float planetWeight = 1;                                                     // weight

        for (int i = 0; i < ridgidNoiseSettings.planetNumLayers; i++)
        {
            // create peaks and ridges
            // take the noise value, get the absolute value of that, and then inverted by subtracting from one
            float v = 1 - Mathf.Abs(planetNoise.Evaluate(point * planetFrequency + ridgidNoiseSettings.planetCentre));
            // make the ridges more pronounced by squaring the value
            v *= v;
            // make the noise in the ridges more detailed compared to the noise in the valleys below
            // weigh the noise in each layer based on the layers that came before it
            // regions that start out fairly low down will remain relatively undetailed compared to regions that start higher up
            v *= planetWeight;
            planetWeight = Mathf.Clamp01(v * ridgidNoiseSettings.planetWeightMultiplier);

            planetNoiseValue += v * planetAmplitude;
            planetFrequency *= ridgidNoiseSettings.planetRoughness;
            planetAmplitude *= ridgidNoiseSettings.planetPersistence;
        }

        planetNoiseValue = planetNoiseValue - ridgidNoiseSettings.planetMinValue;
        return planetNoiseValue * ridgidNoiseSettings.planetStrength;
    }
}