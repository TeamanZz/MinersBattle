using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorEnemyDetection : MonoBehaviour
{
    public Warrior warrior;

    private void Start()
    {
        InvokeRepeating("FindNewEnemyTarget", 1, 1);
    }

    public void FindNewEnemyTarget()
    {
        if (warrior.opponentTarget != null)
            return;

        var newTarget = BattleCrowdController.Instance.GetNearestOpponent(warrior, warrior.transform.position, 3);

        if (newTarget != null && newTarget.GetComponent<ICrowdUnit>().IsDeath == false)
        {
            warrior.opponentTarget = newTarget;
            Debug.Log("finded by other");
            newTarget.GetComponent<ICrowdUnit>().AddAttackerUnit(warrior.transform);
            warrior.isRuninngToCastle = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ICrowdUnit crowdUnit;
        if (other.TryGetComponent<ICrowdUnit>(out crowdUnit))
        {
            if (crowdUnit.TeamIndex != warrior.teamIndex && warrior.opponentTarget == other.transform)
            {
                warrior.opponentTarget = null;
            }
        }
    }
}