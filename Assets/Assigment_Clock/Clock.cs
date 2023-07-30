using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [Range(0, 0.2f)]
    public float tickSizeSecMin = 0.2f;
    [Range(0, 0.2f)]
    public float tickSizeHour = 0.2f;

    public bool use24HClock;

    private int HoursOnClock => use24HClock ? 24 : 12;

    Vector2 AngleToDirection(float angleInRadians) => new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
    private void OnDrawGizmos()
    {
        Handles.matrix = Gizmos.matrix = transform.localToWorldMatrix;
        Handles.DrawWireDisc(default, Vector3.forward, 1);

        for (int i = 0; i < 60; i++)
        {
            var dir = SecondsOrMinutesToDirection(i);
            DrawTick(dir, tickSizeSecMin, 1 );
        }
        
        for (int i = 0; i < HoursOnClock; i++)
        {
            var dir = HourToDirection(i);
            DrawTick(dir, tickSizeHour, 3 );
        }

        var time = DateTime.Now;
        DrawHand(SecondsOrMinutesToDirection(time.Second), 0.9f, 1, Color.red);
        DrawHand(SecondsOrMinutesToDirection(time.Minute), 0.7f, 4, Color.white);
        DrawHand(HourToDirection(time.Hour), 0.5f, 8, Color.yellow);
    }

    void DrawHand(Vector2 dir, float length, float thickness, Color color)
    {
        Handles.color = color;
        Handles.DrawLine(default, dir * length, thickness);
    }
    
    void DrawTick(Vector2 direction, float length, float thickness)
    {
        Handles.DrawLine(direction, direction * (1 - length), thickness); 
    }

    Vector2 HourToDirection(float hour)
    {
        return ValueToDirection(hour, HoursOnClock);
    }
    
    Vector2 SecondsOrMinutesToDirection(float secondOrMin)
    {
        return ValueToDirection(secondOrMin, 60);
    }

    Vector2 ValueToDirection(float value, float valueMax)
    {
        float t = value / valueMax;
        return FractionToClockDir(t); 
    }
    
    Vector2 FractionToClockDir(float t)
    {
        float angleRadians = (0.25f - t) * 2 * Mathf.PI;
        return AngleToDirection(angleRadians);
    }
}