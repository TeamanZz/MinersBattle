using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class PlayerOpponent : MonoBehaviour, IAIMiner
{
    private NavMeshAgent agent;
    public Detection detectionCollider;
    public Transform minerPlatePosition;

    public RocksHandler rocksHandler;
    public Transform defaultPosition;

    public PlayerOpponentState currentState = PlayerOpponentState.Idle;
    [HideInInspector] public MiningRock targetRock;

    private Animator animator;
    [HideInInspector] public BackPack backPack;

    public MiningRock TargetRock { get => targetRock; set => targetRock = value; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        backPack = GetComponent<BackPack>();
    }

    private void Start()
    {
        rocksHandler = RocksHandler.Instance;
        rocksHandler.minersDetections.Add(detectionCollider);
        ChangeState(PlayerOpponentState.Idle);
        ChangeState(PlayerOpponentState.Mining);

        InvokeRepeating("SetNewDestination", 0.5f, 1);
    }


    private void FixedUpdate()
    {
        if ((detectionCollider.rocksNearby.Count == 0 && targetRock != null) || currentState == PlayerOpponentState.RunningToMinersPlate)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
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

    public void OnRocksFull()
    {
        ChangeState(PlayerOpponentState.RunningToMinersPlate);
    }

    //Animation event
    public void HitRocksNearby()
    {
        List<MiningRock> rocksNearbyCopy = new List<MiningRock>(detectionCollider.rocksNearby);
        for (int i = 0; i < rocksNearbyCopy.Count; i++)
        {
            rocksNearbyCopy[i].HitRock(detectionCollider.pickaxe);
        }

        // for (int i = 0; i < detectionCollider.rocksNearby.Count; i++)
        // {
        //     detectionCollider.rocksNearby[i].HitRock(detectionCollider.pickaxe);
        //     Debug.Log(detectionCollider.rocksNearby[i]);
        // }
    }

    public void ChangeState(PlayerOpponentState newState)
    {
        if (newState == PlayerOpponentState.Idle)
        {
            animator.SetBool("IsRunning", false);
            currentState = PlayerOpponentState.Idle;
        }

        if (newState == PlayerOpponentState.Mining)
        {
            // StartCoroutine(IEStartRunningAnimationAfterDelay());
            // animator.SetBool("IsRunning", true);
            SetNewDestination();
            currentState = PlayerOpponentState.Mining;
        }

        if (newState == PlayerOpponentState.RunningToStoragePlate)
        {
            animator.SetBool("IsRunning", true);
            targetRock = null;
            agent.SetDestination(StorageOpponent.Instance.transform.position);
            agent.isStopped = false;
            currentState = PlayerOpponentState.RunningToStoragePlate;
        }

        if (newState == PlayerOpponentState.Unloading)
        {
            animator.SetBool("IsRunning", false);
            agent.isStopped = true;
            currentState = PlayerOpponentState.Unloading;
        }

        if (newState == PlayerOpponentState.RunningToMinersPlate)
        {
            animator.SetBool("IsRunning", true);
            agent.isStopped = false;
            targetRock = null;
            agent.SetDestination(minerPlatePosition.position);
            currentState = PlayerOpponentState.RunningToMinersPlate;
        }
    }

    private IEnumerator IEStartRunningAnimationAfterDelay()
    {
        yield return new WaitForSeconds(0.75f);
        animator.SetBool("IsRunning", true);
    }

    private void SetNewDestination()
    {
        if (currentState != PlayerOpponentState.Mining)
            return;

        if (detectionCollider.rocksNearby.Count != 0)
            return;

        if (currentState == PlayerOpponentState.RunningToStoragePlate || backPack.isUnloading)
            return;

        if (rocksHandler.miningRocks.Count <= 0)
            return;

        var newTarget = rocksHandler.miningRocks.Find(x => Vector3.Distance(x.transform.position, transform.position) <= rocksHandler.miningRocks.Min(x => Vector3.Distance(x.transform.position, transform.position)));
        var newDestination = newTarget.transform.position;
        targetRock = newTarget.GetComponent<MiningRock>();
        targetRock.currentMiner = this;
        agent.SetDestination(newDestination);
        agent.isStopped = false;
    }

    public void OnTargetRockDestroyed()
    {
        SetNewDestination();
    }
}

public interface IAIMiner
{
    void OnTargetRockDestroyed();
    MiningRock TargetRock { get; set; }
}

public enum PlayerOpponentState
{
    Idle,
    Mining,
    Unloading,
    RunningToMinersPlate,
    RunningToStoragePlate,
    RunningToWarriorsPlate,
    RunningToArchersPlate,
}