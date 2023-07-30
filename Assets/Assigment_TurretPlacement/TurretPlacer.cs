using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TurretPlacer : MonoBehaviour
{
    public Transform turret;

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, transform.forward);
    
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var up = hit.normal;
            var forward = Vector3.Cross(transform.right, up).normalized;
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(hit.point, up);
    
            Gizmos.color = Color.red;
            Gizmos.DrawRay(hit.point, Vector3.Cross(up, forward));
            
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(hit.point, forward);
    
            turret.position = hit.point;
            turret.rotation = Quaternion.LookRotation(forward, up);
        }
    }
}
