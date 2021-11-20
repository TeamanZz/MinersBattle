using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeffenderAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public bool isActive = false;

    public agentState AIState;
    public enum agentState
    {
        idle,
        inCastle,
        onPlace,
        inAttack
    }

    [Header("Attack")]
    public Transform emptyPoint;
    public Transform objectOfAttack;

    [Header("In the castle")]
    public Transform castlePoint;

    [Header("Collection on place")]
    public Transform placePoint;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        if (isActive == false)
            return;

        switch (AIState)
        {
            case agentState.inCastle:
                MovingToTheCastle();
                break;

            case agentState.onPlace:
                MovingToThePlace();
                break;

            case agentState.inAttack:
                SearchForTheEnemy();
                break;

            default:
                Idle();
                break;
        }
    }

    public void MovingToTheCastle()
    {
        agent.SetDestination(castlePoint.position);
    }
    public void MovingToThePlace()
    {
        agent.SetDestination(placePoint.position);
    }

    public void Idle()
    {

    }

    public void SearchForTheEnemy()
    {
        if (objectOfAttack == null)
        {
            agent.SetDestination(emptyPoint.position);
        }
        else
        {
            agent.SetDestination(objectOfAttack.position);

        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Castle" && isActive == true)
        {
            if (AIState == agentState.onPlace || AIState == agentState.inAttack)
                return;

            AIState = agentState.idle;
            BattleManager.battleManager.RemovingSoldiersFromOutside();
            isActive = false;
        }

        if (other.transform.tag == "Enemy" && BattleManager.battle == true)
        {
            objectOfAttack = other.transform;
        }
    }
}
