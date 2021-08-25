using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ShapeSettings class
// Scriptable object to hold the planet's shape settings
[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    public float planetRadius = 1;          // planet's radius
    public NoiseLayer[] noiseLayers;        // noise layers

    // NoiseLayer class
    [System.Serializable]
    public class NoiseLayer
    {
        // toggle the visibility of a single noise layer
        public bool enabled = true;
        // determines whether or not the first layer should be used as a mask
        public bool useFirstLayerAsMask;
        public PlanetNoiseSettings planetShapeSettings;
    }
}
