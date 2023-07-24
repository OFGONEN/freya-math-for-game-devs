using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VectorSamples : MonoBehaviour
{
    public Vector3 vector_first;
    public Vector3 vector_second;

    public VectorOperation vector_operation;
    private void OnDrawGizmos()
    {
        if (vector_operation == VectorOperation.Dot)
        {
            var dotProduct = Vector3.Dot(vector_first, vector_second);
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, vector_first);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, vector_second);           
            Gizmos.DrawWireSphere(transform.position + vector_first.normalized * dotProduct, 0.1f);
        }
        else if (vector_operation == VectorOperation.Cross)
        {
            var crossProduct = Vector3.Cross(vector_first, vector_second);
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, vector_first);
            Handles.Label(transform.position + vector_first, "First: " + vector_first);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, vector_second);           
            Handles.Label(transform.position + vector_second, "First: " + vector_second);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, crossProduct);
            Handles.Label(transform.position + crossProduct, "First: " + crossProduct);
        }
    }
}

public enum VectorOperation
{
    Dot,
    Cross
}