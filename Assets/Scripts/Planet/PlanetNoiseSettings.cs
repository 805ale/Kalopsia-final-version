using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlanetNoiseSettings class
[System.Serializable]
public class PlanetNoiseSettings
{
    // List the different noise filters
    public enum FilterType { Simple, Ridgid};
    public FilterType filterType;

    [ConditionalHide("filterType", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType", 1)]
    public RidgidNoiseSettings ridgidNoiseSettings;


    // SimpleNoiseSettings class
    // Settings only available for the Simple Noise Type
    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float planetStrength = 1;            // noise strength
        [Range(1, 8)]
        public int planetNumLayers = 1;             // number of layers     
        public float planetBaseRoughness = 1;       // base roughness
        public float planetRoughness = 2;           // noise roughness
        public float planetPersistence = .5f;       // persistance
        public Vector3 planetCentre;                // centre of the noise
        public float planetMinValue;                // planet min value
    }


    // RigidNoiseSettings class
    // Settings only available for the Rigid Noise Type
    [System.Serializable]
    public class RidgidNoiseSettings : SimpleNoiseSettings
    {
        public float planetWeightMultiplier = .8f;      
    }
}