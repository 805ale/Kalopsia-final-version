using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Planet class
public class Planet : MonoBehaviour
{

    [Range(2, 256)]                             // max amount of vertices in a mesh
    public int resolution = 10;                 // resolution
    public bool autoUpdate = true;

    // Give the option to render just a specific part of the planet
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back};
    public FaceRenderMask faceRenderMask;

    // Link to shape and colour settings
    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    // Don't display settings in inspector
    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colourSettingsFoldout;

    ShapeGenerator shapeGenerator = new ShapeGenerator();           // shape generator
    ColourGenerator colourGenerator = new ColourGenerator();        // colour generator

    [SerializeField, HideInInspector]
    // Create 6 mesh filters for displaying our terrain faces
    MeshFilter[] meshFilters;         
    // Create an array of terrain faces
    TerrainFace[] terrainFaces;

    
    // Initialize method
    void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colourGenerator.UpdateSettings(colourSettings);

        // only reinitialize the mesh filters if the array is null or if it has no elements
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            // only create a new mesh object if meshFilters array is null
            if (meshFilters[i] == null)
            {
                // create new game object
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                // add mesh renderer
                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            // assign materials to each of the meshes
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

            // have each terrain face have access to the shape generator
            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);

            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    // GeneratePlanet method
    // Generate the planet
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    // OnShapeSettingsUpdated method
    // Update shape settings
    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    // OnColourSettingsUpdated method
    // Update colour settings
    public void OnColourSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }

    // GenerateMesh method
    // Loop through all of the terrain faces
    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
            }
        }
        // update the elevation 
        colourGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    //GenerateColours method
    // Loop through the meshes and set the materials colour to the colour that is in the settings
    void GenerateColours()
    {
        colourGenerator.UpdateColours();
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].UpdateUVs(colourGenerator);
            }
        }
    }
}