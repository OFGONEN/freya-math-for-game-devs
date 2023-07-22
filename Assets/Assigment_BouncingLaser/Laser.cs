using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Vector2 origin = transform.position;
        Vector2 direction = transform.right;

        Ray ray = new Ray(origin, direction);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + direction);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Gizmos.DrawSphere(hit.point, 0.1f);

            Vector2 reflected = Reflect2D(ray.direction, hit.normal);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(hit.point, hit.point + hit.normal);
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(hit.point, (Vector2)hit.point + reflected);
        }
    }

    private Vector2 Reflect2D(Vector2 inDirection, Vector2 normal)
    {
        float projectedDistance = Vector2.Dot(inDirection, normal);
        return inDirection - 2 * projectedDistance * normal;
    }
}
