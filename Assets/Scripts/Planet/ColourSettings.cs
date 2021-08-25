using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ColourSettings class
// Scriptable object to hold the planet's colour settings
[CreateAssetMenu()]
public class ColourSettings : ScriptableObject
{

    public Material planetMaterial;                     // planet material
    public BiomeColourSettings biomeColourSettings;     // biome colour settings
    public Gradient oceanColour;                        // ocean colour

    // BiomeColourSettings class
    [System.Serializable]
    public class BiomeColourSettings
    {
        public Biome[] biomes;
        public PlanetNoiseSettings noise;       // biome noise settings
        public float noiseOffset;               // biome noise offset
        public float noiseStrength;             // biome noise strength
        [Range(0, 1)]
        public float blendAmount;               // biome noise blend amount

        // Biome subclass
        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;           // biome gradient
            public Color tint;
            [Range(0, 1)]
            public float startHeight;           // biome start height
            [Range(0, 1)]
            public float tintPercent;           // biome tint percent
        }
    }
}