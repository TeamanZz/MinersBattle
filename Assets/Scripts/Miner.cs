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
    public bool isMovingToCastle;

    public MiningRock TargetRock { get => targetRock; set => targetRock = value; }

    public int teamIndex;

    public Vector3 rockDestination;

    public float distanceToTargetRock;
    public Vector3 lastStuckPosition = Vector3.zero;

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
        InvokeRepeating("MoveToCastleRepeatedly", 5f, 1);
        InvokeRepeating("CheckOnFreeze", 5f, 1);
    }

    private void CheckOnFreeze()
    {
        if (targetRock != null && detectionCollider.rocksNearby.Count == 0)
        {
            if (Vector3.Distance(transform.position, lastStuckPosition) <= 2)
            {
                Debug.Log("NEW DEST");
                agent.isStopped = false;
                // SetNewDestination();
            }
        }
        lastStuckPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // MoveToCastle();
        }
    }

    //Animation event
    public void HitRocksNearby()
    {
        List<MiningRock> rocksNearbyCopy = new List<MiningRock>(detectionCollider.rocksNearby);
        for (int i = 0; i < rocksNearbyCopy.Count; i++)
        {
            rocksNearbyCopy[i].HitRock(detectionCollider.pickaxe);
        }
    }

    public void MoveToCastleRepeatedly()
    {
        if (isMovingToCastle)
            MoveToCastle();
    }

    public void MoveToCastle()
    {
        isMovingToCastle = true;
        targetRock = null;
        Vector3 destination;
        if (teamIndex == 0)
            destination = BattleCrowdController.Instance.playerCastle.position;
        else
            destination = BattleCrowdController.Instance.enemyCastle.position;

        agent.SetDestination(destination);
        agent.isStopped = false;
        if (Vector3.Distance(transform.position, destination) <= 1)
        {
            Destroy(gameObject);
        }
    }

    public void MoveToStorage()
    {
        isMovingToStorage = true;
        targetRock = null;

        if (teamIndex == 0)
            agent.SetDestination(Storage.Instance.transform.position);
        else
            agent.SetDestination(StorageOpponent.Instance.transform.position);
        rockDestination = default;
        agent.isStopped = false;
    }

    private void SetNewDestination()
    {
        if (detectionCollider.rocksNearby.Count != 0)
            return;

        if (isMovingToStorage || backPack.isUnloading || isMovingToCastle)
            return;

        if (rocksHandler.miningRocks.Count <= 0)
            return;

        distanceToTargetRock = Vector3.Distance(transform.position, rockDestination);
        if (targetRock != null)
        {
            if (distanceToTargetRock < 2 || distanceToTargetRock > 5)
                return;
        }

        var newTarget = rocksHandler.miningRocks.Find(x => Vector3.Distance(x.transform.position, transform.position) <= rocksHandler.miningRocks.Min(x => Vector3.Distance(x.transform.position, transform.position)));
        // newTarget.gameObject.SetActive(true);
        rockDestination = newTarget.transform.position;
        targetRock = newTarget.GetComponent<MiningRock>();
        targetRock.currentMiner = this;
        // NavMeshPath path = new NavMeshPath();
        // NavMesh.CalculatePath(transform.position, rockDestination, NavMesh.AllAreas, path);
        // agent.SetPath(path);
        agent.SetDestination(rockDestination);
        agent.isStopped = false;
    }

    public void OnTargetRockDestroyed()
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