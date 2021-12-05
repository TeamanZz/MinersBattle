using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemyDetection : MonoBehaviour
{
    public Archer archer;

    private void Start()
    {
        InvokeRepeating("FindNewEnemyTarget", 1, 1);
    }

    public void FindNewEnemyTarget()
    {
        if (archer.opponentTarget != null)
            return;

        if (archer == null)
            return;
        ICrowdUnit crowdComponent = archer.GetComponent<ICrowdUnit>();
        var newTarget = BattleCrowdController.Instance.GetNearestOpponent(crowdComponent, archer.transform.position, 8);

        if (newTarget != null)
        {
            archer.opponentTarget = newTarget;
            newTarget.GetComponent<ICrowdUnit>().AddAttackerUnit(archer.transform);
            archer.isRuninngToCastle = false;
            archer.animator.SetBool("HaveTarget", true);
            archer.animator.SetBool("IsRunning", false);
            archer.agent.isStopped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ICrowdUnit crowdUnit;
        if (other.TryGetComponent<ICrowdUnit>(out crowdUnit))
        {
            if (crowdUnit.TeamIndex != archer.teamIndex && archer.opponentTarget == other.transform)
            {
                archer.animator.SetBool("HaveTarget", false);
                archer.opponentTarget = null;
            }
        }
    }
}