using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Detection : MonoBehaviour
{
    [Range(0, 360)] public float ViewAngle = 90f;
    public float ViewDistance = 15f;
    public Transform eye;
    public Transform Target;

    private bool IsInView() // true если цель видна
    {
        float realAngle = Vector3.Angle(eye.forward, Target.position - eye.position);
        RaycastHit hit;
        if (Physics.Raycast(eye.transform.position, Target.position - eye.position, out hit, ViewDistance))
        {
            if (realAngle < ViewAngle / 2f && Vector3.Distance(eye.position, Target.position) <= ViewDistance && hit.transform == Target.transform)
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        DrawViewState();
    }

    private void DrawViewState()
    {
        Vector3 left = eye.position + Quaternion.Euler(new Vector3(0, ViewAngle / 2f, 0)) * (eye.forward * ViewDistance);
        Vector3 right = eye.position + Quaternion.Euler(-new Vector3(0, ViewAngle / 2f, 0)) * (eye.forward * ViewDistance);
        Debug.DrawLine(eye.position, left, Color.yellow);
        Debug.DrawLine(eye.position, right, Color.yellow);
    }
}