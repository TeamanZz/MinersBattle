using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Miner : MonoBehaviour
{
    private NavMeshAgent agent;

    public float minX;
    public float maxX;

    public float minZ;
    public float maxZ;

    public MiningRock targetRock;
    public Detection detectionCollider;

    public RocksHandler rocksHandler;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rocksHandler.minersDetections.Add(detectionCollider);
    }

    void Start()
    {
        InvokeRepeating("SetNewDestination", 0.5f, 1);
    }



    private void SetNewDestination()
    {
        if (detectionCollider.rocksNearby.Count != 0)
            return;
        Debug.Log(rocksHandler.miningRocks.Min(x => x.transform.position.z));

        var newTarget = rocksHandler.miningRocks.Find(x => Vector3.Distance(x.transform.position, transform.position) <= rocksHandler.miningRocks.Min(x => Vector3.Distance(x.transform.position, transform.position)));
        newTarget.gameObject.SetActive(true);
        var newDestination = newTarget.transform.position;
        targetRock = newTarget.GetComponent<MiningRock>();
        targetRock.currentMiner = this;
        agent.SetDestination(newDestination);
        agent.isStopped = false;

        // Vector3 newDestination = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
        // agent.SetDestination(newDestination);
    }

    public void OnRockDestroyed()
    {
        // detectionCollider.ClearAll();
        SetNewDestination();
    }

    private void OnCollisionEnter(Collision other)
    {
        MiningRock miningRock;
        if (other.gameObject.TryGetComponent<MiningRock>(out miningRock))
        {
            if (miningRock == targetRock)
                agent.isStopped = true;

        }
    }

    // private void OnCollisionExit(Collision other)
    // {
    //     MiningRock miningRock;
    //     if (other.gameObject.TryGetComponent<MiningRock>(out miningRock))
    //     {
    //         SetNewDestination();
    //         agent.isStopped = false;
    //     }
    // }

}