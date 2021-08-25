using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SimpleNoiseFilter class
// Processing of the PlanetNoise script
public class SimpleNoiseFilter : INoiseFilter
{

    PlanetNoiseSettings.SimpleNoiseSettings simpleNoiseSettings;
    PlanetNoise planetNoise = new PlanetNoise();

    // SimpleNoiseFilter constructor
    public SimpleNoiseFilter(PlanetNoiseSettings.SimpleNoiseSettings simpleNoiseSettings)
    {
        this.simpleNoiseSettings = simpleNoiseSettings;
    }

    // Evaluate method
    // Evaluate the noise
    public float Evaluate(Vector3 point)
    {
        // set noise value to 0
        float planetNoiseValue = 0;

        // set frequency and amplitude
        float planetFrequency = simpleNoiseSettings.planetBaseRoughness;
        float planetAmplitude = 1;

        // when roughness is a value greater than 1, the frequency will increase with each layer
        // when persistance is a value less than 1, the amplitude will decrease with each layer
        for (int i = 0; i < simpleNoiseSettings.planetNumLayers; i++)
        {
            float v = planetNoise.Evaluate(point * planetFrequency + simpleNoiseSettings.planetCentre);
            planetNoiseValue += (v + 1) * .5f * planetAmplitude;
            planetFrequency *= simpleNoiseSettings.planetRoughness;
            planetAmplitude *= simpleNoiseSettings.planetPersistence;
        }

        // this planet min value parameter makes the terrain recede into the base sphere of the planet
        planetNoiseValue = planetNoiseValue - simpleNoiseSettings.planetMinValue;
        return planetNoiseValue * simpleNoiseSettings.planetStrength;
    }
}
