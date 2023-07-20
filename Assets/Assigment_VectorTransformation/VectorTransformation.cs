using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assignment.VectorTransformation 
{
    public class VectorTransformation : MonoBehaviour
    {
        public VectorTransformationType vectorTransformationType;

        public Vector3 input_localPosition;
        public Vector3 input_worldPosition;

        private void OnDrawGizmos()
        {
            if (vectorTransformationType == VectorTransformationType.LocalToWorld)
            {
                var right = transform.right * input_localPosition.x;
                var up = transform.up * input_localPosition.y;

                var worldPosition = transform.position + right + up;
                Handles.DrawLine(Vector3.zero, worldPosition);
                Gizmos.DrawSphere(worldPosition, 0.1f);
            }
            else if (vectorTransformationType == VectorTransformationType.WorldToLocal)
            {
                var offsetVector = input_worldPosition - transform.position;

                var xValue = Vector3.Dot(transform.right, offsetVector);
                var yValue = Vector3.Dot(transform.up, offsetVector);

                var localPosition = new Vector3(xValue, yValue);
            
                Gizmos.DrawSphere(input_worldPosition, 0.1f);
                Gizmos.DrawLine(transform.position, input_worldPosition);
                Handles.Label(input_worldPosition, "LocalPosition: " + localPosition);
            }
        }
    }

    public enum VectorTransformationType
    {
        LocalToWorld,
        WorldToLocal
    }
}
