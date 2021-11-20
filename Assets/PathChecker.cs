using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathChecker : MonoBehaviour
{
    [Header("Check mesh")]
    public NavMeshAgent pathAgent;
    public NavMeshPath navMeshPath;

    public Transform endPoint;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MeshCheck();
        }
    }

    public bool MeshCheck()
    {
        var path = new NavMeshPath();
        pathAgent.CalculatePath(endPoint.position, path);

        if (path.status != NavMeshPathStatus.PathComplete)
        {
            Debug.Log("Path don't exist");
            return false;
        }
        else
        {
            Debug.Log("Path exist");
            pathAgent.SetDestination(endPoint.position);
            return true;
        }
    }
}