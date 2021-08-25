using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ShapeGenerator class

public class ShapeGenerator
{

    ShapeSettings shapeSettings;            // shape settings
    INoiseFilter[] noiseFilters;            // array of INoiseFilters
    public MinMax elevationMinMax;          // min and max values of planet points

    // UpdateSettings method
    public void UpdateSettings(ShapeSettings shapeSettings)
    {
        this.shapeSettings = shapeSettings;
        // assign to the noise filters array
        noiseFilters = new INoiseFilter[shapeSettings.noiseLayers.Length];

        // initialize noise filters
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(shapeSettings.noiseLayers[i].planetShapeSettings);
        }
        elevationMinMax = new MinMax();
    }

    // CalculateUnscaledElevation method
    public float CalculateUnscaledElevation(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;          // store value of the first layer so that it can be used as a mask
        float elevation = 0;                // elevation set to 0

        // loop through all noise filters
        if(noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if(shapeSettings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if(shapeSettings.noiseLayers[i].enabled)
            {
                // the value of mask will depend on whether or not the current noise layer is using the first layer as a mask 
                // if it is, the mask will be equal to the first layer value
                // otherwise, it will be equal to 1, which means there's no mask
                float mask = (shapeSettings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }

        // calculate final elevation
        elevationMinMax.AddValue(elevation);
        return elevation;
    }

    public float GetScaledElevation(float unscaledElevation)
    {
        float elevation = Mathf.Max(0, unscaledElevation);
        elevation = shapeSettings.planetRadius * (1 + elevation);
        return elevation;
    }
}