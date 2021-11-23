using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Miner : MonoBehaviour, IAIMiner
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

    public MiningRock TargetRock { get => targetRock; set => targetRock = value; }

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

    //Animation event
    public void HitRocksNearby()
    {
        for (int i = 0; i < detectionCollider.rocksNearby.Count; i++)
        {
            detectionCollider.rocksNearby[i].HitRock(detectionCollider.pickaxe);
        }
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

        if (rocksHandler.miningRocks.Count <= 0)
            return;

        var newTarget = rocksHandler.miningRocks.Find(x => Vector3.Distance(x.transform.position, transform.position) <= rocksHandler.miningRocks.Min(x => Vector3.Distance(x.transform.position, transform.position)));
        // newTarget.gameObject.SetActive(true);
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