using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager battleManager;
    public static bool battle = false;
    public GameObject emptyDeffender;

    [Header("Check mesh")]
    public NavMeshAgent spawnPosition;
    public NavMeshPath navMeshPath;


    [Header("Collection in castle")]
    public Transform castlePoint;
    public List<DeffenderAI> allDeffenders = new List<DeffenderAI>();
    public List<DeffenderAI> activeDeffenders = new List<DeffenderAI>();


    [Header("Collection on place")]
    public List<Transform> pointsOnPlace = new List<Transform>();
    public static int emptyPoints = 0;

    public int deffenderCount = 30;

    public Transform twoPoint;

    [Header("Attack")]
    public Transform centralEmptyPoint;
    public int i;

    public void Awake()
    {
        battleManager = this;
    }

    public void MeshCheck()
    {
        if (battle == true)
            return;

        var path = new NavMeshPath();
        spawnPosition.CalculatePath(twoPoint.position, path);

        print("New path calculated");

        if (path.status != NavMeshPathStatus.PathComplete)
        {
            Debug.Log("False");
        }
        else
        {
            Debug.Log("True");
            spawnPosition.SetDestination(twoPoint.position);
            battle = true;
        }

        SetOfWarriors();
    }

    public void DefenderCreation()
    {
        GameObject deffender;
        if (allDeffenders.Count < deffenderCount)
        {
            float radius = 1f;
            Vector3 vec = new Vector3(Random.Range(castlePoint.position.x - radius, castlePoint.position.x + radius), castlePoint.position.y, Random.Range(castlePoint.position.z - radius, castlePoint.position.z + radius));
            deffender = Instantiate(emptyDeffender, vec, Quaternion.identity);

            DeffenderAI setAI = deffender.GetComponent<DeffenderAI>();
            setAI.castlePoint = castlePoint;
            setAI.AIState = DeffenderAI.agentState.inCastle;

            allDeffenders.Add(setAI);
        }
    }

    public void Update()
    {
        Defence();
        EnemyDefence();
    }

    public void Defence()
    {
        if (emptyPoints < pointsOnPlace.Count && allDeffenders.Count > 0)
        {
            DeffenderAI deffender = allDeffenders[0];
            activeDeffenders.Add(deffender);

            deffender.placePoint = pointsOnPlace[emptyPoints];
            emptyPoints++;

            deffender.isActive = true;
            deffender.AIState = DeffenderAI.agentState.onPlace;
            allDeffenders.Remove(deffender);
        }
    }

    public void BackToTheCastles()
    {
        foreach (var deffender in activeDeffenders)
        {
            allDeffenders.Add(deffender);

            deffender.AIState = DeffenderAI.agentState.inCastle;
        }
        activeDeffenders.Clear();
        emptyPoints = 0;
    }

    public void RemovingSoldiersFromOutside()
    {
        if (emptyPoints > 0)
            emptyPoints -= 1;
    }


    public void SetOfWarriors()
    {
        if (battle == false)
            return;

        foreach (var deffender in activeDeffenders)
        {
            deffender.AIState = DeffenderAI.agentState.inAttack;
            deffender.emptyPoint = centralEmptyPoint;
        }
    }


    public void EnemyDefence()
    {
        if (emptyPoints < pointsOnPlace.Count && allDeffenders.Count > 0)
        {
            DeffenderAI deffender = allDeffenders[0];
            activeDeffenders.Add(deffender);

            deffender.placePoint = pointsOnPlace[emptyPoints];
            emptyPoints++;

            deffender.isActive = true;
            deffender.AIState = DeffenderAI.agentState.onPlace;
            allDeffenders.Remove(deffender);
        }
    }

    public void EnemyBackToTheCastles()
    {
        foreach (var deffender in activeDeffenders)
        {
            allDeffenders.Add(deffender);

            deffender.AIState = DeffenderAI.agentState.inCastle;
        }
        activeDeffenders.Clear();
        emptyPoints = 0;
    }

    public void EnemyRemovingSoldiersFromOutside()
    {
        if (emptyPoints > 0)
            emptyPoints -= 1;
    }
}
