using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCheckerAgentCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PathCheckerAgent agent;
        if (other.TryGetComponent<PathCheckerAgent>(out agent))
        {
            Destroy(agent.gameObject);
            PathChecker.Instance.OnTruePathExist();
        }
    }
}