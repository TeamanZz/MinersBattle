using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleCrowdController : MonoBehaviour
{
    public PathChecker pathChecker;
    public Transform endPoint;
    public List<Transform> crowdTransforms = new List<Transform>();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (pathChecker.CheckPathExist())
            {
                for (int i = 0; i < crowdTransforms.Count; i++)
                {
                    crowdTransforms[i].GetComponent<ICrowdUnit>().SendToEndPoint(endPoint.position);
                }
            }
        }
    }
}
