using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Miner : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;

    public float minX;
    public float maxX;

    public float minZ;
    public float maxZ;

    public MiningRock targetRock;
    public Detection detectionCollider;

    public RocksHandler rocksHandler;
    public BackPack backPack;

    public bool isMovingToStorage;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        backPack = GetComponent<BackPack>();
    }

    void Start()
    {
        rocksHandler = RocksHandler.Instance;
        rocksHandler.minersDetections.Add(detectionCollider);
        InvokeRepeating("SetNewDestination", 0.5f, 1);
    }

    public void MoveToStorage()
    {
        isMovingToStorage = true;
        targetRock = null;

        agent.SetDestination(Storage.Instance.transform.position);
        agent.isStopped = false;
    }

    private void SetNewDestination()
    {
        if (detectionCollider.rocksNearby.Count != 0)
            return;

        if (isMovingToStorage || backPack.isUnloading)
            return;

        var newTarget = rocksHandler.miningRocks.Find(x => Vector3.Distance(x.transform.position, transform.position) <= rocksHandler.miningRocks.Min(x => Vector3.Distance(x.transform.position, transform.position)));
        newTarget.gameObject.SetActive(true);
        var newDestination = newTarget.transform.position;
        targetRock = newTarget.GetComponent<MiningRock>();
        targetRock.currentMiner = this;
        agent.SetDestination(newDestination);
        agent.isStopped = false;
    }

    public void OnRockDestroyed()
    {
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
}