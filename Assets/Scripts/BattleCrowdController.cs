using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class BattleCrowdController : MonoBehaviour
{
    public PathChecker pathChecker;
    public Transform playerCastle;
    public Transform enemyCastle;

    public List<Transform> playerCrowdTransforms = new List<Transform>();
    public List<Transform> enemyCrowdTransforms = new List<Transform>();

    public static BattleCrowdController Instance;

    public bool canRunToCastle;

    private void Awake()
    {
        Instance = this;
    }

    public Transform GetOpponentCastleTransform(int teamIndex)
    {
        if (teamIndex == 0)
            return enemyCastle;
        else
            return
            playerCastle;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CheckPath();
        }
    }

    public Transform GetNearestOpponent(ICrowdUnit crowdUnit, Vector3 currentPosition, float detectionRange)
    {
        if (crowdUnit.TeamIndex == 0)
        {
            if (enemyCrowdTransforms.Count == 0)
                return null;

            var newTargetPos = enemyCrowdTransforms.Find(x => Vector3.Distance(x.transform.position, currentPosition) <= enemyCrowdTransforms.Min(x => Vector3.Distance(x.transform.position, currentPosition)));
            if (Vector3.Distance(newTargetPos.position, currentPosition) <= detectionRange)
            {
                return newTargetPos;
            }
            else
                return null;
        }
        else
        {
            if (playerCrowdTransforms.Count == 0)
                return null;

            var newTargetPos = playerCrowdTransforms.Find(x => Vector3.Distance(x.position, currentPosition) <= playerCrowdTransforms.Min(x => Vector3.Distance(x.position, currentPosition)));
            if (Vector3.Distance(newTargetPos.position, currentPosition) <= detectionRange)
            {
                return newTargetPos;
            }
            else
                return null;
        }
    }

    private IEnumerator CheckPathAfterDelay()
    {
        if (!canRunToCastle)
        {
            yield return new WaitForSeconds(0.1f);
            if (PathChecker.Instance.CheckPathExist())
            {
                canRunToCastle = true;
                SendPlayerUnitsToEnemyCastle();
                SendEnemyUnitsToPlayerCastle();
            }
        }
    }

    public void CheckPath()
    {
        StartCoroutine(CheckPathAfterDelay());
    }

    public void SendPlayerUnitsToEnemyCastle()
    {
        for (int i = 0; i < playerCrowdTransforms.Count; i++)
        {
            playerCrowdTransforms[i].GetComponent<ICrowdUnit>().SendToOpponentCastle();
        }
    }

    public void SendEnemyUnitsToPlayerCastle()
    {
        for (int i = 0; i < enemyCrowdTransforms.Count; i++)
        {
            enemyCrowdTransforms[i].GetComponent<ICrowdUnit>().SendToOpponentCastle();
        }
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         if (pathChecker.CheckPathExist())
    //         {
    //             for (int i = 0; i < playerCrowdTransforms.Count; i++)
    //             {
    //                 playerCrowdTransforms[i].GetComponent<ICrowdUnit>().SendToOpponentCastle(enemyCastle.position);
    //             }
    //         }
    //     }
    // }
}
