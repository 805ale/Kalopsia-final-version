using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TerrainFace class
// Each face is going to have its own mesh.
public class TerrainFace
{

    ShapeGenerator shapeGenerator;
    Mesh mesh;
    int resolution;         // determine how detailed the planet will look
    Vector3 localUp;        // determine which way the planet is facing
    Vector3 axisA;          // calculate axis A direction
    Vector3 axisB;          // calculate axis B direction

    // TerrainFace constructor
    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        // swap around the coordinates of the localUp
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        // find a vector which is perpendicular to both localUp and axisA
        axisB = Vector3.Cross(localUp, axisA);
    }

    // ConstructMesh method
    public void ConstructMesh()
    {
        // array that stores the vertices
        // resolution represents the number of vertices along a single edge of the face
        Vector3[] vertices = new Vector3[resolution * resolution];
        // array to hold the indices of all of the triangles
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;
        Vector2[] uv = (mesh.uv.Length == vertices.Length) ? mesh.uv : new Vector2[vertices.Length];

        // define where the vertex should be along the face
        for (int y = 0; y < resolution; y++)
        {
            // when X is equal to 0, the percent of the X axis will be equal to 0 as well
            // when X has reached its highest value, the percent will be equal to 1 on the X axis
            for (int x = 0; x < resolution; x++)
            {
                // index
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                // turn the cube into a sphere: make all of the vertices to be at the same distance away from the centre 
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                // calculate index
                float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
                vertices[i] = pointOnUnitSphere * shapeGenerator.GetScaledElevation(unscaledElevation);
                uv[i].y = unscaledElevation;

                // create trinagles as long as the current vertex is not along the right or bottom edge
                if (x != resolution - 1 && y != resolution - 1)
                {
                    // first triangle
                    triangles[triIndex] = i;                                // first vertex               
                    triangles[triIndex + 1] = i + resolution + 1;           // second vertex
                    triangles[triIndex + 2] = i + resolution;               // third vertex

                    // second triangle
                    triangles[triIndex + 3] = i;                            // first vertex
                    triangles[triIndex + 4] = i + 1;                        // second vertex
                    triangles[triIndex + 5] = i + resolution + 1;           // third vertex
                    triIndex += 6;                                          // add 6 vertices to the triIndex
                }
            }
        }
        // clear all data from the mesh
        mesh.Clear();
        // assign the data to the mesh
        mesh.vertices = vertices;          
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.uv = uv;
    }

    // UpdateUVs method
    // For each vertex in the planet, calculate which biome it's in and store that information in the UV channel so that can be passed into the shader
    public void UpdateUVs(ColourGenerator colourGenerator)
    {
        Vector2[] uv = mesh.uv;

        // loop through and find all of the points on the unit sphere
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                uv[i].x = colourGenerator.BiomePercentFromPoint(pointOnUnitSphere);
            }
        }
        mesh.uv = uv;
    }
}