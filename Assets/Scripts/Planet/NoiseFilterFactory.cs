using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NoiseFilterFactory class
public static class NoiseFilterFactory
{

    // CreateNoiseFilter method
    public static INoiseFilter CreateNoiseFilter(PlanetNoiseSettings planetNoiseSettings)
    {
        // the type of noise filter that it's going to create will depend on the filter type specified in the settings
        switch (planetNoiseSettings.filterType)
        {
            // return a simple noise filter
            case PlanetNoiseSettings.FilterType.Simple:
                return new SimpleNoiseFilter(planetNoiseSettings.simpleNoiseSettings);
            // return a rigid noise filter
            case PlanetNoiseSettings.FilterType.Ridgid:
                return new RidgidNoiseFilter(planetNoiseSettings.ridgidNoiseSettings);
        }
        return null;
    }
}