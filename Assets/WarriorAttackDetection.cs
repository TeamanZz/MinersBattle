using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarriorAttackDetection : MonoBehaviour
{
    public Warrior warrior;

    private void OnTriggerEnter(Collider other)
    {
        ICrowdUnit crowdUnit;
        if (other.TryGetComponent<ICrowdUnit>(out crowdUnit))
        {
            if (crowdUnit.TeamIndex != warrior.teamIndex)
            {
                warrior.opponentTarget = other.transform;
                other.GetComponent<Warrior>().whoAttackThisWarrior.Add(warrior);
                warrior.agent.isStopped = true;
                warrior.isRuninngToCastle = false;
                warrior.animator.SetBool("EnemyIsNearby", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ICrowdUnit crowdUnit;
        if (other.TryGetComponent<ICrowdUnit>(out crowdUnit))
        {
            // //Если таргет умер
            // if (warrior.enemyTarget == null)
            // {
            //     warrior.animator.SetBool("EnemyIsNearby", false);
            //     return;
            // }
            //Если таргет просто решил побежать за другим
            if (crowdUnit.TeamIndex != warrior.teamIndex && other.transform == warrior.opponentTarget)
            {
                warrior.animator.SetBool("EnemyIsNearby", false);
                warrior.agent.isStopped = false;
            }
        }
    }
}