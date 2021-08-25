using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// MinMax class
// Get highest and lowest points of the planet in order to work with the shader
public class MinMax
{
    // min point
    public float Min { get; private set; }
    // max point
    public float Max { get; private set; }

    // MinMax constructor
    public MinMax()
    {
        Min = float.MaxValue;
        Max = float.MinValue;
    }


    // AddValue method
    public void AddValue(float v)
    {
        if (v > Max)
        {
            Max = v;
        }
        if (v < Min)
        {
            Min = v;
        }
    }
}
