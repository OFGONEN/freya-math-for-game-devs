using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assignment.RadialTrigger
{
    public class Barrel : MonoBehaviour
    {
        public float trigger_radius = 1f;
        public Transform trigger_target;
        private void OnDrawGizmos()
        {
            var distance = Vector3.Distance(transform.position, trigger_target.position);
            var trigger = distance <= trigger_radius;

            Handles.color = trigger ? Color.red : Color.green;
        
            Handles.DrawWireArc(transform.position, Vector3.back, Vector3.left, 360f, trigger_radius);
            Handles.Label(transform.position + Vector3.up * trigger_radius / 2f, "Trigger: " + (trigger ? "On" : "Off"));
        }
    }    
}