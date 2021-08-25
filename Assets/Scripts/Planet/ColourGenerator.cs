using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ColourGenerator class
public class ColourGenerator
{
    ColourSettings settings;            // reference to colour settings
    Texture2D texture;                  // texture
    const int textureResolution = 50;   // resolution of the texture
    INoiseFilter biomeNoiseFilter;      // biome noise filter

    // UpdateSettings method
    public void UpdateSettings(ColourSettings settings)
    {
        this.settings = settings;
        // the texture is being updated if the texture is null and if the number the biomes changes
        if (texture == null || texture.height != settings.biomeColourSettings.biomes.Length)
        {
            // texture set to have a height that corresponds to the number of biomes that each row of the texture can store the colours for that biome
            texture = new Texture2D(textureResolution * 2, settings.biomeColourSettings.biomes.Length, TextureFormat.RGBA32, false);
        }
        // assign the biome noise filter
        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColourSettings.noise);
    }

    //UpdateElevation method
    // Send information to the shader
    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }


    // BiomePercentFromPoint method
    // Return 0 if first biome
    // Return 1 if last biome
    // Return 0-1 if other biomes
    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        // height percent has a value of 0 at the planet's South Pole and a value of 1 at the planet's North Pole
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColourSettings.noiseOffset) * settings.biomeColourSettings.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = settings.biomeColourSettings.biomes.Length;
        // range of the blend
        float blendRange = settings.biomeColourSettings.blendAmount / 2f + .001f;

        for (int i = 0; i < numBiomes; i++)
        {
            // calculate the distance of the biomes start height from the current height percent
            float dst = heightPercent - settings.biomeColourSettings.biomes[i].startHeight;
            // create a weight that will depend on whether the distance is within the range of the blending distance
            float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }

        // return number of biomes
        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    }

    // UpdateColours
    public void UpdateColours()
    {
        // create a colour array
        Color[] colours = new Color[texture.width * texture.height];
        // set colour index to 0
        int colourIndex = 0;
        // loop through all of the biomes 
        foreach (var biome in settings.biomeColourSettings.biomes)
        {
            for (int i = 0; i < textureResolution * 2; i++)
            {
                Color gradientCol;
                if (i < textureResolution)
                {
                    // sample from ocean gradient
                    gradientCol = settings.oceanColour.Evaluate(i / (textureResolution - 1f));
                }
                else
                {
                    // sample from biome gradient
                    gradientCol = biome.gradient.Evaluate((i - textureResolution) / (textureResolution - 1f));
                }
                Color tintCol = biome.tint;
                // if there's no tint, the colour will be entirely based on the gradient colour
                colours[colourIndex] = gradientCol * (1 - biome.tintPercent) + tintCol * biome.tintPercent;
                colourIndex++;
            }
        }
        // pass settings to shader
        texture.SetPixels(colours);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}