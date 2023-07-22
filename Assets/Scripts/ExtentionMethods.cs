using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtentionMethods
{
    public static Vector3 SetX(this Vector3 vector, float xValue)
    {
        vector.x = xValue;
        return vector;
    }
    
    public static Vector3 SetY(this Vector3 vector, float yValue)
    {
        vector.y = yValue;
        return vector;
    }
    
    public static Vector3 SetZ(this Vector3 vector, float zValue)
    {
        vector.z = zValue;
        return vector;
    }
}