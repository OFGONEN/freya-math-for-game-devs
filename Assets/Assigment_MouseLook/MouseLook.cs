using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform turret;

    public float mouseSensitivity = 1;
    public float turretYawSensitivity = 1;

    float turretYawOffsetDeg;
    float pitch;
    float yaw;

    private void Awake()
    {
        var startEuler = transform.eulerAngles;
        pitch = startEuler.x;
        yaw = startEuler.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        UpdateTurretYawInput();
        UpdateMouseLook();
        PlaceTurret();
    }

    void UpdateTurretYawInput()
    {
        float scrollDelta = Input.mouseScrollDelta.y;
        turretYawOffsetDeg += scrollDelta * turretYawSensitivity;
    }

    void UpdateMouseLook()
    {
        if (Cursor.lockState == CursorLockMode.None)
            return;
        
        float xDelta = Input.GetAxis("Mouse X");
        float yDelta = Input.GetAxis("Mouse Y");

        pitch += -yDelta * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -90, 90);
        yaw += xDelta * mouseSensitivity;

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }

    void PlaceTurret()
    {
         Ray ray = new Ray(transform.position, transform.forward);
         if (Physics.Raycast(ray, out RaycastHit hit))
         {
             turret.position = hit.point;
             Vector3 yAxis = hit.normal;
             Vector3 zAxis = Vector3.Cross(transform.right, yAxis).normalized;

             Quaternion offsetRot = Quaternion.Euler(0, turretYawOffsetDeg, 0);
             turret.rotation = Quaternion.LookRotation(zAxis, yAxis) * offsetRot;
             //Since we are compositing the second rotation it rotates the first rotation alon IT'S axis. 
             // So we are basically turning the turret along +Y in its local axies.
         }       
    }
}
