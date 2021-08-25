using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INoiseFilter interface
public interface INoiseFilter
{
    // Evaluate method
    // Any class has to have this method if they want to qualify as a noise filter
    float Evaluate(Vector3 point);
}